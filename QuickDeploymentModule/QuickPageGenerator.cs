using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Sage.Platform.FileSystem.Interfaces;
using Sage.Platform.Projects.Interfaces;
using Sage.Platform.Projects.Localization;
using Sage.Platform.TemplateSupport;
using Sage.Platform.WebPortal.Design;
using QuickDeploymentModule.Properties;
using Sage.Platform.FileSystem;

namespace QuickDeploymentModule
{
    public class QuickPageGenerator
    {
        internal const string CONST_PAGETEMPLATE = "PortalPageAspx.vm";
        internal const string CONST_PAGETEMPLATE_CODE = "DefaultCSharpCodeBehind.vm";
        readonly IProject _project;
        Hashtable _context;
        ITemplate _markupTemplate;
        ITemplate _codeTemplate;
        readonly GenTools _tools;

        public QuickPageGenerator(IProject project)
        {
            _project = project;
            LoadTemplate();
            _tools = new GenTools();
        }

        internal void GeneratePage(PortalPage page, TextWriter markupWriter, TextWriter codeBehindWriter)
        {
            InitContext(page);
            _markupTemplate.ApplyTemplate(markupWriter, _context);
            _codeTemplate.ApplyTemplate(codeBehindWriter, _context);
            markupWriter.Flush();
            codeBehindWriter.Flush();
        }

        void InitContext(PortalPage page)
        {
            _context = new Hashtable();
            _context.Add("page", page);
            _context.Add("tools", _tools);
        }

        void LoadTemplate()
        {
            _markupTemplate = PortalUtil.LoadTemplate(_project, CONST_PAGETEMPLATE);
            _codeTemplate = PortalUtil.LoadTemplate(_project, CONST_PAGETEMPLATE_CODE);
        }
    }

    internal class QuickPageGenerationProvider : GeneratorProviderBase, IShadowCopyProvider
    {
        readonly QuickPageGenerator _gen;
        readonly PortalPage _page;

        internal QuickPageGenerationProvider(QuickPageGenerator generator, PortalPage page, PortalApplication portal)
            : base(portal)
        {
            _gen = generator;
            _page = page;
        }

        IShadowCopyItem[] IShadowCopyProvider.GetItems(BackgroundWorker worker)
        {
            worker.ReportProgress(-1, String.Format(Resources.GeneratingPortalPage, _page.PageAlias));

            string pageName = _page.PageAlias + ".aspx";
            string csName = pageName + ".cs";
            string fullPageName = GetBaseOutputCacheFolder() + pageName;
            string fullCsName = GetBaseOutputCacheFolder() + csName;

            if (PageNeedsToBeGenerated(fullPageName, fullCsName))
                GenerateMarkupAndCode(fullPageName, fullCsName);

            var items = new List<IShadowCopyItem>
                {
                    new LazyShadowCopyItem(GetBaseOutputCacheFolder() + pageName, pageName),
                    new LazyShadowCopyItem(GetBaseOutputCacheFolder() + csName, csName)
                };

            var childResources = _page.SmartParts.ToDictionary(part => part.SmartPartId, part => part.LocalResources);
            DateTime LatestResxChange = GetLatestResxChange(childResources.Values.ToList());

            Dictionary<string, byte[]> mergedResources = ProjectResourceManager.FlattenResources(
                _page.PageResources, pageName, childResources, new Dictionary<string, string>());

            foreach (KeyValuePair<string, byte[]> merged in mergedResources)
            {
                string relativePath = Path.Combine("App_LocalResources", merged.Key);
                string fullName = Path.Combine(GetBaseOutputCacheFolder(), relativePath);
                var resxFile = FileSystem.GetFileInfo(fullName);
                if (ResxFileNeedsRegeneration(resxFile, LatestResxChange))
                {
                    if (!resxFile.Directory.Exists)
                        resxFile.Directory.Create();
                    using(var stream = resxFile.OpenWrite())
                    {
                        stream.Write(merged.Value, 0, merged.Value.Length);
                    }
                }
                items.Add(new LazyShadowCopyItem(fullName, relativePath));
            }
            return items.ToArray();
        }

        private DateTime GetLatestResxChange(List<ProjectResourceManager> childResources)
        {
            childResources.Add(_page.PageResources);
            return childResources
                .SelectMany(resMgr => resMgr.ResourceDirectory.GetFiles("*.resx"))
                .Select(file => file.LastWriteTimeUtc)
                .Max();
        }

        private bool ResxFileNeedsRegeneration(IFileInfo resxFile, DateTime latestSourceChange)
        {
            if (!resxFile.Exists)
                return true;

            return latestSourceChange > resxFile.LastWriteTimeUtc;
        }

        private bool PageNeedsToBeGenerated(string fullPageName, string fullCsName)
        {
            var markupFile = FileSystem.GetFileInfo(fullPageName);
            var csFile = FileSystem.GetFileInfo(fullCsName);

            if (!markupFile.Exists || !csFile.Exists)
                return true;

            var affectingSourceFiles = new List<IFileInfo>() { _page.FilePath, _portal.FilePath };
            affectingSourceFiles.AddRange(_page.SmartParts.Select(smartPart => smartPart.FilePath));
            affectingSourceFiles.AddRange(_page.GetAllModules().Select(module => module.FilePath));
            affectingSourceFiles.AddRange(_portal.Tasklets.Select(tasklet => tasklet.FilePath));
            affectingSourceFiles.AddRange(_portal.NavigationGroups.Select(navGroup => navGroup.FilePath));
            affectingSourceFiles.AddRange(_portal.MenuItems.Select(menuItem => menuItem.FilePath));
            
            DateTime latestSourceChange = affectingSourceFiles.Select(file => file.LastWriteTimeUtc).Max();
            DateTime latestCachedCopy = markupFile.LastWriteTimeUtc > csFile.LastWriteTimeUtc
                                            ? markupFile.LastWriteTimeUtc
                                            : csFile.LastWriteTimeUtc;

            return latestSourceChange > latestCachedCopy;
        }

        private void GenerateMarkupAndCode(string pageFileName, string csFileName)
        {
            var markupFile = FileSystem.GetFileInfo(pageFileName);
            if (!markupFile.Directory.Exists)
                markupFile.Directory.Create();
            var csFile = FileSystem.GetFileInfo(csFileName);
            if (!csFile.Directory.Exists)
                csFile.Directory.Create();
            using(var markupStream = markupFile.OpenWrite())
            using(var csStream = csFile.OpenWrite())
            using(var markupWriter = new StreamWriter(markupStream))
            using(var csWriter = new StreamWriter(csStream))
            {
                _gen.GeneratePage(_page, markupWriter, csWriter);
            }
        }
    }
}
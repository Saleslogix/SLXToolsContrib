using System;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Sage.Platform.Application;
using Sage.Platform.IDEModule;
using Sage.Platform.Projects;
using Sage.Platform.Projects.Interfaces;
using Sage.Platform.Projects.Localization;
using Sage.Platform.QuickForms;
using SageApp = Sage.Platform.Application;
using Sage.Platform.Application.UI.WinForms;
using ApplicationContext=Sage.Platform.Application.ApplicationContext;

namespace QuickFormDiff
{
    public partial class frmQFDiff : Form
    {
        private WorkItem _baseWorkItem;
        private WorkItem _customWorkItem;
        private WorkItem _currentWorkItem;
        private string _baseProjectPath;
        private string _customizedProjectPath;
        private string _currentProjectPath;
        private string _quickFormPath;
        private IQuickFormDefinition _baseQuickForm;
        private IQuickFormDefinition _customQuickForm;
        private IQuickFormDefinition _currentQuickForm;
        private QuickFormComparison _baseToCustomComparison;
        private QuickFormComparison _baseToCurrentComparison;
        private QuickFormReader _baseFormReader;
        private QuickFormReader _customFormReader;
        private QuickFormReader _currentFormReader;

        public frmQFDiff()
        {
            InitializeComponent();
            SetupApplicationContext();
            tabs.Selected += tabs_Selected;
        }

        void tabs_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == null || e.TabPage.Controls.Count == 0)
                return;

            FormDesignerHost designer = e.TabPage.Controls[0] as FormDesignerHost;
            if (designer != null)
            {
                ApplicationContextDefiler.SetRootWorkItemProjectLocalizationService(designer.WorkItem);
            }
        }

        private void SetupApplicationContext()
        {
            SageApp.ApplicationContext.Initialize(Assembly.GetExecutingAssembly().GetName().Name);
            _baseWorkItem = SetupWorkItem();
            _customWorkItem = SetupWorkItem();
            _currentWorkItem = SetupWorkItem();
        }

        private static WorkItem SetupWorkItem()
        {
            WorkItem workItem = SageApp.ApplicationContext.Current.WorkItems.AddNew(typeof(WorkItem));
            workItem.Services.AddNew(typeof(TypeResolutionService), typeof(ITypeResolutionService));
            workItem.Services.AddNew(typeof(SelectionService), typeof(ISelectionService));
            workItem.Services.AddNew(typeof(BrowsableObjectService), typeof(SageApp.IBrowsableObjectService));
            return workItem;
        }

        private static IProject SetupProject(WorkItem workItem, string projectPath)
        {
            var modelTypes = new ModelTypeCollection();
            modelTypes.Add(new ModelType(typeof(QuickFormModel)));
            var projectWorkspace = new ProjectWorkspace(projectPath);
            IProject project = new Project(projectWorkspace, modelTypes);

            IProjectContextService pcs = new SimpleProjectContextService(project);
            RemoveServiceFromWorkItem<IProjectContextService>(workItem);
            workItem.Services.Add(pcs);

            RemoveServiceFromWorkItem<ProjectLocalizationService>(workItem);
            workItem.Services.Add(typeof (ProjectLocalizationService), new ProjectLocalizationService(pcs));
            ApplicationContextDefiler.SetRootWorkItemProjectLocalizationService(workItem);

            return project;
        }        

        private static void RemoveServiceFromWorkItem<TService>(WorkItem workItem)
        {
            TService dummy;
            if (workItem.Services.TryGetService<TService>(out dummy))
                workItem.Services.Remove(typeof(TService));
        }

        private static IQuickFormDefinition LoadQuickForm(WorkItem workItem, string projectPath, string quickFormPath)
        {
            IProject project = SetupProject(workItem, projectPath);
            var qfModel = project.Models.Get<QuickFormModel>();
            IQuickFormDefinition form = qfModel.LoadDefinition(quickFormPath);
            var dummy = form.LocalResources; //force loading of LocalizationService while project context is correct
            return form;
        }

        private string BaseProjectPath
        {
            get { return _baseProjectPath; }
            set
            {
                _baseProjectPath = txtBasePath.Text = value;
                btnSetQuickFormPath.Enabled = true;
            }
        }

        private bool BaseProjectPathIsSet
        {
            get { return !string.IsNullOrEmpty(BaseProjectPath); }
        }

        public string CustomizedProjectPath
        {
            get { return _customizedProjectPath; }
            set { _customizedProjectPath = txtCustomizedPath.Text = value; }
        }

        private bool CustomizedProjectPathIsSet
        {
            get { return !string.IsNullOrEmpty(CustomizedProjectPath); }
        }

        public string CurrentProjectPath
        {
            get { return _currentProjectPath; }
            set { _currentProjectPath = txtCurrentPath.Text = value; }
        }

        private bool CurrentProjectPathIsSet
        {
            get { return !string.IsNullOrEmpty(CurrentProjectPath); }
        }

        public string QuickFormPath
        {
            get { return _quickFormPath; }
            set { _quickFormPath = txtQuickFormPath.Text = value; }
        }

        private void btnSetCustomizedPath_Click(object sender, EventArgs e)
        {
            CustomizedProjectPath = SetProjectPath(CustomizedProjectPath);
        }

        private void btnSetCurrentPath_Click(object sender, EventArgs e)
        {
            CurrentProjectPath = SetProjectPath(CurrentProjectPath);
        }

        private void btnSetBasePath_Click(object sender, EventArgs e)
        {
            BaseProjectPath = SetProjectPath(BaseProjectPath);
        }

        private string SetProjectPath(string currentPath)
        {
            if (!string.IsNullOrEmpty(currentPath))
                dlgBrowseFolder.SelectedPath = currentPath;

            if (dlgBrowseFolder.ShowDialog() == DialogResult.OK)
                return NormalizeProjectPath(dlgBrowseFolder.SelectedPath);

            return currentPath;
        }
        
        private static string NormalizeProjectPath(string path)
        {
            if (!path.EndsWith("\\Model", StringComparison.InvariantCultureIgnoreCase))
            {
         
                var folder = new DirectoryInfo(Path.Combine(path, "Model"));
                if (folder.Exists)
                    path = folder.FullName;
            }

            return path;
        }

        private void btnSetQuickFormPath_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(QuickFormPath))
                dlgSelectFile.InitialDirectory = Path.GetDirectoryName(GetFullQuickFormPathFromRelativePath(QuickFormPath));
            else if (!string.IsNullOrEmpty(BaseProjectPath))
                dlgSelectFile.InitialDirectory = BaseProjectPath;

            if (dlgSelectFile.ShowDialog() == DialogResult.OK)
                QuickFormPath = GetRelativeQuickFormPathFromFullPath(dlgSelectFile.FileName);
        }        

        private string GetFullQuickFormPathFromRelativePath(string relativeQFPath)
        {
            return Path.Combine(BaseProjectPath, relativeQFPath);
        }

        private string GetRelativeQuickFormPathFromFullPath(string fullQFPath)
        {
            return fullQFPath.Substring(BaseProjectPath.Length);
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            _baseQuickForm = LoadQuickForm(_baseWorkItem, BaseProjectPath, QuickFormPath);
            _baseFormReader = new QuickFormReader(_baseQuickForm);
            _baseFormReader.ReadAllProperties();

            if (CustomizedProjectPathIsSet)
            {
                _customQuickForm = LoadQuickForm(_customWorkItem, CustomizedProjectPath, QuickFormPath);
                _customFormReader = new QuickFormReader(_customQuickForm);
                _customFormReader.ReadAllProperties();

                _baseToCustomComparison = new QuickFormComparison(
                    _baseFormReader.FormProperties, _customFormReader.FormProperties,
                    _baseFormReader.FormResources, _customFormReader.FormResources,
                    _baseQuickForm, _customQuickForm);
                _baseToCustomComparison.Compare();
            }

            if (CurrentProjectPathIsSet)
            {
                _currentQuickForm = LoadQuickForm(_currentWorkItem, CurrentProjectPath, QuickFormPath);
                _currentFormReader = new QuickFormReader(_currentQuickForm);
                _currentFormReader.ReadAllProperties();

                _baseToCurrentComparison = new QuickFormComparison(
                    _baseFormReader.FormProperties, _currentFormReader.FormProperties,
                    _baseFormReader.FormResources, _currentFormReader.FormResources,
                    _baseQuickForm, _currentQuickForm);
                _baseToCurrentComparison.Compare();
            }
            
            var projectTab = tabs.TabPages[0];
            tabs.TabPages.Clear();
            tabs.TabPages.Add(projectTab);
            if (CustomizedProjectPathIsSet)
                AddTab(new FormChangesControl(_baseToCustomComparison, _currentQuickForm), "Base To Custom Changes");
            if (CurrentProjectPathIsSet)
                AddTab(new FormChangesControl(_baseToCurrentComparison, _currentQuickForm), "Base To Current SLX Changes");
            AddTab(new FormDesignerHost(_baseWorkItem, _baseQuickForm), "Base SLX Form");
            if (CustomizedProjectPathIsSet)
                AddTab(new FormDesignerHost(_customWorkItem, _customQuickForm), "Custom Form");
            if (CurrentProjectPathIsSet)
                AddTab(new FormDesignerHost(_currentWorkItem, _currentQuickForm), "Current SLX Form");
            AddTab(new AllFormPropertiesControl(_baseFormReader), "All Base Form Properties");
            if (CustomizedProjectPathIsSet)
                AddTab(new AllFormPropertiesControl(_customFormReader), "All Custom Form Properties");
            if (CurrentProjectPathIsSet)
                AddTab(new AllFormPropertiesControl(_currentFormReader), "All Current SLX Form Properties");
        }

        private bool ValidateInput()
        {
            if (!BaseProjectPathIsSet)
            {
                MessageBox.Show("Base SLX project path must be set.");
                return false;
            }

            if (!CustomizedProjectPathIsSet && !CurrentProjectPathIsSet)
            {
                MessageBox.Show("The current or customized project path must be set.");
                return false;
            }

            if (string.IsNullOrEmpty(QuickFormPath))
            {
                MessageBox.Show("The QuickForm path must be set.");
                return false;
            }

            string baseQuickFormPath = Path.Combine(BaseProjectPath, QuickFormPath.Substring(1));
            if (!QFFileExists(baseQuickFormPath))
            {
                MessageBox.Show(baseQuickFormPath + " does not exist in base project.");
                return false;    
            }

            if (CustomizedProjectPathIsSet)
            {
                string customQuickFormPath = Path.Combine(CustomizedProjectPath, QuickFormPath.Substring(1));

                if (!QFFileExists(customQuickFormPath))
                {
                    MessageBox.Show(customQuickFormPath + " does not exist.");
                    return false;
                }
            }

            if (CurrentProjectPathIsSet)
            {
                string currentQuickFormPath = Path.Combine(CurrentProjectPath, QuickFormPath.Substring(1));

                if (!QFFileExists(currentQuickFormPath))
                {
                    MessageBox.Show(currentQuickFormPath + " does not exist.");
                    return false;
                }
            }

            return true;
        }

        private static bool QFFileExists(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            return new FileInfo(filePath).Exists;
        }

        private void AddTab(Control control, string tabText)
        {
            control.Dock = DockStyle.Fill;
            TabPage page = new TabPage(tabText);
            page.Dock = DockStyle.Fill;
            page.Controls.Add(control);
            tabs.TabPages.Add(page); 
        }
    }

    public static class ApplicationContextDefiler
    {
        /// <summary>
        /// This is a total hack to get around the fact that we have no real concept of a current WorkItem.
        /// ApplicationContext.Current always returns the root WorkItem,
        /// so we have to load it up with services from different workitems, whenever our WorkItem context changes.
        /// We should always allow CAB/Unity to inject our dependencies 
        /// and never use ApplicationContext.Current because it does not support multiple WorkItems!
        /// </summary>
        /// <param name="sourceWorkItem"></param>
        public static void SetRootWorkItemProjectLocalizationService(WorkItem sourceWorkItem)
        {
            if (ApplicationContext.Current.Services.Contains<ProjectLocalizationService>())
                ApplicationContext.Current.Services.Remove<ProjectLocalizationService>();

            if (sourceWorkItem != null)
            {
                var pls = sourceWorkItem.Services.Get<ProjectLocalizationService>();
                ApplicationContext.Current.Services.Add(pls);
            }
        }    
    }
}

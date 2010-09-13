using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sage.Platform.QuickForms.Controls;
using Sage.Platform.QuickForms;

namespace QuickFormDiff
{
    public partial class FormChangesControl : UserControl
    {
        private QuickFormComparison _formComparison;
        private IQuickFormDefinition _currentSlxForm;

        public FormChangesControl(QuickFormComparison formComparison, IQuickFormDefinition currentSlxForm)
        {
            InitializeComponent();
            _formComparison = formComparison;
            _currentSlxForm = currentSlxForm;
            btnCopyAll.Enabled = _currentSlxForm != null;
    
            InitializeRemovesTab();
            InitializeMovesTab();
            InitializeAddsTab();
            InitializeModsTab();
            InitializeResourcesTab();
        }

        private void InitializeResourcesTab()
        {
            grdRemovedResources.DataSource = _formComparison.ResourcesRemoved.ToList();
            grdAddedResources.DataSource = _formComparison.ResourcesAdded.ToList();
            grdModifiedResources.DataSource = _formComparison.ResourcesModified.ToList();
        }        

        private void InitializeModsTab()
        {
            StringBuilder sb = new StringBuilder();
            _formComparison.ModifiedProperties.ForEach(
                propDiff => sb.AppendLine(propDiff.PropertyName 
                                          + ": OldValue=" 
                                          + propDiff.OldValue 
                                          + "|NewValue=" 
                                          + propDiff.NewValue));
            txtMods.Text = sb.ToString();
        }

        private void InitializeMovesTab()
        {
            txtRowColumnChanges.Text = _formComparison.RowAndColumnChangeSummary;

            var sb = new StringBuilder();
            foreach (ControlPair movedControl in _formComparison.ControlsMoved)
            {
                sb.AppendLine(string.Format("{0}({1}) moved from Row={2}, Column={3} to Row={4}, Column={5}", 
                    movedControl.NewControl.ControlId, 
                    movedControl.NewControl.GetType().Name, 
                    movedControl.OldControl.Row, 
                    movedControl.OldControl.Column,
                    movedControl.NewControl.Row, 
                    movedControl.NewControl.Column
                    ));
            }
            txtControlMoves.Text = sb.ToString();
        }

        private void InitializeAddsTab()
        {
            StringBuilder sb = new StringBuilder();
            _formComparison.ControlsAdded.ForEach(control => sb.AppendLine(
                GenerateControlDescription(control, RenameFromSuggestion(control))));
            txtAddedControls.Text = sb.ToString();
           
            sb = new StringBuilder();
            _formComparison.AddedProperties
                .OrderBy(keyValue => keyValue.Key)
                .ForEach(keyValue => sb.AppendLine(keyValue.Key + ": " + keyValue.Value));
            txtAddedProperties.Text = sb.ToString();
        }

        private void InitializeRemovesTab()
        {
            var sb = new StringBuilder();
            _formComparison.ControlsRemoved.ForEach(control => sb.AppendLine(
                GenerateControlDescription(control, RenameToSuggestion(control))));
            txtRemovedControls.Text = sb.ToString();

            sb = new StringBuilder();
            _formComparison.RemovedProperties
                .OrderBy(keyValue => keyValue.Key)
                .ForEach(keyValue => sb.AppendLine(keyValue.Key + ": " + keyValue.Value));
            txtRemovedProperties.Text = sb.ToString();
        }

        private static string GenerateControlDescription(IQuickFormsControl control, string renameSuggestion)
        {
            return string.Format("{0}({1}) [Row={2}, Column={3}]{4}", control.ControlId, 
                control.GetType().Name, control.Row, control.Column, renameSuggestion);
        }

        private string RenameToSuggestion(IQuickFormsControl control)
        {
            string suggestion = string.Empty;

            ControlPair renamedControl = _formComparison.ControlsPossiblyRenamed
                .Where(rename => rename.OldControl.ControlId == control.ControlId).FirstOrDefault();

            if (renamedControl != null)
                suggestion = " (Possibly renamed to " + renamedControl.NewControl.ControlId + ")";

            return suggestion;
        }

        private string RenameFromSuggestion(IQuickFormsControl control)
        {
            string suggestion = string.Empty;

            ControlPair renamedControl = _formComparison.ControlsPossiblyRenamed
                .Where(rename => rename.NewControl.ControlId == control.ControlId).FirstOrDefault();

            if (renamedControl != null)
                suggestion = " (Possibly renamed from " + renamedControl.OldControl.ControlId + ")";

            return suggestion;
        }

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            string sourceResxPath = _currentSlxForm.FilePath.FullName + ".resx";
            var resxUpdater = new ResxUpdater(sourceResxPath,
                                              _formComparison.ResourcesAdded, 
                                              _formComparison.ResourcesRemoved,
                                              _formComparison.ResourcesModified);
            string newResxPath = sourceResxPath + ".new.resx";
            resxUpdater.UpdateResxWithChanges(newResxPath);
            MessageBox.Show(string.Format("The resource changes show here have been applied to {0} into a new resx saved at {1}",
                sourceResxPath, newResxPath));
        }
    }
}

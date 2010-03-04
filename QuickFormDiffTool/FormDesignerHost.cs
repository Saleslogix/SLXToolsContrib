using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sage.Platform.Application;
using Sage.Platform.IDEModule;
using Sage.Platform.QuickForms;
using SageApp = Sage.Platform.Application;

namespace QuickFormDiff
{
    public partial class FormDesignerHost : UserControl
    {
        private PropertyGridToolWindow _propertyGrid;

        public FormDesignerHost(WorkItem workItem, IQuickFormDefinition quickForm)
        {
            InitializeComponent();

            quickFormsDefinitionCompositeEditor1.SelectionService =
                workItem.Services.Get<ISelectionService>();
            quickFormsDefinitionCompositeEditor1.BrowsableService =
                workItem.Services.Get<IBrowsableObjectService>();
            quickFormsDefinitionCompositeEditor1.DataSource = quickForm;
            ApplicationContextDefiler.SetRootWorkItemProjectLocalizationService(workItem);

            _propertyGrid = workItem.BuildTransientItem(typeof(PropertyGridToolWindow)) as PropertyGridToolWindow;
            _propertyGrid.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(_propertyGrid);

            WorkItem = workItem;
        }

        public WorkItem WorkItem { get; private set; }
    }
}

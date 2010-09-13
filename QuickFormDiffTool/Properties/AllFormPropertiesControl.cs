using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sage.Platform.QuickForms;

namespace QuickFormDiff
{
    public partial class AllFormPropertiesControl : UserControl
    {
        public AllFormPropertiesControl(QuickFormReader formReader)
        {
            InitializeComponent();
            txtTotalRows.Text = formReader.QuickForm.Rows.Count.ToString();
            txtTotalColumns.Text = formReader.QuickForm.Columns.Count.ToString();

            var sb = new StringBuilder();

            formReader.FormProperties
                .OrderBy(keyValue => keyValue.Key)
                .ForEach(keyValue => sb.AppendLine(keyValue.Key + ": " + keyValue.Value));
            txtProperties.Text = sb.ToString();
        }
    }
}

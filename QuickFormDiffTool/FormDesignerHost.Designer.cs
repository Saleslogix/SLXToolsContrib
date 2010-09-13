namespace QuickFormDiff
{
    partial class FormDesignerHost
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.quickFormsDefinitionCompositeEditor1 = new Sage.Platform.QuickForms.Designer.QuickFormsDefinitionCompositeEditor();
            this.Label = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Label);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(684, 33);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 33);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.quickFormsDefinitionCompositeEditor1);
            this.splitContainer1.Size = new System.Drawing.Size(684, 448);
            this.splitContainer1.SplitterDistance = 447;
            this.splitContainer1.TabIndex = 3;
            // 
            // quickFormsDefinitionCompositeEditor1
            // 
            this.quickFormsDefinitionCompositeEditor1.BrowsableService = null;
            this.quickFormsDefinitionCompositeEditor1.DataSource = null;
            this.quickFormsDefinitionCompositeEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickFormsDefinitionCompositeEditor1.Location = new System.Drawing.Point(0, 0);
            this.quickFormsDefinitionCompositeEditor1.Name = "quickFormsDefinitionCompositeEditor1";
            this.quickFormsDefinitionCompositeEditor1.SelectionService = null;
            this.quickFormsDefinitionCompositeEditor1.Size = new System.Drawing.Size(447, 448);
            this.quickFormsDefinitionCompositeEditor1.TabIndex = 0;
            // 
            // Label
            // 
            this.Label.AutoSize = true;
            this.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label.Location = new System.Drawing.Point(12, 11);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(537, 13);
            this.Label.TabIndex = 0;
            this.Label.Text = "NOTE - This view is for informational purposes only.  Changes made here will not " +
                "be retained.";
            // 
            // FormDesignerHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "FormDesignerHost";
            this.Size = new System.Drawing.Size(684, 481);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Sage.Platform.QuickForms.Designer.QuickFormsDefinitionCompositeEditor quickFormsDefinitionCompositeEditor1;

    }
}

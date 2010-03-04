namespace QuickFormDiff
{
    partial class AllFormPropertiesControl
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTotalRows = new System.Windows.Forms.TextBox();
            this.txtTotalColumns = new System.Windows.Forms.TextBox();
            this.txtProperties = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtTotalColumns);
            this.panel1.Controls.Add(this.txtTotalRows);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(482, 44);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtProperties);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 44);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(4);
            this.panel2.Size = new System.Drawing.Size(482, 342);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Number of Rows:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(227, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Total Number of Columns:";
            // 
            // txtTotalRows
            // 
            this.txtTotalRows.Location = new System.Drawing.Point(138, 15);
            this.txtTotalRows.Name = "txtTotalRows";
            this.txtTotalRows.ReadOnly = true;
            this.txtTotalRows.Size = new System.Drawing.Size(51, 20);
            this.txtTotalRows.TabIndex = 2;
            // 
            // txtTotalColumns
            // 
            this.txtTotalColumns.Location = new System.Drawing.Point(362, 15);
            this.txtTotalColumns.Name = "txtTotalColumns";
            this.txtTotalColumns.ReadOnly = true;
            this.txtTotalColumns.Size = new System.Drawing.Size(51, 20);
            this.txtTotalColumns.TabIndex = 3;
            // 
            // txtProperties
            // 
            this.txtProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProperties.Location = new System.Drawing.Point(4, 4);
            this.txtProperties.Multiline = true;
            this.txtProperties.Name = "txtProperties";
            this.txtProperties.ReadOnly = true;
            this.txtProperties.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProperties.Size = new System.Drawing.Size(474, 334);
            this.txtProperties.TabIndex = 0;
            // 
            // AllFormPropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "AllFormPropertiesControl";
            this.Size = new System.Drawing.Size(482, 386);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtTotalColumns;
        private System.Windows.Forms.TextBox txtTotalRows;
        private System.Windows.Forms.TextBox txtProperties;
    }
}

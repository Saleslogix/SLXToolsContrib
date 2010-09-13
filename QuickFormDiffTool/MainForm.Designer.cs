namespace QuickFormDiff
{
    partial class frmQFDiff
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSetQuickFormPath = new System.Windows.Forms.Button();
            this.txtQuickFormPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.txtCurrentPath = new System.Windows.Forms.TextBox();
            this.txtCustomizedPath = new System.Windows.Forms.TextBox();
            this.txtBasePath = new System.Windows.Forms.TextBox();
            this.btnSetCurrentPath = new System.Windows.Forms.Button();
            this.btnSetCustomizedPath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSetBasePath = new System.Windows.Forms.Button();
            this.dlgBrowseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgSelectFile = new System.Windows.Forms.OpenFileDialog();
            this.tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabPage1);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(667, 405);
            this.tabs.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSetQuickFormPath);
            this.tabPage1.Controls.Add(this.txtQuickFormPath);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.btnAnalyze);
            this.tabPage1.Controls.Add(this.txtCurrentPath);
            this.tabPage1.Controls.Add(this.txtCustomizedPath);
            this.tabPage1.Controls.Add(this.txtBasePath);
            this.tabPage1.Controls.Add(this.btnSetCurrentPath);
            this.tabPage1.Controls.Add(this.btnSetCustomizedPath);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnSetBasePath);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(659, 379);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Select Projects";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSetQuickFormPath
            // 
            this.btnSetQuickFormPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetQuickFormPath.Enabled = false;
            this.btnSetQuickFormPath.Location = new System.Drawing.Point(554, 201);
            this.btnSetQuickFormPath.Name = "btnSetQuickFormPath";
            this.btnSetQuickFormPath.Size = new System.Drawing.Size(75, 23);
            this.btnSetQuickFormPath.TabIndex = 4;
            this.btnSetQuickFormPath.Text = "Set";
            this.btnSetQuickFormPath.UseVisualStyleBackColor = true;
            this.btnSetQuickFormPath.Click += new System.EventHandler(this.btnSetQuickFormPath_Click);
            // 
            // txtQuickFormPath
            // 
            this.txtQuickFormPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuickFormPath.Location = new System.Drawing.Point(26, 203);
            this.txtQuickFormPath.Name = "txtQuickFormPath";
            this.txtQuickFormPath.ReadOnly = true;
            this.txtQuickFormPath.Size = new System.Drawing.Size(522, 20);
            this.txtQuickFormPath.TabIndex = 12;
            this.txtQuickFormPath.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "QuickForm Path:";
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalyze.Location = new System.Drawing.Point(554, 266);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(75, 23);
            this.btnAnalyze.TabIndex = 5;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // txtCurrentPath
            // 
            this.txtCurrentPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentPath.Location = new System.Drawing.Point(26, 153);
            this.txtCurrentPath.Name = "txtCurrentPath";
            this.txtCurrentPath.ReadOnly = true;
            this.txtCurrentPath.Size = new System.Drawing.Size(522, 20);
            this.txtCurrentPath.TabIndex = 9;
            this.txtCurrentPath.TabStop = false;
            // 
            // txtCustomizedPath
            // 
            this.txtCustomizedPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomizedPath.Location = new System.Drawing.Point(26, 105);
            this.txtCustomizedPath.Name = "txtCustomizedPath";
            this.txtCustomizedPath.ReadOnly = true;
            this.txtCustomizedPath.Size = new System.Drawing.Size(522, 20);
            this.txtCustomizedPath.TabIndex = 8;
            this.txtCustomizedPath.TabStop = false;
            // 
            // txtBasePath
            // 
            this.txtBasePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBasePath.Location = new System.Drawing.Point(26, 51);
            this.txtBasePath.Name = "txtBasePath";
            this.txtBasePath.ReadOnly = true;
            this.txtBasePath.Size = new System.Drawing.Size(522, 20);
            this.txtBasePath.TabIndex = 7;
            this.txtBasePath.TabStop = false;
            // 
            // btnSetCurrentPath
            // 
            this.btnSetCurrentPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetCurrentPath.Location = new System.Drawing.Point(554, 150);
            this.btnSetCurrentPath.Name = "btnSetCurrentPath";
            this.btnSetCurrentPath.Size = new System.Drawing.Size(75, 23);
            this.btnSetCurrentPath.TabIndex = 3;
            this.btnSetCurrentPath.Text = "Set";
            this.btnSetCurrentPath.UseVisualStyleBackColor = true;
            this.btnSetCurrentPath.Click += new System.EventHandler(this.btnSetCurrentPath_Click);
            // 
            // btnSetCustomizedPath
            // 
            this.btnSetCustomizedPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetCustomizedPath.Location = new System.Drawing.Point(554, 102);
            this.btnSetCustomizedPath.Name = "btnSetCustomizedPath";
            this.btnSetCustomizedPath.Size = new System.Drawing.Size(75, 23);
            this.btnSetCustomizedPath.TabIndex = 2;
            this.btnSetCustomizedPath.Text = "Set";
            this.btnSetCustomizedPath.UseVisualStyleBackColor = true;
            this.btnSetCustomizedPath.Click += new System.EventHandler(this.btnSetCustomizedPath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Current SLX Project Path:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Customized Project Path:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Base SLX Project Path:";
            // 
            // btnSetBasePath
            // 
            this.btnSetBasePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetBasePath.Location = new System.Drawing.Point(554, 48);
            this.btnSetBasePath.Name = "btnSetBasePath";
            this.btnSetBasePath.Size = new System.Drawing.Size(75, 23);
            this.btnSetBasePath.TabIndex = 0;
            this.btnSetBasePath.Text = "Set";
            this.btnSetBasePath.UseVisualStyleBackColor = true;
            this.btnSetBasePath.Click += new System.EventHandler(this.btnSetBasePath_Click);
            // 
            // dlgSelectFile
            // 
            this.dlgSelectFile.FileName = "openFileDialog1";
            this.dlgSelectFile.Filter = "QuickForm Files|*.quickform.xml";
            // 
            // frmQFDiff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 405);
            this.Controls.Add(this.tabs);
            this.Name = "frmQFDiff";
            this.Text = "Compare a QuickForm";
            this.tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnSetBasePath;
        private System.Windows.Forms.TextBox txtCurrentPath;
        private System.Windows.Forms.TextBox txtCustomizedPath;
        private System.Windows.Forms.TextBox txtBasePath;
        private System.Windows.Forms.Button btnSetCurrentPath;
        private System.Windows.Forms.Button btnSetCustomizedPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog dlgBrowseFolder;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnSetQuickFormPath;
        private System.Windows.Forms.TextBox txtQuickFormPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog dlgSelectFile;

    }
}


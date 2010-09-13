namespace QuickFormDiff
{
    partial class FormChangesControl
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabRemoves = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtRemovedControls = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRemovedProperties = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.tabMoves = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.txtRowColumnChanges = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtControlMoves = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.tabAdds = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.txtAddedControls = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAddedProperties = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.tabMods = new System.Windows.Forms.TabPage();
            this.txtMods = new System.Windows.Forms.TextBox();
            this.tabResources = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.grdRemovedResources = new System.Windows.Forms.DataGridView();
            this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnCopyAll = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.grdAddedResources = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.grdModifiedResources = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.mnuManageResources = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabRemoves.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabMoves.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabAdds.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabMods.SuspendLayout();
            this.tabResources.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRemovedResources)).BeginInit();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAddedResources)).BeginInit();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdModifiedResources)).BeginInit();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabRemoves);
            this.tabControl1.Controls.Add(this.tabMoves);
            this.tabControl1.Controls.Add(this.tabAdds);
            this.tabControl1.Controls.Add(this.tabMods);
            this.tabControl1.Controls.Add(this.tabResources);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(594, 511);
            this.tabControl1.TabIndex = 0;
            // 
            // tabRemoves
            // 
            this.tabRemoves.Controls.Add(this.splitContainer1);
            this.tabRemoves.Location = new System.Drawing.Point(4, 4);
            this.tabRemoves.Name = "tabRemoves";
            this.tabRemoves.Padding = new System.Windows.Forms.Padding(3);
            this.tabRemoves.Size = new System.Drawing.Size(586, 485);
            this.tabRemoves.TabIndex = 0;
            this.tabRemoves.Text = "Removes";
            this.tabRemoves.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtRemovedControls);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtRemovedProperties);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(580, 479);
            this.splitContainer1.SplitterDistance = 193;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtRemovedControls
            // 
            this.txtRemovedControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemovedControls.Location = new System.Drawing.Point(0, 30);
            this.txtRemovedControls.Multiline = true;
            this.txtRemovedControls.Name = "txtRemovedControls";
            this.txtRemovedControls.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemovedControls.Size = new System.Drawing.Size(580, 163);
            this.txtRemovedControls.TabIndex = 1;
            this.txtRemovedControls.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(580, 30);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Removed Controls:";
            // 
            // txtRemovedProperties
            // 
            this.txtRemovedProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemovedProperties.Location = new System.Drawing.Point(0, 40);
            this.txtRemovedProperties.Multiline = true;
            this.txtRemovedProperties.Name = "txtRemovedProperties";
            this.txtRemovedProperties.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemovedProperties.Size = new System.Drawing.Size(580, 242);
            this.txtRemovedProperties.TabIndex = 1;
            this.txtRemovedProperties.WordWrap = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(580, 40);
            this.panel2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Removed Properties:";
            // 
            // tabMoves
            // 
            this.tabMoves.Controls.Add(this.splitContainer3);
            this.tabMoves.Location = new System.Drawing.Point(4, 4);
            this.tabMoves.Name = "tabMoves";
            this.tabMoves.Size = new System.Drawing.Size(586, 485);
            this.tabMoves.TabIndex = 2;
            this.tabMoves.Text = "Moves";
            this.tabMoves.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.txtRowColumnChanges);
            this.splitContainer3.Panel1.Controls.Add(this.panel5);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.txtControlMoves);
            this.splitContainer3.Panel2.Controls.Add(this.panel6);
            this.splitContainer3.Size = new System.Drawing.Size(586, 485);
            this.splitContainer3.SplitterDistance = 195;
            this.splitContainer3.TabIndex = 1;
            // 
            // txtRowColumnChanges
            // 
            this.txtRowColumnChanges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRowColumnChanges.Location = new System.Drawing.Point(0, 30);
            this.txtRowColumnChanges.Multiline = true;
            this.txtRowColumnChanges.Name = "txtRowColumnChanges";
            this.txtRowColumnChanges.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRowColumnChanges.Size = new System.Drawing.Size(586, 165);
            this.txtRowColumnChanges.TabIndex = 1;
            this.txtRowColumnChanges.WordWrap = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label5);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(586, 30);
            this.panel5.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Total Row and Column Changes:";
            // 
            // txtControlMoves
            // 
            this.txtControlMoves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtControlMoves.Location = new System.Drawing.Point(0, 40);
            this.txtControlMoves.Multiline = true;
            this.txtControlMoves.Name = "txtControlMoves";
            this.txtControlMoves.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtControlMoves.Size = new System.Drawing.Size(586, 246);
            this.txtControlMoves.TabIndex = 1;
            this.txtControlMoves.WordWrap = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label6);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(586, 40);
            this.panel6.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Control Movement:";
            // 
            // tabAdds
            // 
            this.tabAdds.Controls.Add(this.splitContainer2);
            this.tabAdds.Location = new System.Drawing.Point(4, 4);
            this.tabAdds.Name = "tabAdds";
            this.tabAdds.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdds.Size = new System.Drawing.Size(586, 485);
            this.tabAdds.TabIndex = 1;
            this.tabAdds.Text = "Adds";
            this.tabAdds.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtAddedControls);
            this.splitContainer2.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtAddedProperties);
            this.splitContainer2.Panel2.Controls.Add(this.panel4);
            this.splitContainer2.Size = new System.Drawing.Size(580, 479);
            this.splitContainer2.SplitterDistance = 193;
            this.splitContainer2.TabIndex = 1;
            // 
            // txtAddedControls
            // 
            this.txtAddedControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAddedControls.Location = new System.Drawing.Point(0, 30);
            this.txtAddedControls.Multiline = true;
            this.txtAddedControls.Name = "txtAddedControls";
            this.txtAddedControls.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAddedControls.Size = new System.Drawing.Size(580, 163);
            this.txtAddedControls.TabIndex = 1;
            this.txtAddedControls.WordWrap = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(580, 30);
            this.panel3.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Added Controls:";
            // 
            // txtAddedProperties
            // 
            this.txtAddedProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAddedProperties.Location = new System.Drawing.Point(0, 40);
            this.txtAddedProperties.Multiline = true;
            this.txtAddedProperties.Name = "txtAddedProperties";
            this.txtAddedProperties.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAddedProperties.Size = new System.Drawing.Size(580, 242);
            this.txtAddedProperties.TabIndex = 1;
            this.txtAddedProperties.WordWrap = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(580, 40);
            this.panel4.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Added Properties:";
            // 
            // tabMods
            // 
            this.tabMods.Controls.Add(this.txtMods);
            this.tabMods.Location = new System.Drawing.Point(4, 4);
            this.tabMods.Name = "tabMods";
            this.tabMods.Size = new System.Drawing.Size(586, 485);
            this.tabMods.TabIndex = 3;
            this.tabMods.Text = "Modifications";
            this.tabMods.UseVisualStyleBackColor = true;
            // 
            // txtMods
            // 
            this.txtMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMods.Location = new System.Drawing.Point(0, 0);
            this.txtMods.Multiline = true;
            this.txtMods.Name = "txtMods";
            this.txtMods.Size = new System.Drawing.Size(586, 485);
            this.txtMods.TabIndex = 0;
            this.txtMods.WordWrap = false;
            // 
            // tabResources
            // 
            this.tabResources.Controls.Add(this.splitContainer4);
            this.tabResources.Location = new System.Drawing.Point(4, 4);
            this.tabResources.Name = "tabResources";
            this.tabResources.Padding = new System.Windows.Forms.Padding(3);
            this.tabResources.Size = new System.Drawing.Size(586, 485);
            this.tabResources.TabIndex = 4;
            this.tabResources.Text = "Resources";
            this.tabResources.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.grdModifiedResources);
            this.splitContainer4.Panel2.Controls.Add(this.panel9);
            this.splitContainer4.Size = new System.Drawing.Size(580, 479);
            this.splitContainer4.SplitterDistance = 322;
            this.splitContainer4.TabIndex = 1;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.grdRemovedResources);
            this.splitContainer5.Panel1.Controls.Add(this.panel7);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.grdAddedResources);
            this.splitContainer5.Panel2.Controls.Add(this.panel8);
            this.splitContainer5.Size = new System.Drawing.Size(580, 322);
            this.splitContainer5.SplitterDistance = 164;
            this.splitContainer5.TabIndex = 0;
            // 
            // grdRemovedResources
            // 
            this.grdRemovedResources.AllowUserToAddRows = false;
            this.grdRemovedResources.AllowUserToDeleteRows = false;
            this.grdRemovedResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRemovedResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colKey,
            this.colValue});
            this.grdRemovedResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdRemovedResources.Location = new System.Drawing.Point(0, 30);
            this.grdRemovedResources.Name = "grdRemovedResources";
            this.grdRemovedResources.ReadOnly = true;
            this.grdRemovedResources.RowHeadersVisible = false;
            this.grdRemovedResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdRemovedResources.Size = new System.Drawing.Size(580, 134);
            this.grdRemovedResources.TabIndex = 3;
            // 
            // colKey
            // 
            this.colKey.DataPropertyName = "Key";
            this.colKey.HeaderText = "Resource Name";
            this.colKey.Name = "colKey";
            this.colKey.ReadOnly = true;
            this.colKey.Width = 200;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.DataPropertyName = "Value";
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnCopyAll);
            this.panel7.Controls.Add(this.label7);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(580, 30);
            this.panel7.TabIndex = 2;
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Location = new System.Drawing.Point(272, 1);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(263, 23);
            this.btnCopyAll.TabIndex = 1;
            this.btnCopyAll.Text = "Merge Changes into Copy of Current SLX Version";
            this.btnCopyAll.UseVisualStyleBackColor = true;
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Removed Resources:";
            // 
            // grdAddedResources
            // 
            this.grdAddedResources.AllowUserToAddRows = false;
            this.grdAddedResources.AllowUserToDeleteRows = false;
            this.grdAddedResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAddedResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.grdAddedResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAddedResources.Location = new System.Drawing.Point(0, 30);
            this.grdAddedResources.Name = "grdAddedResources";
            this.grdAddedResources.ReadOnly = true;
            this.grdAddedResources.RowHeadersVisible = false;
            this.grdAddedResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdAddedResources.Size = new System.Drawing.Size(580, 124);
            this.grdAddedResources.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Key";
            this.dataGridViewTextBoxColumn1.HeaderText = "Resource Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Value";
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label8);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(580, 30);
            this.panel8.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Added Resources:";
            // 
            // grdModifiedResources
            // 
            this.grdModifiedResources.AllowUserToAddRows = false;
            this.grdModifiedResources.AllowUserToDeleteRows = false;
            this.grdModifiedResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdModifiedResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.colOldValue,
            this.colNewValue});
            this.grdModifiedResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdModifiedResources.Location = new System.Drawing.Point(0, 30);
            this.grdModifiedResources.Name = "grdModifiedResources";
            this.grdModifiedResources.ReadOnly = true;
            this.grdModifiedResources.RowHeadersVisible = false;
            this.grdModifiedResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdModifiedResources.Size = new System.Drawing.Size(580, 123);
            this.grdModifiedResources.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ResourceName";
            this.dataGridViewTextBoxColumn3.HeaderText = "Resource Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 200;
            // 
            // colOldValue
            // 
            this.colOldValue.DataPropertyName = "OldValue";
            this.colOldValue.HeaderText = "Old Value";
            this.colOldValue.Name = "colOldValue";
            this.colOldValue.ReadOnly = true;
            this.colOldValue.Width = 200;
            // 
            // colNewValue
            // 
            this.colNewValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNewValue.DataPropertyName = "NewValue";
            this.colNewValue.HeaderText = "New Value";
            this.colNewValue.Name = "colNewValue";
            this.colNewValue.ReadOnly = true;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.label9);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(580, 30);
            this.panel9.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Modified Resources:";
            // 
            // mnuManageResources
            // 
            this.mnuManageResources.Name = "mnuManageResources";
            this.mnuManageResources.Size = new System.Drawing.Size(61, 4);
            // 
            // FormChangesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "FormChangesControl";
            this.Size = new System.Drawing.Size(594, 511);
            this.tabControl1.ResumeLayout(false);
            this.tabRemoves.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabMoves.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            this.splitContainer3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.tabAdds.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabMods.ResumeLayout(false);
            this.tabMods.PerformLayout();
            this.tabResources.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRemovedResources)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAddedResources)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdModifiedResources)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabRemoves;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabAdds;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtRemovedControls;
        private System.Windows.Forms.TextBox txtRemovedProperties;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txtAddedControls;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAddedProperties;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabMoves;
        private System.Windows.Forms.TabPage tabMods;
        private System.Windows.Forms.TextBox txtMods;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox txtRowColumnChanges;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtControlMoves;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabResources;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView grdRemovedResources;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridView grdAddedResources;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridView grdModifiedResources;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewValue;
        private System.Windows.Forms.ContextMenuStrip mnuManageResources;
        private System.Windows.Forms.Button btnCopyAll;
    }
}

namespace SpreadsheetGUI
{
    partial class SpreadsheetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpreadsheetForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutSpreadsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upgradeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.professionalVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.cellNameLabel = new System.Windows.Forms.TextBox();
            this.editorNameTextBox = new System.Windows.Forms.TextBox();
            this.editorContentTextBox = new System.Windows.Forms.TextBox();
            this.editorValueTextBox = new System.Windows.Forms.TextBox();
            this.inputLabel = new System.Windows.Forms.TextBox();
            this.cellValueLabel = new System.Windows.Forms.TextBox();
            this.spreadsheetPanel = new SS.SpreadsheetPanel();
            this.menuStrip.SuspendLayout();
            this.topTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.topTableLayoutPanel.SetColumnSpan(this.menuStrip, 3);
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.upgradeToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(2157, 40);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(64, 36);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            this.newToolStripMenuItem.Text = "Disconnect";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.DisconnectToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutSpreadsheetToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(77, 36);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutSpreadsheetToolStripMenuItem
            // 
            this.aboutSpreadsheetToolStripMenuItem.Name = "aboutSpreadsheetToolStripMenuItem";
            this.aboutSpreadsheetToolStripMenuItem.Size = new System.Drawing.Size(318, 38);
            this.aboutSpreadsheetToolStripMenuItem.Text = "About Spreadsheet";
            this.aboutSpreadsheetToolStripMenuItem.Click += new System.EventHandler(this.AboutSpreadsheetToolStripMenuItem_Click);
            // 
            // upgradeToolStripMenuItem
            // 
            this.upgradeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.professionalVersionToolStripMenuItem});
            this.upgradeToolStripMenuItem.Name = "upgradeToolStripMenuItem";
            this.upgradeToolStripMenuItem.Size = new System.Drawing.Size(118, 36);
            this.upgradeToolStripMenuItem.Text = "Upgrade";
            // 
            // professionalVersionToolStripMenuItem
            // 
            this.professionalVersionToolStripMenuItem.Name = "professionalVersionToolStripMenuItem";
            this.professionalVersionToolStripMenuItem.Size = new System.Drawing.Size(327, 38);
            this.professionalVersionToolStripMenuItem.Text = "Professional Version";
            this.professionalVersionToolStripMenuItem.Click += new System.EventHandler(this.professionalVersionToolStripMenuItem_Click);
            // 
            // topTableLayoutPanel
            // 
            this.topTableLayoutPanel.AutoSize = true;
            this.topTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.topTableLayoutPanel.ColumnCount = 3;
            this.topTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.topTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.topTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.topTableLayoutPanel.Controls.Add(this.cellNameLabel, 0, 1);
            this.topTableLayoutPanel.Controls.Add(this.menuStrip, 0, 0);
            this.topTableLayoutPanel.Controls.Add(this.editorNameTextBox, 0, 2);
            this.topTableLayoutPanel.Controls.Add(this.editorContentTextBox, 1, 2);
            this.topTableLayoutPanel.Controls.Add(this.editorValueTextBox, 2, 2);
            this.topTableLayoutPanel.Controls.Add(this.inputLabel, 1, 1);
            this.topTableLayoutPanel.Controls.Add(this.cellValueLabel, 2, 1);
            this.topTableLayoutPanel.Controls.Add(this.spreadsheetPanel, 0, 3);
            this.topTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topTableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.topTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.topTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.topTableLayoutPanel.Name = "topTableLayoutPanel";
            this.topTableLayoutPanel.RowCount = 4;
            this.topTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.topTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.topTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.topTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.topTableLayoutPanel.Size = new System.Drawing.Size(2157, 1115);
            this.topTableLayoutPanel.TabIndex = 3;
            // 
            // cellNameLabel
            // 
            this.cellNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cellNameLabel.Location = new System.Drawing.Point(3, 43);
            this.cellNameLabel.Name = "cellNameLabel";
            this.cellNameLabel.ReadOnly = true;
            this.cellNameLabel.Size = new System.Drawing.Size(533, 31);
            this.cellNameLabel.TabIndex = 5;
            this.cellNameLabel.Text = "Cell Name:";
            // 
            // editorNameTextBox
            // 
            this.editorNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorNameTextBox.Location = new System.Drawing.Point(3, 83);
            this.editorNameTextBox.Name = "editorNameTextBox";
            this.editorNameTextBox.ReadOnly = true;
            this.editorNameTextBox.Size = new System.Drawing.Size(533, 31);
            this.editorNameTextBox.TabIndex = 2;
            // 
            // editorContentTextBox
            // 
            this.editorContentTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorContentTextBox.Location = new System.Drawing.Point(542, 83);
            this.editorContentTextBox.Name = "editorContentTextBox";
            this.editorContentTextBox.Size = new System.Drawing.Size(1072, 31);
            this.editorContentTextBox.TabIndex = 3;
            this.editorContentTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EditorContentTextBox_KeyUp);
            // 
            // editorValueTextBox
            // 
            this.editorValueTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorValueTextBox.Location = new System.Drawing.Point(1620, 83);
            this.editorValueTextBox.Name = "editorValueTextBox";
            this.editorValueTextBox.ReadOnly = true;
            this.editorValueTextBox.Size = new System.Drawing.Size(534, 31);
            this.editorValueTextBox.TabIndex = 4;
            // 
            // inputLabel
            // 
            this.inputLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputLabel.Location = new System.Drawing.Point(542, 43);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.ReadOnly = true;
            this.inputLabel.Size = new System.Drawing.Size(1072, 31);
            this.inputLabel.TabIndex = 6;
            this.inputLabel.Text = "Input:";
            // 
            // cellValueLabel
            // 
            this.cellValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cellValueLabel.Location = new System.Drawing.Point(1620, 43);
            this.cellValueLabel.Name = "cellValueLabel";
            this.cellValueLabel.ReadOnly = true;
            this.cellValueLabel.Size = new System.Drawing.Size(534, 31);
            this.cellValueLabel.TabIndex = 7;
            this.cellValueLabel.Text = "Cell Value:";
            // 
            // spreadsheetPanel
            // 
            this.topTableLayoutPanel.SetColumnSpan(this.spreadsheetPanel, 3);
            this.spreadsheetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel.Location = new System.Drawing.Point(0, 117);
            this.spreadsheetPanel.Margin = new System.Windows.Forms.Padding(0);
            this.spreadsheetPanel.Name = "spreadsheetPanel";
            this.spreadsheetPanel.Size = new System.Drawing.Size(2157, 998);
            this.spreadsheetPanel.TabIndex = 0;
            this.spreadsheetPanel.SelectionChanged += new SS.SelectionChangedHandler(this.SpreadsheetPanel_SelectionChanged);
            // 
            // SpreadsheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2157, 1115);
            this.Controls.Add(this.topTableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "SpreadsheetForm";
            this.Text = "Untitled - Spreadsheet 3500";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadsheetForm_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.topTableLayoutPanel.ResumeLayout(false);
            this.topTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutSpreadsheetToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel topTableLayoutPanel;
        private System.Windows.Forms.TextBox editorValueTextBox;
        private System.Windows.Forms.TextBox editorContentTextBox;
        private System.Windows.Forms.TextBox editorNameTextBox;
        private System.Windows.Forms.TextBox cellNameLabel;
        private System.Windows.Forms.TextBox inputLabel;
        private System.Windows.Forms.TextBox cellValueLabel;
        private System.Windows.Forms.ToolStripMenuItem upgradeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem professionalVersionToolStripMenuItem;

        //// Text box within a cell being edited
        //private System.Windows.Forms.TextBox cellInputTextBox;
        //// 
        //// SpreadsheetForm
        //// 
    }
}


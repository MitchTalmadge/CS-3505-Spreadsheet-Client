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
            this.spreadsheetPanel = new SS.SpreadsheetPanel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutSpreadsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.cellNameTextBox = new System.Windows.Forms.TextBox();
            this.contentTextBox = new System.Windows.Forms.TextBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip.SuspendLayout();
            this.topTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel
            // 
            this.topTableLayoutPanel.SetColumnSpan(this.spreadsheetPanel, 3);
            this.spreadsheetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel.Location = new System.Drawing.Point(0, 80);
            this.spreadsheetPanel.Margin = new System.Windows.Forms.Padding(0);
            this.spreadsheetPanel.Name = "spreadsheetPanel";
            this.spreadsheetPanel.Size = new System.Drawing.Size(2157, 1035);
            this.spreadsheetPanel.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.topTableLayoutPanel.SetColumnSpan(this.menuStrip, 3);
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
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
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(64, 36);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.newToolStripMenuItem.Text = "New";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.openToolStripMenuItem.Text = "Open...";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.closeToolStripMenuItem.Text = "Close";
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
            // 
            // topTableLayoutPanel
            // 
            this.topTableLayoutPanel.AutoSize = true;
            this.topTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.topTableLayoutPanel.ColumnCount = 3;
            this.topTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.topTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.topTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.topTableLayoutPanel.Controls.Add(this.menuStrip, 0, 0);
            this.topTableLayoutPanel.Controls.Add(this.spreadsheetPanel, 0, 2);
            this.topTableLayoutPanel.Controls.Add(this.cellNameTextBox, 0, 1);
            this.topTableLayoutPanel.Controls.Add(this.contentTextBox, 1, 1);
            this.topTableLayoutPanel.Controls.Add(this.valueTextBox, 2, 1);
            this.topTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topTableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.topTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.topTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.topTableLayoutPanel.Name = "topTableLayoutPanel";
            this.topTableLayoutPanel.RowCount = 3;
            this.topTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.topTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.topTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.topTableLayoutPanel.Size = new System.Drawing.Size(2157, 1115);
            this.topTableLayoutPanel.TabIndex = 3;
            // 
            // cellNameTextBox
            // 
            this.cellNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cellNameTextBox.Location = new System.Drawing.Point(3, 43);
            this.cellNameTextBox.Name = "cellNameTextBox";
            this.cellNameTextBox.Size = new System.Drawing.Size(533, 31);
            this.cellNameTextBox.TabIndex = 2;
            // 
            // contentTextBox
            // 
            this.contentTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentTextBox.Location = new System.Drawing.Point(542, 43);
            this.contentTextBox.Name = "contentTextBox";
            this.contentTextBox.Size = new System.Drawing.Size(1072, 31);
            this.contentTextBox.TabIndex = 3;
            // 
            // valueTextBox
            // 
            this.valueTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valueTextBox.Location = new System.Drawing.Point(1620, 43);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(534, 31);
            this.valueTextBox.TabIndex = 4;
            // 
            // SpreadsheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2157, 1115);
            this.Controls.Add(this.topTableLayoutPanel);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "SpreadsheetForm";
            this.ShowIcon = false;
            this.Text = "Spreadsheet - Jiahui Chen & Mitch Talmadge";
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
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutSpreadsheetToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel topTableLayoutPanel;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.TextBox contentTextBox;
        private System.Windows.Forms.TextBox cellNameTextBox;
    }
}


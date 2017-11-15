namespace SpaceWars
{
    partial class MainMenuForm
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
            this._mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._inputLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._nicknameLabel = new System.Windows.Forms.Label();
            this._nicknameTextBox = new System.Windows.Forms.TextBox();
            this._hostNameTextBox = new System.Windows.Forms.TextBox();
            this._hostNameLabel = new System.Windows.Forms.Label();
            this._connectButton = new System.Windows.Forms.Button();
            this._logoLabel = new System.Windows.Forms.Label();
            this._copyrightLabel = new System.Windows.Forms.Label();
            this._mainLayoutPanel.SuspendLayout();
            this._inputLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mainLayoutPanel
            // 
            this._mainLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this._mainLayoutPanel.ColumnCount = 1;
            this._mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._mainLayoutPanel.Controls.Add(this._inputLayoutPanel, 0, 1);
            this._mainLayoutPanel.Controls.Add(this._logoLabel, 0, 0);
            this._mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._mainLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this._mainLayoutPanel.Name = "_mainLayoutPanel";
            this._mainLayoutPanel.RowCount = 2;
            this._mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this._mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this._mainLayoutPanel.Size = new System.Drawing.Size(1668, 1054);
            this._mainLayoutPanel.TabIndex = 0;
            // 
            // _inputLayoutPanel
            // 
            this._inputLayoutPanel.ColumnCount = 2;
            this._inputLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._inputLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._inputLayoutPanel.Controls.Add(this._nicknameLabel, 1, 0);
            this._inputLayoutPanel.Controls.Add(this._nicknameTextBox, 0, 1);
            this._inputLayoutPanel.Controls.Add(this._hostNameTextBox, 0, 1);
            this._inputLayoutPanel.Controls.Add(this._hostNameLabel, 0, 0);
            this._inputLayoutPanel.Controls.Add(this._connectButton, 0, 2);
            this._inputLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._inputLayoutPanel.Location = new System.Drawing.Point(4, 636);
            this._inputLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this._inputLayoutPanel.Name = "_inputLayoutPanel";
            this._inputLayoutPanel.RowCount = 3;
            this._inputLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._inputLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._inputLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._inputLayoutPanel.Size = new System.Drawing.Size(1660, 414);
            this._inputLayoutPanel.TabIndex = 0;
            // 
            // _nicknameLabel
            // 
            this._nicknameLabel.AutoSize = true;
            this._nicknameLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._nicknameLabel.Font = new System.Drawing.Font("OCR A Extended", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._nicknameLabel.ForeColor = System.Drawing.Color.White;
            this._nicknameLabel.Location = new System.Drawing.Point(850, 19);
            this._nicknameLabel.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this._nicknameLabel.Name = "_nicknameLabel";
            this._nicknameLabel.Size = new System.Drawing.Size(790, 65);
            this._nicknameLabel.TabIndex = 3;
            this._nicknameLabel.Text = "Nickname";
            this._nicknameLabel.UseCompatibleTextRendering = true;
            // 
            // _nicknameTextBox
            // 
            this._nicknameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._nicknameTextBox.Font = new System.Drawing.Font("OCR A Extended", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._nicknameTextBox.Location = new System.Drawing.Point(850, 122);
            this._nicknameTextBox.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this._nicknameTextBox.Name = "_nicknameTextBox";
            this._nicknameTextBox.Size = new System.Drawing.Size(790, 63);
            this._nicknameTextBox.TabIndex = 1;
            // 
            // _hostNameTextBox
            // 
            this._hostNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hostNameTextBox.Font = new System.Drawing.Font("OCR A Extended", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._hostNameTextBox.Location = new System.Drawing.Point(20, 122);
            this._hostNameTextBox.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this._hostNameTextBox.Name = "_hostNameTextBox";
            this._hostNameTextBox.Size = new System.Drawing.Size(790, 63);
            this._hostNameTextBox.TabIndex = 0;
            // 
            // _hostNameLabel
            // 
            this._hostNameLabel.AutoSize = true;
            this._hostNameLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._hostNameLabel.Font = new System.Drawing.Font("OCR A Extended", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._hostNameLabel.ForeColor = System.Drawing.Color.White;
            this._hostNameLabel.Location = new System.Drawing.Point(20, 19);
            this._hostNameLabel.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this._hostNameLabel.Name = "_hostNameLabel";
            this._hostNameLabel.Size = new System.Drawing.Size(790, 65);
            this._hostNameLabel.TabIndex = 2;
            this._hostNameLabel.Text = "Server Address";
            this._hostNameLabel.UseCompatibleTextRendering = true;
            // 
            // _connectButton
            // 
            this._connectButton.AutoSize = true;
            this._connectButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._connectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._inputLayoutPanel.SetColumnSpan(this._connectButton, 2);
            this._connectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._connectButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._connectButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._connectButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._connectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._connectButton.Font = new System.Drawing.Font("OCR A Extended", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._connectButton.ForeColor = System.Drawing.Color.White;
            this._connectButton.Location = new System.Drawing.Point(20, 225);
            this._connectButton.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this._connectButton.Name = "_connectButton";
            this._connectButton.Size = new System.Drawing.Size(1620, 170);
            this._connectButton.TabIndex = 2;
            this._connectButton.Text = "Connect";
            this._connectButton.UseCompatibleTextRendering = true;
            this._connectButton.UseVisualStyleBackColor = false;
            this._connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // _logoLabel
            // 
            this._logoLabel.AutoSize = true;
            this._logoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logoLabel.Font = new System.Drawing.Font("OCR A Extended", 80.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._logoLabel.ForeColor = System.Drawing.Color.White;
            this._logoLabel.Location = new System.Drawing.Point(4, 0);
            this._logoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._logoLabel.Name = "_logoLabel";
            this._logoLabel.Size = new System.Drawing.Size(1660, 632);
            this._logoLabel.TabIndex = 1;
            this._logoLabel.Text = "Space Wars";
            this._logoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._logoLabel.UseCompatibleTextRendering = true;
            // 
            // _copyrightLabel
            // 
            this._copyrightLabel.AutoSize = true;
            this._copyrightLabel.BackColor = System.Drawing.Color.Transparent;
            this._copyrightLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._copyrightLabel.Font = new System.Drawing.Font("OCR A Extended", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._copyrightLabel.ForeColor = System.Drawing.Color.White;
            this._copyrightLabel.Location = new System.Drawing.Point(0, 0);
            this._copyrightLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._copyrightLabel.Name = "_copyrightLabel";
            this._copyrightLabel.Size = new System.Drawing.Size(1060, 59);
            this._copyrightLabel.TabIndex = 2;
            this._copyrightLabel.Text = "(C) Jiahui Chen and Mitch Talmadge";
            this._copyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._copyrightLabel.UseCompatibleTextRendering = true;
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SpaceWars.Properties.Resources.space_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1668, 1054);
            this.Controls.Add(this._copyrightLabel);
            this.Controls.Add(this._mainLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainMenuForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Space Wars";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainMenuForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainMenuForm_KeyDown);
            this._mainLayoutPanel.ResumeLayout(false);
            this._mainLayoutPanel.PerformLayout();
            this._inputLayoutPanel.ResumeLayout(false);
            this._inputLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _mainLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel _inputLayoutPanel;
        private System.Windows.Forms.Label _nicknameLabel;
        private System.Windows.Forms.TextBox _nicknameTextBox;
        private System.Windows.Forms.TextBox _hostNameTextBox;
        private System.Windows.Forms.Label _hostNameLabel;
        private System.Windows.Forms.Button _connectButton;
        private System.Windows.Forms.Label _logoLabel;
        private System.Windows.Forms.Label _copyrightLabel;
    }
}


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
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.inputLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.NicknameLabel = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.ServerTextBox = new System.Windows.Forms.TextBox();
            this.ServerAddressLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.logoLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.mainLayoutPanel.SuspendLayout();
            this.inputLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Controls.Add(this.inputLayoutPanel, 0, 1);
            this.mainLayoutPanel.Controls.Add(this.logoLabel, 0, 0);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(1668, 1079);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // inputLayoutPanel
            // 
            this.inputLayoutPanel.ColumnCount = 2;
            this.inputLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.inputLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.inputLayoutPanel.Controls.Add(this.NicknameLabel, 1, 0);
            this.inputLayoutPanel.Controls.Add(this.NameTextBox, 0, 1);
            this.inputLayoutPanel.Controls.Add(this.ServerTextBox, 0, 1);
            this.inputLayoutPanel.Controls.Add(this.ServerAddressLabel, 0, 0);
            this.inputLayoutPanel.Controls.Add(this.ConnectButton, 0, 2);
            this.inputLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputLayoutPanel.Location = new System.Drawing.Point(4, 651);
            this.inputLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.inputLayoutPanel.Name = "inputLayoutPanel";
            this.inputLayoutPanel.RowCount = 3;
            this.inputLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.inputLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.inputLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.inputLayoutPanel.Size = new System.Drawing.Size(1660, 424);
            this.inputLayoutPanel.TabIndex = 0;
            // 
            // NicknameLabel
            // 
            this.NicknameLabel.AutoSize = true;
            this.NicknameLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.NicknameLabel.Font = new System.Drawing.Font("OCR A Extended", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NicknameLabel.ForeColor = System.Drawing.Color.White;
            this.NicknameLabel.Location = new System.Drawing.Point(850, 19);
            this.NicknameLabel.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this.NicknameLabel.Name = "NicknameLabel";
            this.NicknameLabel.Size = new System.Drawing.Size(790, 68);
            this.NicknameLabel.TabIndex = 3;
            this.NicknameLabel.Text = "Nickname";
            this.NicknameLabel.UseCompatibleTextRendering = true;
            // 
            // NameTextBox
            // 
            this.NameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameTextBox.Font = new System.Drawing.Font("OCR A Extended", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameTextBox.Location = new System.Drawing.Point(850, 125);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(790, 63);
            this.NameTextBox.TabIndex = 1;
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerTextBox.Font = new System.Drawing.Font("OCR A Extended", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerTextBox.Location = new System.Drawing.Point(20, 125);
            this.ServerTextBox.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.Size = new System.Drawing.Size(790, 63);
            this.ServerTextBox.TabIndex = 0;
            // 
            // ServerAddressLabel
            // 
            this.ServerAddressLabel.AutoSize = true;
            this.ServerAddressLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ServerAddressLabel.Font = new System.Drawing.Font("OCR A Extended", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerAddressLabel.ForeColor = System.Drawing.Color.White;
            this.ServerAddressLabel.Location = new System.Drawing.Point(20, 19);
            this.ServerAddressLabel.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this.ServerAddressLabel.Name = "ServerAddressLabel";
            this.ServerAddressLabel.Size = new System.Drawing.Size(790, 68);
            this.ServerAddressLabel.TabIndex = 2;
            this.ServerAddressLabel.Text = "Server Address";
            this.ServerAddressLabel.UseCompatibleTextRendering = true;
            // 
            // ConnectButton
            // 
            this.ConnectButton.AutoSize = true;
            this.ConnectButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ConnectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.inputLayoutPanel.SetColumnSpan(this.ConnectButton, 2);
            this.ConnectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ConnectButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ConnectButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ConnectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConnectButton.Font = new System.Drawing.Font("OCR A Extended", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.ForeColor = System.Drawing.Color.White;
            this.ConnectButton.Location = new System.Drawing.Point(20, 231);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(1620, 174);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseCompatibleTextRendering = true;
            this.ConnectButton.UseVisualStyleBackColor = false;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // logoLabel
            // 
            this.logoLabel.AutoSize = true;
            this.logoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoLabel.Font = new System.Drawing.Font("OCR A Extended", 80.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logoLabel.ForeColor = System.Drawing.Color.White;
            this.logoLabel.Location = new System.Drawing.Point(4, 0);
            this.logoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.logoLabel.Name = "logoLabel";
            this.logoLabel.Size = new System.Drawing.Size(1660, 647);
            this.logoLabel.TabIndex = 1;
            this.logoLabel.Text = "Space Wars";
            this.logoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.logoLabel.UseCompatibleTextRendering = true;
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.BackColor = System.Drawing.Color.Transparent;
            this.copyrightLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.copyrightLabel.Font = new System.Drawing.Font("OCR A Extended", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyrightLabel.ForeColor = System.Drawing.Color.White;
            this.copyrightLabel.Location = new System.Drawing.Point(0, 0);
            this.copyrightLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(1060, 59);
            this.copyrightLabel.TabIndex = 2;
            this.copyrightLabel.Text = "(C) Jiahui Chen and Mitch Talmadge";
            this.copyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.copyrightLabel.UseCompatibleTextRendering = true;
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SpaceWars.Properties.Resources.space_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1668, 1079);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.mainLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainMenuForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Space Wars";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainMenuForm_FormClosed);
            this.mainLayoutPanel.ResumeLayout(false);
            this.mainLayoutPanel.PerformLayout();
            this.inputLayoutPanel.ResumeLayout(false);
            this.inputLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel inputLayoutPanel;
        private System.Windows.Forms.Label NicknameLabel;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.TextBox ServerTextBox;
        private System.Windows.Forms.Label ServerAddressLabel;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label logoLabel;
        private System.Windows.Forms.Label copyrightLabel;
    }
}


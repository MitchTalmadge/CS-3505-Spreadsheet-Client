namespace TipCalculator
{
    partial class Form1
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
            this.TotalBillLabel = new System.Windows.Forms.Label();
            this.billTotal = new System.Windows.Forms.TextBox();
            this.CalculateButton = new System.Windows.Forms.Button();
            this.tipDisplay = new System.Windows.Forms.TextBox();
            this.tipAmountLabel = new System.Windows.Forms.Button();
            this.tipAmount = new System.Windows.Forms.TextBox();
            this.percentSign = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TotalBillLabel
            // 
            this.TotalBillLabel.AutoSize = true;
            this.TotalBillLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.TotalBillLabel.Font = new System.Drawing.Font("Comic Sans MS", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalBillLabel.Location = new System.Drawing.Point(62, 49);
            this.TotalBillLabel.Name = "TotalBillLabel";
            this.TotalBillLabel.Size = new System.Drawing.Size(314, 51);
            this.TotalBillLabel.TabIndex = 0;
            this.TotalBillLabel.Text = "Enter Total Bill:";
            // 
            // billTotal
            // 
            this.billTotal.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.billTotal.Location = new System.Drawing.Point(408, 49);
            this.billTotal.Multiline = true;
            this.billTotal.Name = "billTotal";
            this.billTotal.Size = new System.Drawing.Size(323, 51);
            this.billTotal.TabIndex = 1;
            // 
            // CalculateButton
            // 
            this.CalculateButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.CalculateButton.Font = new System.Drawing.Font("Comic Sans MS", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CalculateButton.Location = new System.Drawing.Point(71, 481);
            this.CalculateButton.Name = "CalculateButton";
            this.CalculateButton.Size = new System.Drawing.Size(208, 60);
            this.CalculateButton.TabIndex = 2;
            this.CalculateButton.Text = "Calculate Tip";
            this.CalculateButton.UseVisualStyleBackColor = false;
            this.CalculateButton.Click += new System.EventHandler(this.CalculateButton_Click);
            // 
            // tipDisplay
            // 
            this.tipDisplay.Font = new System.Drawing.Font("Comic Sans MS", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tipDisplay.Location = new System.Drawing.Point(408, 481);
            this.tipDisplay.Name = "tipDisplay";
            this.tipDisplay.ReadOnly = true;
            this.tipDisplay.Size = new System.Drawing.Size(323, 67);
            this.tipDisplay.TabIndex = 3;
            // 
            // tipAmountLabel
            // 
            this.tipAmountLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tipAmountLabel.Font = new System.Drawing.Font("Comic Sans MS", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tipAmountLabel.Location = new System.Drawing.Point(71, 274);
            this.tipAmountLabel.Name = "tipAmountLabel";
            this.tipAmountLabel.Size = new System.Drawing.Size(277, 61);
            this.tipAmountLabel.TabIndex = 4;
            this.tipAmountLabel.Text = "Tip Amount:";
            this.tipAmountLabel.UseVisualStyleBackColor = false;
            // 
            // tipAmount
            // 
            this.tipAmount.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tipAmount.Location = new System.Drawing.Point(408, 274);
            this.tipAmount.Multiline = true;
            this.tipAmount.Name = "tipAmount";
            this.tipAmount.Size = new System.Drawing.Size(323, 61);
            this.tipAmount.TabIndex = 5;
            // 
            // percentSign
            // 
            this.percentSign.AutoSize = true;
            this.percentSign.Font = new System.Drawing.Font("Comic Sans MS", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.percentSign.Location = new System.Drawing.Point(737, 279);
            this.percentSign.Name = "percentSign";
            this.percentSign.Size = new System.Drawing.Size(52, 51);
            this.percentSign.TabIndex = 6;
            this.percentSign.Text = "%";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.ClientSize = new System.Drawing.Size(823, 668);
            this.Controls.Add(this.percentSign);
            this.Controls.Add(this.tipAmount);
            this.Controls.Add(this.tipAmountLabel);
            this.Controls.Add(this.tipDisplay);
            this.Controls.Add(this.CalculateButton);
            this.Controls.Add(this.billTotal);
            this.Controls.Add(this.TotalBillLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TotalBillLabel;
        private System.Windows.Forms.TextBox billTotal;
        private System.Windows.Forms.Button CalculateButton;
        private System.Windows.Forms.TextBox tipDisplay;
        private System.Windows.Forms.Button tipAmountLabel;
        private System.Windows.Forms.TextBox tipAmount;
        private System.Windows.Forms.Label percentSign;
    }
}


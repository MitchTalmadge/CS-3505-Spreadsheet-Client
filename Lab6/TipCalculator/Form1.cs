using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            double bill = Double.Parse(billTotal.Text);
            double tip = 1 + Double.Parse(tipAmount.Text) / 100;
            double total = tip * bill;

            tipDisplay.Text = total.ToString();
        }
    }
}

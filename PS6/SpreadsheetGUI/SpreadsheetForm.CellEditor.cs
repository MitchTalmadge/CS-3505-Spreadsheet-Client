using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
   partial class SpreadsheetForm : Form
    {
        private void inputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //when enter key is pressed in text input box
            if (e.KeyChar == 13)
            {
                string input = inputTextBox.Text;

                try {
                    _spreadsheet.SetContentsOfCell(GetSelectedCellName(), input);
                    spreadsheetPanel.GetSelection(out int col, out int row);
                    //display cell's value
                    object value;
                    if ((value = _spreadsheet.GetCellValue(GetSelectedCellName())) is FormulaError error)
                    {
                        value = "N O !";
                    }
                    spreadsheetPanel.SetValue(col, row, value.ToString());
                    valueTextBox.Text = value.ToString();
                }
                catch (CircularException) {
                    MessageBox.Show("A circular dependency was found!", "Invalid Cell Input"); 
                }
                catch (InvalidNameException)
                {
                    MessageBox.Show("The name of the cell is invalid!", "Invalid Cell Input");
                }
                catch (FormulaFormatException)
                {
                    MessageBox.Show("The syntax of the formula entered is invalid!", "Invalid Cell Input");
                }
            }
        }
    }
}

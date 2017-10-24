using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetGUI.Properties;

namespace SpreadsheetGUI
{
    partial class SpreadsheetForm
    {
        private void inputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Make sure the enter button was pressed in the input box.
            if (e.KeyChar != 13)
                return;

            var input = inputTextBox.Text;

            try
            {
                _spreadsheet.SetContentsOfCell(GetSelectedCellName(), input);
                spreadsheetPanel.GetSelection(out var col, out var row);

                //display cell's value
                var value = _spreadsheet.GetCellValue(GetSelectedCellName());
                if (value is FormulaError)
                {
                    value = Resources.SpreadsheetForm_Formula_Error_Value;
                }
                
                spreadsheetPanel.SetValue(col, row, value.ToString());
                valueTextBox.Text = value.ToString();
            }
            catch (CircularException)
            {
                MessageBox.Show(Resources.SpreadsheetForm_inputTextBox_Circular_Dependency,
                    Resources.SpreadsheetForm_inputTextBox_Invalid_Cell_Input);
            }
            catch (InvalidNameException)
            {
                MessageBox.Show(Resources.SpreadsheetForm_inputTextBox_Invalid_Cell_Name,
                    Resources.SpreadsheetForm_inputTextBox_Invalid_Cell_Input);
            }
            catch (FormulaFormatException)
            {
                MessageBox.Show(Resources.SpreadsheetForm_inputTextBox_Invalid_Formula,
                    Resources.SpreadsheetForm_inputTextBox_Invalid_Cell_Input);
            }
        }
    }
}
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
    /// <inheritdoc />
    /// <summary>
    /// A partial class of SpreadsheetForm to organize cell editor logic.
    /// </summary>
    /// <authors>Jiahui Chen and Mitch Talmadge</authors>
    partial class SpreadsheetForm
    {
        /// <summary>
        /// Called when a key is pressed while the editor content text box is focused.
        /// Saves the contents when the enter key is pressed.
        /// </summary>
        /// <param name="sender">The focused text box.</param>
        /// <param name="e">An event containing the key that was pressed.</param>
        private void InputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Make sure the enter button was pressed in the input box.
            if (e.KeyChar != 13)
                return;

            try
            {
                // Set the contents of the cell, and update the values of any dependents.
                UpdateCellValues(_spreadsheet.SetContentsOfCell(GetSelectedCellName(), editorContentTextBox.Text));

                // Reselect the current cell to update its displayed value.
                SpreadsheetPanelOnSelectionChanged(spreadsheetPanel);
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

        /// <summary>
        /// Updates the values of the given cells in the spreadsheet.
        /// </summary>
        /// <param name="cellNames">The names of the cells to update.</param>
        private void UpdateCellValues(IEnumerable<string> cellNames)
        {
            foreach (var cell in cellNames)
            {
                // Get the new value.
                var value = _spreadsheet.GetCellValue(cell);
                if (value is FormulaError)
                {
                    value = Resources.SpreadsheetForm_Formula_Error_Value;
                }

                // Update the value in the spreadsheet panel.
                GetColumnAndRowFromCellName(cell, out var col, out var row);
                spreadsheetPanel.SetValue(col, row, value.ToString());
            }
        }

        /// <summary>
        /// From a cell name, determines the column and row of the cell in the spreadsheet panel.
        /// </summary>
        /// <param name="cellName">The name of the cell.</param>
        /// <param name="col">The variable to store the column in.</param>
        /// <param name="row">The variable to store the row in.</param>
        private void GetColumnAndRowFromCellName(string cellName, out int col, out int row)
        {
            // Column
            col = cellName[0] - 'A';

            // Row
            int.TryParse(cellName.Substring(1), out row);
            row = row - 1;
        }
    }
}
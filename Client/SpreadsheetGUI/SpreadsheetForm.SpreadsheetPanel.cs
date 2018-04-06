using System.Collections.Generic;
using System.Windows.Forms;
using SpreadsheetGUI.Properties;
using SpreadsheetUtilities;
using SS;

namespace SpreadsheetGUI
{
    /// <inheritdoc />
    /// <summary>
    /// A partial class of SpreadsheetForm to organize spreadsheet panel logic.
    /// </summary>
    /// <authors>Jiahui Chen and Mitch Talmadge</authors>
    partial class SpreadsheetForm
    {
        /// <summary>
        /// Called when a cell in the spreadsheet has been selected.
        /// </summary>
        /// <param name="sender">The Spreadsheet Panel containing the cell.</param>
        private void SpreadsheetPanel_SelectionChanged(SpreadsheetPanel sender)
        {
            DisplayCurrentCellInEditor();
        }

        /// <summary>
        /// Using the currently selected cell in the spreadsheet panel, displays the name, content, and value in the cell editor.
        /// </summary>
        private void DisplayCurrentCellInEditor()
        {
            // Display the cell name in the editor.
            var cellName = GetSelectedCellName();
            editorNameTextBox.Text = cellName;
            GetColumnAndRowFromCellName(cellName, out var col, out var row);

            // Display the cell contents in the editor (and add an equals sign to formulas).
            var contents = _spreadsheet.GetCellContents(GetSelectedCellName());
            if (contents is Formula)
            {
                contents = "=" + contents;
            }

            spreadsheetPanel.cellInputTextBox.Text = contents.ToString();

            // Move the text cursor to the content edit text box.
            spreadsheetPanel.cellInputTextBox.Focus();
            spreadsheetPanel.cellInputTextBox.SelectAll();

            // Display the cell value in the editor.
            // Currently, this doesn't return anything cause we aren't setting actual values in the spreadsheet
            //value = _spreadsheet.GetCellValue(cellName); 
            //if (value is FormulaError)
            //{
            //    value = Resources.SpreadsheetForm_Formula_Error_Value;
            //}
            //editorValueTextBox.Text = value.ToString();

            // SpreadsheetPanel has Dictionary of cell values (only as strings/display form)
            spreadsheetPanel.GetValue(col, row, out string val);
            editorValueTextBox.Text = val;
        }

        /// <summary>
        /// Called when enter is pressed while cell editor text box is selected. 
        /// </summary>
        /// <param name="sender">The Spreadsheet Panel containing the cell.</param>
        private void SpreadsheetPanel_CellEditEnter(SpreadsheetPanel sender)
        {
            // Display the selected cell value in the editor.
            var cellName = GetSelectedCellName();
            GetColumnAndRowFromCellName(cellName, out var col, out var row);
            // val in the spreadsheetPanel is not being set (as of now)
            //spreadsheetPanel.GetValue(col, row, out string val);

            // Setting and displaying input value in spreadsheet
            //spreadsheetPanel.SetValue(col, row, val);
            // so displaying the value of the input directly 
            spreadsheetPanel.SetValue(col, row, spreadsheetPanel.cellInputTextBox.Text);

            // Moving selection down
            spreadsheetPanel.MoveSelectionDown();
        }

        /// <summary>
        /// From a cell name, determines the column and row of the cell in the spreadsheet panel.
        /// </summary>
        /// <param name="cellName">The name of the cell.</param>
        /// <param name="col">The variable to store the column in.</param>
        /// <param name="row">The variable to store the row in.</param>
        private static void GetColumnAndRowFromCellName(string cellName, out int col, out int row)
        {
            // Column
            col = cellName[0] - 'A';

            // Row
            int.TryParse(cellName.Substring(1), out row);
            row = row - 1;
        }

        /// <summary>
        /// Determines the name of the currently selected cell in the Spreadsheet Panel.
        /// </summary>
        /// <returns>The selected cell's name.</returns>
        private string GetSelectedCellName()
        {
            spreadsheetPanel.GetSelection(out var col, out var row);
            var cellName = (char) ('A' + col) + (++row).ToString();

            return cellName;
        }

        /// <summary>
        /// Refreshes the values of the given cells on the spreadsheet panel.
        /// </summary>
        /// <param name="cellNames">The names of the cells to refresh.</param>
        private void RefreshCellValues(IEnumerable<string> cellNames)
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
        /// Clears the spreadsheet panel and sets the selection to A1.
        /// </summary>
        private void ClearSpreadsheetPanel()
        {
            spreadsheetPanel.SetSelection(0, 0);
            spreadsheetPanel.Clear();
        }
    }
}
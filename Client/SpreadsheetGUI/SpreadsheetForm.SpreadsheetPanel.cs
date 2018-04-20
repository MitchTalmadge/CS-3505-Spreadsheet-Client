using SpreadsheetGUI.Properties;
using SpreadsheetUtilities;
using SS;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <inheritdoc />
    /// <summary>
    /// A partial class of SpreadsheetForm to organize spreadsheet panel logic.
    /// </summary>
    /// <authors>Jiahui Chen, Tarun Sunkaraneni, Mark Van der Merwe and Mitch Talmadge</authors>
    partial class SpreadsheetForm
    {
        /// <summary>
        /// Called when a cell in the spreadsheet has been selected.
        /// </summary>
        /// <param name="sender">The Spreadsheet Panel containing the cell.</param>
        private void SpreadsheetPanel_SelectionChanged(SpreadsheetPanel sender)
        {
            if (_spreadsheet == null) return;
            // Display the cell name in the editor.
            var cellName = GetSelectedCellName();
            editorNameTextBox.Text = cellName;
            GetColumnAndRowFromCellName(cellName, out var col, out var row);

            // Tell Server to deselect whatever this Client was previously editing, allowing other Clients to access it
            // If the Client hasn't edited/selected a cell before, this does nothing
            networkController.Unfocus();
            // Tell Server this client is editing selected cell, and other client's shouldn't have access
            networkController.Focus(cellName);

            // Cell's contents aren't being set (yet)
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
            var value = _spreadsheet.GetCellValue(cellName);
            if (value is FormulaError)
            {
                value = Resources.SpreadsheetForm_Formula_Error_Value;
            }
            editorValueTextBox.Text = value.ToString();
        }

        /// <summary>
        /// Edits GUI of spreadsheet panel to display cells being edited by other clients on the Server.
        /// </summary>
        /// <param name="cell"></param>
        private void SpreadsheetPanel_Focus(string cell, string user)
        {
            this.Invoke(new Focus(spreadsheetPanel.Focus), new object[] { cell, user });
            /*spreadsheetPanel.Focus(cell, user)*/;
        }

        /// <summary>
        /// Edits GUI of spreadsheet panel to stop displaying cells no longer being
        /// edited by other clients on the Server.
        /// </summary>
        private void SpreadsheetPanel_Unfocus(string user)
        {
            this.Invoke(new Unfocus(spreadsheetPanel.Unfocus), new object[] { user });
            //spreadsheetPanel.Unfocus(user);
        }

        /// <summary>
        /// Called when enter is pressed while cell editor text box is selected.
        /// </summary>
        /// <param name="sender">The Spreadsheet Panel containing the cell.</param>
        private void SpreadsheetPanel_CellEditEnter(SpreadsheetPanel sender)
        {
            try
            {
                if (_spreadsheet == null) return;
                // Send edit message to Server
                networkController.Edit(GetSelectedCellName(), spreadsheetPanel.cellInputTextBox.Text);

                // Set the contents of the cell, and update the values of any dependents.
                RefreshCellValues(_spreadsheet.SetContentsOfCell(GetSelectedCellName(), spreadsheetPanel.cellInputTextBox.Text));

                // SEND EDIT SERVER MESSAGE

                //// Moving cell selection down if cell edit is valid
                spreadsheetPanel.MoveSelectionDown();
                spreadsheetPanel.cellInputTextBox.Clear();
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
        /// Called when down arrow key is pressed while cell editor text box is selected.
        /// </summary>
        /// <param name="sender">The Spreadsheet Panel containing the cell.</param>
        private void SpreadsheetPanel_CellEditDown(SpreadsheetPanel sender)
        {
            // Moving cell selection down
            spreadsheetPanel.MoveSelectionDown();
            // Changing selection display
            SpreadsheetPanel_SelectionChanged(spreadsheetPanel);
        }

        /// <summary>
        /// Called when up arrow key is pressed while cell editor text box is selected.
        /// </summary>
        /// <param name="sender">The Spreadsheet Panel containing the cell.</param>
        private void SpreadsheetPanel_CellEditUp(SpreadsheetPanel sender)
        {
            // Moving cell selection down
            spreadsheetPanel.MoveSelectionUp();
            // Changing selection display
            SpreadsheetPanel_SelectionChanged(spreadsheetPanel);
        }

        /// <summary>
        /// Called when up arrow key is pressed while cell editor text box is selected.
        /// </summary>
        /// <param name="sender">The Spreadsheet Panel containing the cell.</param>
        private void SpreadsheetPanel_CellEditRight(SpreadsheetPanel sender)
        {
            // Moving cell selection down
            spreadsheetPanel.MoveSelectionRight();
            // Changing selection display
            SpreadsheetPanel_SelectionChanged(spreadsheetPanel);
        }

        /// <summary>
        /// Called when left arrow key is pressed while cell editor text box is selected.
        /// </summary>
        /// <param name="sender">The Spreadsheet Panel containing the cell.</param>
        private void SpreadsheetPanel_CellEditLeft(SpreadsheetPanel sender)
        {
            // Moving cell selection down
            spreadsheetPanel.MoveSelectionLeft();
            // Changing selection display
            SpreadsheetPanel_SelectionChanged(spreadsheetPanel);
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
            var cellName = (char)('A' + col) + (++row).ToString();

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

        /// <summary>
        /// Clears the cell editor text box.
        /// </summary>
        private void ClearCellEditor()
        {
            spreadsheetPanel.cellInputTextBox.Clear();
        }
    }
}
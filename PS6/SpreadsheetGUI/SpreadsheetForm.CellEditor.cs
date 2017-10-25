﻿using SpreadsheetUtilities;
using SS;
using System.Collections.Generic;
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
        /// Called when a key is released while the editor content text box is focused.
        /// Saves the contents when the enter key is pressed, and selects cells with the arrow keys.
        /// </summary>
        private void editorContentTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var refresh = true;
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    SubmitChanges();
                    break;
                case Keys.Up:
                    spreadsheetPanel.MoveSelectionUp();
                    break;
                case Keys.Down:
                    spreadsheetPanel.MoveSelectionDown();
                    break;
                case Keys.Left:
                    spreadsheetPanel.MoveSelectionLeft();
                    break;
                case Keys.Right:
                    spreadsheetPanel.MoveSelectionRight();
                    break;
                default:
                    refresh = false;
                    break;
            }

            // Refresh the selected cell if needed.
            if (refresh)
                DisplayCurrentCellInEditor();
        }

        /// <summary>
        /// Updates the currently selected cell with the contents of the content input field.
        /// </summary>
        private void SubmitChanges()
        {
            try
            {
                // Set the contents of the cell, and update the values of any dependents.
                RefreshCellValues(_spreadsheet.SetContentsOfCell(GetSelectedCellName(), editorContentTextBox.Text));

                // Select cell below current cell.
                spreadsheetPanel.MoveSelectionDown();

                // Update the cell editor with the new data of the current cell.
                DisplayCurrentCellInEditor();
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
        /// Using the currently selected cell in the spreadsheet panel, displays the name, content, and value in the cell editor.
        /// </summary>
        private void DisplayCurrentCellInEditor()
        {
            // Display the cell name in the editor.
            var cellName = GetSelectedCellName();
            editorNameTextBox.Text = cellName;

            // Display the cell contents in the editor (and add an equals sign to formulas).
            var contents = _spreadsheet.GetCellContents(GetSelectedCellName());
            if (contents is Formula)
            {
                contents = "=" + contents;
            }

            editorContentTextBox.Text = contents.ToString();

            // Move the text cursor to the content edit text box.
            editorContentTextBox.Focus();
            editorContentTextBox.SelectAll();

            // Display the cell value in the editor.
            var value = _spreadsheet.GetCellValue(cellName);
            if (value is FormulaError)
            {
                value = Resources.SpreadsheetForm_Formula_Error_Value;
            }
            editorValueTextBox.Text = value.ToString();
        }

        /// <summary>
        /// Clears the cell editor.
        /// </summary>
        private void ClearCellEditor()
        {
            editorNameTextBox.Clear();
            editorContentTextBox.Clear();
            editorValueTextBox.Clear();
        }
    }
}
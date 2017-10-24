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
        private void SpreadsheetPanelOnSelectionChanged(SpreadsheetPanel sender)
        {
            // Move the text cursor to the content edit text box.
            editorContentTextBox.Focus();

            // Display the cell name in the editor.
            var cellName = GetSelectedCellName();
            editorNameTextBox.Text = cellName;

            // Display the cell value in the editor.
            object value;
            if ((value = _spreadsheet.GetCellValue(cellName)) is FormulaError)
            {
                value = Resources.SpreadsheetForm_Formula_Error_Value;
            }
            editorValueTextBox.Text = value.ToString();

            // Display the cell contents in the editor (and add an equals sign to formulas).
            var contents = _spreadsheet.GetCellContents(GetSelectedCellName());
            if (contents is Formula)
            {
                contents = "=" + contents;
            }

            editorContentTextBox.Text = contents.ToString();
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
    }
}
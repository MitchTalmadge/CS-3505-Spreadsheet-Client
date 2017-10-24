using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SS;
using SpreadsheetUtilities;

namespace SpreadsheetGUI
{
    /// <inheritdoc />
    /// <summary>
    /// The controller for the Spreadsheet GUI.
    /// </summary>
    /// <authors>Jiahui Chen and Mitch Talmadge</authors>
    public partial class SpreadsheetForm : Form
    {
        /// <summary>
        /// The version of this spreadsheet application.
        /// </summary>
        private const string SpreadsheetVersion = "ps6";

        /// <summary>
        /// The regex pattern used for validating cell names.
        /// This pattern only allows cells with columns from A to Z, and rows from 1 to 99.
        /// </summary>
        private static readonly Regex CellValidityPattern = new Regex("^[A-Z][1-9][0-9]?$");

        /// <summary>
        /// The backing spreadsheet for this form.
        /// </summary>
        private Spreadsheet _spreadsheet;

        /// <summary>
        /// The path to the file that was opened for this spreadsheet, if any.
        /// </summary>
        private string _openedFilePath;

        /// <summary>
        /// Creates a SpreadsheetForm with a new, empty spreadsheet.
        /// </summary>
        public SpreadsheetForm()
        {
            InitializeComponent();

            // Create a new, empty spreadsheet.
            _spreadsheet = new Spreadsheet(IsValid, Normalize, SpreadsheetVersion);

            // Register a listener for when a spreadsheet cell has been selected.
            spreadsheetPanel.SelectionChanged += SpreadsheetPanelOnSelectionChanged;
        }

        /// <summary>
        /// Determines if a cell name is valid (exists within the spreadsheet panel).
        /// </summary>
        /// <param name="cellName">The name of the cell to validate.</param>
        /// <returns>True if the cell name is valid, false otherwise.</returns>
        private static bool IsValid(string cellName)
        {
            return CellValidityPattern.IsMatch(cellName);
        }

        /// <summary>
        /// Normalizes the given cell name to maintain consistency.
        /// Lowercase cell names are converted to uppercase.
        /// </summary>
        /// <param name="cellName">The name of the cell to normalize.</param>
        /// <returns>The normalized cell name.</returns>
        private static string Normalize(string cellName)
        {
            return cellName.ToUpper();
        }

        private string GetSelectedCellName()
        {
            spreadsheetPanel.GetSelection(out var col, out var row);
            string cellName = ((char)('A' + col)) + (++row).ToString();

            return cellName;
        }

        /// <summary>
        /// Called when a cell in the spreadsheet has been selected.
        /// </summary>
        /// <param name="sender">The Spreadsheet Panel containing the cell.</param>
        private void SpreadsheetPanelOnSelectionChanged(SpreadsheetPanel sender)
        {
            inputTextBox.Focus();
            //displays cell's name
            var cellName = GetSelectedCellName();
            cellNameTextBox.Text = cellName;
            inputTextBox.Clear();
            //display cell's value
            object value;
            if ( (value = _spreadsheet.GetCellValue(cellName)) is FormulaError error)
            {
                value = "N O !";
            }
            valueTextBox.Text = value.ToString();
            inputTextBox.Text = _spreadsheet.GetCellContents(GetSelectedCellName()).ToString();
        }

       
    }
}
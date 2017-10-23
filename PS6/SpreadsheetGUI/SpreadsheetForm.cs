using System;
using System.Windows.Forms;
using SS;

namespace SpreadsheetGUI
{
    public partial class SpreadsheetForm : Form
    {
        /// <summary>
        /// The version of this spreadsheet application.
        /// </summary>
        private const string SpreadsheetVersion = "ps6";

        /// <summary>
        /// The backing spreadsheet for this form.
        /// </summary>
        private Spreadsheet _spreadsheet;

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
        private bool IsValid(string cellName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Normalizes the given cell name to maintain consistency.
        /// Lowercase cell names are converted to uppercase.
        /// </summary>
        /// <param name="cellName">The name of the cell to normalize.</param>
        /// <returns>The normalized cell name.</returns>
        private string Normalize(string cellName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when a cell in the spreadsheet has been selected.
        /// </summary>
        /// <param name="sender">The Spreadsheet Panel containing the cell.</param>
        private void SpreadsheetPanelOnSelectionChanged(SpreadsheetPanel sender)
        {
            throw new NotImplementedException();
            //TODO: User has selected a cell in the spreadsheet
        }
    }
}
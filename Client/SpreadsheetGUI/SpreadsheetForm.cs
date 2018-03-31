using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SpreadsheetGUI.Properties;
using SS;

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
        /// Do not set this variable directly. Instead, use OpenedFilePath.
        /// </summary>
        /// <see cref="OpenedFilePath"/>
        private string _openedFilePath;

        /// <summary>
        /// The path to the file that was opened for this spreadsheet, if any.
        /// </summary>
        private string OpenedFilePath
        {
            get => _openedFilePath;

            set
            {
                _openedFilePath = value;

                // Find the name of the file, without extension.
                var fileName = value != null ? Path.GetFileNameWithoutExtension(value) : "Untitled";

                // Update the title bar of the application.
                Text = fileName + Resources.SpreadsheetForm_Title_Suffix;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a SpreadsheetForm with a new, empty spreadsheet.
        /// </summary>
        public SpreadsheetForm()
        {
            InitializeComponent();

            // Create a new, empty spreadsheet.
            _spreadsheet = new Spreadsheet(IsValid, Normalize, SpreadsheetVersion);

            // Select the first cell.
            spreadsheetPanel.SetSelection(0, 0);
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a SpreadsheetForm by loading the provided file.
        /// </summary>
        /// <param name="filePath">The path to the spreadsheet file to load.</param>
        public SpreadsheetForm(string filePath) : this()
        {
            OpenSpreadsheet(filePath);
        }

        /// <summary>
        /// Called when the form has been requested to be closed.
        /// Checks for changes before allowing the form to close.
        /// </summary>
        /// <param name="sender">The form.</param>
        /// <param name="e">The form closing event.</param>
        private void SpreadsheetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cancels the close if the form needs to be saved.
            e.Cancel = !SaveIfNeeded();
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

        /// <summary>
        /// Checks if the spreadsheet has been changed, and if so, asks the user if they want to save their changes.
        /// The spreadsheet will be saved as needed.
        /// 
        /// Alternatively, the user has the ability to cancel the operation that called this method, 
        /// which will cause this method to return false.
        /// </summary>
        /// <returns>True if the spreadsheet is saved/not saved as desired, false if the user chose to cancel the operation.</returns>
        private bool SaveIfNeeded()
        {
            // Check for changes.
            if (!_spreadsheet.Changed)
                return true;

            var result = MessageBox.Show(Resources.SpreadsheetForm_Unsaved_Changes_Text,
                Resources.SpreadsheetForm_Unsaved_Changes_Caption,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3);

            switch (result)
            {
                case DialogResult.Cancel:
                    // Cancel the operation.
                    return false;
                case DialogResult.Yes:
                    SaveSpreadsheet();
                    break;
            }

            return true;
        }

        /// <summary>
        /// Saves the spreadsheet to the most recently opened or saved file path. 
        /// If no file was opened/saved before, or chooseFile is true, asks the user to select a place to save.
        /// </summary>
        /// <param name="chooseFile">True if the user should choose a file to save to.</param>
        private void SaveSpreadsheet(bool chooseFile = false)
        {
            // Save to the currently open file (if we have saved/opened before)
            if (!chooseFile && OpenedFilePath != null)
            {
                _spreadsheet.Save(OpenedFilePath);
            }
            else
            {
                // File was not opened or has not been saved before.

                // Ask the user to select a file to save to.
                var fileDialog = new SaveFileDialog {Filter = Resources.SpreadsheetForm_File_Extension_Filter};

                // If no file was chosen (cancelled) then return.
                if (fileDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                // Save the spreadsheet and record its path.
                var filePath = fileDialog.FileName;
                _spreadsheet.Save(filePath);
                OpenedFilePath = filePath;
            }
        }

        /// <summary>
        /// Opens a spreadsheet from a file, replacing the contents of the current spreadsheet (saving if needed).
        /// </summary>
        /// <param name="fileName"> An optional file path to open. If not specified, a dialog box will be opened for choosing the file.</param>
        private void OpenSpreadsheet(string fileName=null)
        {
            // Show a file chooser if no file path was provided.
            if (fileName == null)
            {
                // Opens the menu for user to choose file to open
                var fileDialogue = new OpenFileDialog {Filter = Resources.SpreadsheetForm_File_Extension_Filter};

                // If no file was opened, then return.
                if (fileDialogue.ShowDialog(this) != DialogResult.OK)
                    return;

                // Allow the user to save their changes if needed.
                if (!SaveIfNeeded())
                    return;


                fileName = fileDialogue.FileName;
            }

            // Attempt to create a new spreadsheet from the given file.
            Spreadsheet spreadsheet;
            try
            {
                spreadsheet = new Spreadsheet(fileName, IsValid, Normalize, SpreadsheetVersion);
            }
            catch (SpreadsheetReadWriteException)
            {
                MessageBox.Show(Resources.SpreadsheetForm_OpenSpreadsheet_LoadFailMessage,
                    Resources.SpreadsheetForm_OpenSpreadsheet_LoadFailTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Clear the current spreadsheet.
            ClearSpreadsheet();

            // Load the new spreadsheet
            _spreadsheet = spreadsheet;
            OpenedFilePath = fileName;

            // Load the data from the new spreadsheet into the spreadsheet panel.
            RefreshCellValues(_spreadsheet.GetNamesOfAllNonemptyCells());

            // Select the first cell.
            spreadsheetPanel.SetSelection(0, 0);
        }

        /// <summary>
        /// Disconnects the current client instance from the server it is connected to.
        /// </summary>
        private void DisconnectSpreadsheet()
        {
            //TODO. Everything L O L.
        }

            /// <summary>
            /// Clears all parts of the spreadsheet GUI, selects A1, and sets the spreadsheet to null.
            /// </summary>
            private void ClearSpreadsheet()
        {
            ClearSpreadsheetPanel();
            ClearCellEditor();

            _spreadsheet = null;
            OpenedFilePath = null;
        }
    }
}
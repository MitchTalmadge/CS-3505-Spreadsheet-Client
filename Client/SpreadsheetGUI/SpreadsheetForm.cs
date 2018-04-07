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
    /// <authors>Jiahui Chen, Tarun Sunkaraneni, Mark Van der Merwe and Mitch Talmadge</authors>
    public partial class SpreadsheetForm : Form
    {

        /// <summary>
        /// The regex pattern used for validating cell names.
        /// This pattern only allows cells with columns from A to Z, and rows from 1 to 99.
        /// </summary>
        private static readonly Regex CellValidityPattern = new Regex("^[A-Z][1-9][0-9]?$");

        /// <summary>
        /// The backing spreadsheet for this form.
        /// </summary>
        private Spreadsheet _spreadsheet;

        /// <inheritdoc />
        /// <summary>
        /// Creates a SpreadsheetForm with a new, empty spreadsheet.
        /// </summary>
        public SpreadsheetForm()
        {
            InitializeComponent();
            this.spreadsheetPanel.ReadOnly(true);
            this.documentNameTextBox.ReadOnly = true;
            // Create a new, empty spreadsheet.
            _spreadsheet = new Spreadsheet(IsValid, Normalize);

            this.connectedServerTextBox.Focus();
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a SpreadsheetForm by loading the provided file. Since to "open" we need to have access to the server,
        /// that is a required argument here. 
        /// </summary>
        /// <param name="serverAddress">The address of the server we need to connect to.</param>
        public SpreadsheetForm(string serverAddress) : this()
        {
            OpenSpreadsheet(serverAddress);
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
        /// Disconnects the current client instance from the server it is connected to. This essentially resets this instance of the client to 
        /// its startup state
        /// </summary>
        private void DisconnectSpreadsheet()
        {
            ClearSpreadsheet();
            //TODO. Disconnect logic.

        }

        /// <summary>
        /// Clears the cells in this sheet and enables the user to edit the document textbox to open another document in the same server.
        /// </summary>
        /// <param name="serverAddress">The address of the server we need to connect to.</param>
        private void OpenSpreadsheet(string serverAddress)
        {
            if (!this.connectedServerTextBox.ReadOnly)
            {
                ClearSpreadsheet();
                return;
            }
            ClearSpreadsheet();
            //TODO Open a new instance of the spreadsheet. Connect to the Server
            this.connectedServerTextBox.Text = serverAddress;
            this.connectedServerTextBox.ReadOnly = true;
            this.spreadsheetPanel.ReadOnly(true);
        }

        /// <summary>
        /// Clears all parts of the spreadsheet GUI, selects A1, and sets the spreadsheet to null.
        /// </summary>
        private void ClearSpreadsheet()
        {
            ClearSpreadsheetPanel();
            //ClearCellEditor();

            this.documentNameTextBox.Text = "";
            this.documentNameTextBox.ReadOnly = false;
            this.connectedServerTextBox.Text = "";
            this.connectedServerTextBox.ReadOnly = false;

            _spreadsheet = new Spreadsheet(IsValid, Normalize);
        }

        private void connectedServerTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if ( !((TextBox)sender).ReadOnly &&  e.KeyCode == Keys.Enter)
            {
                //ESTABLISH SERVER CONNECTION
                this.documentNameTextBox.ReadOnly = false;
                this.connectedServerTextBox.ReadOnly = true;
                this.documentNameTextBox.Focus();
            }
        }

        private void documentNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!((TextBox)sender).ReadOnly && e.KeyCode == Keys.Enter)
            {
                //RETRIEVE DOCUMENT
                this.documentNameTextBox.ReadOnly = true;
                this.spreadsheetPanel.cellInputTextBox.Focus();

            }
        }
    }
}
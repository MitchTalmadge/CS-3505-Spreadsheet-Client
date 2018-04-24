using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SS;
using System;

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
        /// The backing spreadsheet for this form.
        /// </summary>
        private Spreadsheet _spreadsheet;

        private NetworkController networkController;

        /// <summary>
        /// Delegate used to display unfocusing of other clients on the same server.
        /// </summary>
        /// <param name=""></param>
        //private Action<string> unfocus;
        private delegate void Unfocus(string user);

        /// <summary>
        /// Delegate used to display unfocusing of other clients on the same server.
        /// </summary>
        /// <param name=""></param>
        //private Action<string, string> focus;
        private delegate void Focus(string cell, string user);

        /// <inheritdoc />
        /// <summary>
        /// Creates a SpreadsheetForm with a new, empty spreadsheet.
        /// </summary>
        public SpreadsheetForm()
        {
            InitializeComponent();

            networkController = new NetworkController(ConnectionFailed, ConnectionSucceded, RecieveDocumentsList,
                CreateSpreadsheet, SpreadsheetPanel_Focus, SpreadsheetPanel_Unfocus, EditSpreadsheet, ClearSpreadsheet);

            // this.spreadsheetPanel.ReadOnly(true);
            documentNameDropdown.Enabled = false;
            spreadsheetPanel.ReadOnly = true;
            undoButton.Enabled = false;
            revertButton.Enabled = false;

            // Create a new, empty spreadsheet.
            _spreadsheet = null;

            // start connection handshake with server
            connectedServerTextBox.Focus();
            registerServerConnect_backgroundworker();
        }

        /// <summary>
        /// Disconnects the current client instance from the server it is connected to. This essentially resets this instance of the client to
        /// its startup state
        /// </summary>
        private void DisconnectSpreadsheet()
        {
            // disconnecting from Server and cleanin up socket
            networkController.Disconnect();
            ClearSpreadsheet();
        }

        /// <summary>
        /// Clears all parts of the spreadsheet GUI, selects A1, and sets the spreadsheet to null.
        /// </summary>
        private void ClearSpreadsheet()
        {
            Invoke(new MethodInvoker(() =>
            {
                ClearSpreadsheetPanel();
                ClearCellEditor();
                networkController._socketState?.Disconnect();

                documentNameDropdown.Text = "";
                documentNameDropdown.Items.Clear();
                documentNameDropdown.Enabled = false;
                spreadsheetPanel.cellInputTextBox.ReadOnly = true;
                connectedServerTextBox.Text = "";
                connectedServerTextBox.ReadOnly = false;
                Text = "Untitled - Spreadsheet 3505";
                documentNameLabel.Text = "Document Name: ";

                _spreadsheet = null;
            }));
        }

        /// <summary>
        /// Event handler for when SpreadsheetForm is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseSpreadsheet(object sender, FormClosingEventArgs e)
        {
            DisconnectSpreadsheet();
        }

        /////////////////// Events ////////////////////

        private void connectedServerTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!((TextBox)sender).ReadOnly && e.KeyCode == Keys.Enter)
            {
                connectedServerTextBox.ReadOnly = true;
                serverConnect_backgroundworker.RunWorkerAsync(connectedServerTextBox.Text);
            }
        }

        /// <summary>
        /// Called when a spreadsheet is selected in the dropdown menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void documentNameDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox documentsDropdown = (ComboBox)sender;
            // IF we check for the index instead of text, a spreadsheet named "New..." wouldn't become a loophole anymore
            if (documentsDropdown.SelectedIndex.Equals(documentsDropdown.Items.Count - 1))
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Document Name",
                    "New Document on " + connectedServerTextBox.Text);
                if (input.Length > 0)
                {
                    Text = input + " - Spreadsheet 3505";
                    documentNameLabel.Text = "Document Name: " + input;
                    networkController.Load(input);
                }
            }
            else
            {
                // sending load message to the Server, to retreive the selected spreadsheet
                networkController.Load(documentNameDropdown.SelectedItem.ToString());
                Text = documentNameDropdown.SelectedItem.ToString() + " - Spreadsheet 3505";
                documentNameLabel.Text = "Document Name: " + documentNameDropdown.SelectedItem.ToString();
                spreadsheetPanel.cellInputTextBox.Focus();
            }

            documentNameDropdown.Enabled = false;
            undoButton.Enabled = true;
            revertButton.Enabled = true;
        }

        private void registerServerConnect_backgroundworker()
        {
            // What our background worker for establishing a connection has to do
            serverConnect_backgroundworker.DoWork += (backgrounWorker_Sender, backgroundWorker_e) =>
            {
                networkController.ConnectToServer((String)backgroundWorker_e.Argument);
            };
            // what happens when the background worker either successfully connects or fails connecting to the given name
            serverConnect_backgroundworker.RunWorkerCompleted += (backgrounWorker_Sender, backgroundWorker_e) =>
            {
                if (backgroundWorker_e.Error is ArgumentException)
                {
                    MessageBox.Show(backgroundWorker_e.Error.Message + "\nTry again!", "Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    connectedServerTextBox.ReadOnly = false;
                }
            };
        }

        /// <summary>
        /// Simple Gui event to trigger Undo command everytime
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoButton_MouseClick(object sender, MouseEventArgs e)
        {
            networkController.Undo();
        }

        /// <summary>
        ///  Gui event to trigger Revert command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void revertButton_Click(object sender, EventArgs e)
        {
            spreadsheetPanel.GetSelection(out int column, out int row);
            networkController.Revert(((char)(column + 'A')).ToString() + (row + 1).ToString());
        }

        ////////////////////////// Network Controller Delegates /////////////////////////////////////////
        /// <summary>
        /// Called when a connection to the server has failed.
        /// Displays a warning message dialog.
        /// </summary>
        /// <param name="reason">Why the connection failed.</param>
        private void ConnectionFailed(string reason)
        {
            Invoke(new MethodInvoker(() =>
            {
                connectedServerTextBox.ReadOnly = false;
                MessageBox.Show(reason,
                    "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (reason.Contains("Unable to open")) documentNameDropdown.Enabled = true;
            }));
        }

        /// <summary>
        /// Called when a connection to the SpaceWars server has failed.
        /// Displays a warning message dialog.
        /// </summary>
        /// <param name="reason">Why the connection failed.</param>
        private void ConnectionSucceded(string message)
        {
            Invoke(new MethodInvoker(() =>
            {
                MessageBox.Show(message,
                    "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }));
        }

        /// <summary>
        /// Called when a handshake message from the server has been sent to the client, along with a list of all documents on the server
        /// </summary>
        /// <param name="reason">Why the connection failed.</param>
        private void RecieveDocumentsList(string[] documents)
        {
            Invoke(new MethodInvoker(() =>
            {
                documentNameDropdown.Enabled = true;
                documentNameDropdown.Focus();
                documentNameDropdown.Items.AddRange(documents);
                documentNameDropdown.Items.Add("New...");
            }));
        }

        /// <summary>
        /// This is how we are able to edit our spreadsheet without having threading issues, the Networking Controller should
        /// be calling this often
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="content"></param>
        private void EditSpreadsheet(string cell, string content)
        {
            Invoke(new MethodInvoker(() =>
            {
                RefreshCellValues(_spreadsheet.SetContentsOfCell(cell, content));
                spreadsheetPanel.cellInputTextBox.Text = _spreadsheet.GetCellContents(GetSelectedCellName()).ToString();
            }));
        }

        /// <summary>
        ///
        /// </summary>
        private void CreateSpreadsheet()
        {
            Invoke(new MethodInvoker(() =>
            {
                _spreadsheet = new Spreadsheet();
                spreadsheetPanel.ReadOnly = false;
            }));
        }
    }
}
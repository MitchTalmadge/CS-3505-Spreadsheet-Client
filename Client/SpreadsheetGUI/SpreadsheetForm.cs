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

        /// <inheritdoc />
        /// <summary>
        /// Creates a SpreadsheetForm with a new, empty spreadsheet.
        /// </summary>
        public SpreadsheetForm()
        {
            InitializeComponent();

            networkController = new NetworkController(this.ConnectionFailed, this.ConnectionSucceded, this.RecieveDocumentsList);
            // this.spreadsheetPanel.ReadOnly(true);
            this.documentNameDropdown.Enabled = false;
            // Create a new, empty spreadsheet.
            _spreadsheet = new Spreadsheet();
            this.connectedServerTextBox.Focus();
            this.registerServerConnect_backgroundworker();
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
            // this.spreadsheetPanel.ReadOnly(true);
        }

        /// <summary>
        /// Clears all parts of the spreadsheet GUI, selects A1, and sets the spreadsheet to null.
        /// </summary>
        private void ClearSpreadsheet()
        {
            ClearSpreadsheetPanel();
            ClearCellEditor();

            this.documentNameDropdown.Text = "";
            this.documentNameDropdown.Items.Clear();
            this.documentNameDropdown.Enabled = false;
            this.connectedServerTextBox.Text = "";
            this.connectedServerTextBox.ReadOnly = false;

            _spreadsheet = null;
        }

        /////////////////// Event ////////////////////

        private void connectedServerTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!((TextBox)sender).ReadOnly && e.KeyCode == Keys.Enter)
            {
                this.connectedServerTextBox.ReadOnly = true;
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
                Invoke(new MethodInvoker(() =>
                {
                    string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Document Name",
                        "New Document on",
                        "Default",
                        0,
                        0);
                }));
                _spreadsheet = new Spreadsheet();
            }
            else
            {
                // sending load message to the Server, to retreive the selected spreadsheet
                networkController.Load(documentNameDropdown.SelectedItem.ToString());

                this.spreadsheetPanel.cellInputTextBox.Focus();
            }

            // this.spreadsheetPanel.ReadOnly(false);
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
                    this.connectedServerTextBox.ReadOnly = false;
                }
            };
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
                this.connectedServerTextBox.ReadOnly = false;
                MessageBox.Show(reason,
                    "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                this.documentNameDropdown.Enabled = true;
                this.documentNameDropdown.Focus();
                this.documentNameDropdown.Items.AddRange(documents);
                this.documentNameDropdown.Items.Add("New...");
            }));
        }
    }
}
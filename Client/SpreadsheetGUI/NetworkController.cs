using Networking;
using SS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controls the interactions with the Network for the Spreadsheet.
    /// </summary>
    public class NetworkController
    {
        /// <summary>
        /// String constants, specified by protocl, used in communication messages.
        /// </summary>
        private static readonly string END_OF_TEXT = AbstractNetworking.END_OF_TEXT;

        private static readonly string PING = "ping " + END_OF_TEXT; // "ping \3"
        private static readonly string PING_RESPONSE = "ping_response " + END_OF_TEXT; // "ping_response \3"
        private static readonly string FOCUS_PREFIX = "focus "; // “focus A9:unique_1\3”
        private static readonly string UNFOCUS_PREFIX = "unfocus "; // “unfocus unique1\3”
        private static readonly string LOAD_PREFIX = "load ";
        public static readonly string REVERT_PREFIX = "revert ";
        public static readonly string EDIT_PREFIX = "edit ";

        /// <summary>
        /// String constants, specified by protocl, used in Server's
        /// messages that are received.
        /// </summary>
        private static readonly string CONNECTION_ACCEPTED_PREFIX = "connect_accepted "; // "connect_accepted sales\nmarketing ideas\nanother_sheet\3" or "connect_accepted\3"

        private static readonly string FULL_STATE_PREFIX = "full_state "; // "full_state A6:3\nA9:=A6/2\n\3" or "full_state \3"
        private static readonly string CHANGE_PREFIX = "change "; // "change A4:=A1+A3\3"
        private static readonly string FILE_LOAD_ERROR = "file_load_error " + END_OF_TEXT; // "file_load_error \3"

        /// <summary>
        /// String constants, specified by protocl, used in Client's
        /// messages that are sent to the Server.
        /// </summary>
        public static readonly string REGISTER = "register " + END_OF_TEXT; // "register \3"

        public static readonly string DISCONNECT = "disconnect " + END_OF_TEXT; // "disconnect \3"
        public static readonly string UNDO = "undo " + END_OF_TEXT;

        /// <summary>
        /// Timer that ensures the Client pings the Server every 10 seconds (10000ms)
        /// </summary>
        private Timer pingTimer;

        /// <summary>
        /// Timer that ensures a Server's ping_response is received every 60
        /// seconds (60000ms) to validate Server is still up.
        /// </summary>
        private Timer serverTimer;

        /// <summary>
        /// Delegate called when an error is ocurrred in the connection.
        /// </summary>
        private event Action<string> ErrorCallback;

        /// <summary>
        /// Delegate called when the connection is successful.
        /// </summary>
        private event Action<string> SuccessfulConnection;

        /// <summary>
        /// Delegate called to populate documents.
        /// </summary>
        private event Action<string[]> PopulateDocumentListCallback;

        private event Action CreateSpreadsheet;

        /// <summary>
        /// Delegate called when Focus GUI update must be done.
        /// First parameter is cell id, second is user id.
        /// </summary>
        private readonly Action<string, string> FocusCallback;

        /// <summary>
        /// Delegate called when Focus GUI update must be done.
        /// Parameter is user id.
        /// </summary>
        private readonly Action<string> UnfocusCallback;

        /// <summary>
        /// This is how we'll be able to edit our spreadsheet.
        /// </summary>
        private event Action<string, string> SpreadsheetEditCallback;

        /// <summary>
        /// This event is fired when the connection to the server has been lost,
        /// whether unexpectedly or by calling the Disconnect method.
        /// </summary>
        public event Action Disconnected;

        /// <summary>
        /// Is called upon whenever a Disconnect Method is called
        /// </summary>
        public event Action DisconnectSpreadsheetCallback;

        /// <summary>
        /// Socket that the connection is made through.
        /// </summary>
        public SocketState _socketState;

        //if (this._spreadsheet == null) { this._spreadsheet = new Spreadsheet(); this.spreadsheetPanel.ReadOnly = false; }
        /// <summary>
        /// Creates a Network Controller. All the parameters are delegates which will be called based on events that occur
        /// in this controller. These delegates are all going to affect the GUI somehow.
        /// </summary>
        /// <param name="ErrorCallback"> Whenever an error happens, this will be used to show the user a message describing the problem </param>
        /// <param name="SuccessfulConnection"> When a successful connection happens, we will notify the user that they were able to connect </param>
        /// <param name="PopulateDocumentListCallback">triggers populating dropdown with options of documents </param>
        public NetworkController(Action<string> ErrorCallback, Action<string> SuccessfulConnection, Action<string[]> PopulateDocumentListCallback,
            Action CreateSpreadsheet, Action<string, string> FocusCallback, Action<string> UnfocusCallback, Action<string, string> SpreadsheetEditCallback,
            Action DisconnectSpreadsheetCallback)
        {
            this.ErrorCallback = ErrorCallback;
            this.SuccessfulConnection = SuccessfulConnection;
            this.PopulateDocumentListCallback = PopulateDocumentListCallback;
            this.CreateSpreadsheet = CreateSpreadsheet;
            this.FocusCallback = FocusCallback;
            this.UnfocusCallback = UnfocusCallback;
            this.SpreadsheetEditCallback = SpreadsheetEditCallback;
            this.DisconnectSpreadsheetCallback = DisconnectSpreadsheetCallback;
        }

        /// <summary>
        /// This attempts to connect to the server at the ip provided.
        /// </summary>
        /// <param name="server">String ip of the server.</param>
        /// <param name="name">Name to send.</param>
        public void ConnectToServer(String server, String name = "Tarun")
        {
            // This is where we connect to the server for the first time. After the setup is done we
            // want our callback to be FirstContact.
            ClientNetworking.ConnectToServer(server,
                state =>
                {
                    SuccessfulConnection("Connected to host: " + server);

                    _socketState = state;

                    // Listen for when data is received on the socket.
                    _socketState.DataReceived += DataReceived;

                    // Listen for when the socket disconnects.
                    _socketState.Disconnected += () => { Disconnected?.Invoke(); };

                    // Send the register message with the server.
                    System.Diagnostics.Debug.WriteLine(REGISTER, "sending register message");
                    AbstractNetworking.Send(state, REGISTER);

                    // Wait for data.
                    AbstractNetworking.GetData(state);
                },
                reason => ErrorCallback(reason));
        }

        /// <summary>
        /// Sends a message to the server requesting to load a spreadsheet of the parameter name.
        /// </summary>
        /// <param name="name"></param>
        public void Load(String name)
        {
            // protocol-specified string for a load message to the Server
            string loadMessage = LOAD_PREFIX + name + END_OF_TEXT;

            // sending the message to the Server
            AbstractNetworking.Send(_socketState, loadMessage);
        }

        /// <summary>
        /// Sends a message to the server requesting an Undo action.
        /// </summary>
        public void Undo()
        {
            AbstractNetworking.Send(_socketState, UNDO);
        }

        /// <summary>
        /// Sends a message to the server requesting a Revert action with the specified cell.
        /// </summary>
        /// <param name="cell"></param>
        public void Revert(String cell)
        {
            AbstractNetworking.Send(_socketState, REVERT_PREFIX + cell + END_OF_TEXT);
        }

        /// <summary>
        /// Sends a message to the server requesting an Edit action with the specified cell.
        /// </summary>
        /// <param name="cell"></param>
        public void Edit(String cell, string content)
        {
            AbstractNetworking.Send(_socketState, EDIT_PREFIX + cell + ":" + content + END_OF_TEXT);
        }

        /// <summary>
        /// Sends a message to the server requesting a Focus action with the specified cell.
        /// Used so other clients may be notified that this client is editing a cell.
        /// </summary>
        /// <param name="cell"></param>
        public void Focus(String cell)
        {
            AbstractNetworking.Send(_socketState, FOCUS_PREFIX + cell + END_OF_TEXT);
        }

        /// <summary>
        /// Sends a message to the server requesting a Unfocus action with the specified cell.
        /// Used so other clients may be notified that this client has stopped editing a cell.
        /// </summary>
        public void Unfocus()
        {
            AbstractNetworking.Send(_socketState, UNFOCUS_PREFIX + END_OF_TEXT);
        }

        /// <summary>
        /// Sends a ping message to the Server, is the delegate used when the pingTimer's Elapse event occurs.
        /// </summary>
        public void Ping(object sender, ElapsedEventArgs e)
        {
            AbstractNetworking.Send(_socketState, PING);
        }

        /// <summary>
        /// Called when data is received on the socket.
        /// </summary>
        /// <param name="data">The data that was received.</param>
        public void DataReceived(string data)
        {
            String[] commands = data.Split((char)3);

            foreach ( string message in commands)
            {
                System.Diagnostics.Debug.WriteLine(data, "data recieved from from server ");
                // If a disconnect message is received, Disconnect the client
                if (message.Equals(DISCONNECT)) DisconnectSpreadsheetCallback();

                // If a ping is received from the Server, send a ping_response back
                if (message.Equals(PING))
                {
                    AbstractNetworking.Send(_socketState, PING_RESPONSE);
                }

                // If a ping response is received from the Server, the Server ping response timer is reset
                if (message.Equals(PING_RESPONSE))
                {
                    // timer ensuring Server is still up resets, Server has another 60 seconds until
                    // another ping_response is necessary
                    serverTimer.Stop(); serverTimer.Start();
                }

                // We know the first packet has been handled once the world is not null.
                if (message.Equals(FILE_LOAD_ERROR) || message.StartsWith(CONNECTION_ACCEPTED_PREFIX))
                    ParseFirstPacket(message);
                else
                {
                    // full_state is only received upon initial loading of spreadsheet
                    // and the ping loop begins after the full_state message is received
                    if (message.StartsWith(FULL_STATE_PREFIX))
                    {
                        FullStateDocumentDocument(message);

                        // if serverTimer reaches 60s, disconnect from the Server
                        serverTimer = new Timer(60000);
                        serverTimer.Enabled = true;
                        serverTimer.AutoReset = false;
                        serverTimer.Elapsed += new System.Timers.ElapsedEventHandler(Disconnect);

                        // every 10 seconds (10000 milliseconds) another ping is sent to the Server
                        pingTimer = new Timer(10000);
                        pingTimer.Enabled = true;
                        pingTimer.Elapsed += new System.Timers.ElapsedEventHandler(Ping);

                        // ping loop begins as both timers are started
                        pingTimer.Start();
                        serverTimer.Start();
                    }
                    else if (message.StartsWith(CHANGE_PREFIX))
                        ChangeDocument(message);
                    else if (message.StartsWith(FOCUS_PREFIX))
                        Focus_Cell(message, FOCUS_PREFIX);
                    else if (message.StartsWith(UNFOCUS_PREFIX))
                        Unfocus_Cell(message, UNFOCUS_PREFIX);
                }
            
            }

            // Get new data.
            AbstractNetworking.GetData(_socketState);
        }

        /// <summary>
        /// Calls the FocusCallback to update GUI display to reflect another client editing a cell.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="command"></param>
        private void Focus_Cell(string data, string command)
        {
            // first item should be cell id, second item should be user id
            string[] focusCell = data.Replace(command, "").Split(':');
            FocusCallback(focusCell[0], focusCell[1]);
        }

        /// <summary>
        /// Calls the unFocusCallback to update GUI display to reflect another client has stopped editing a cell.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="command"></param>
        private void Unfocus_Cell(string data, string command)
        {
            // getting user id
            string userid = data.Replace(command, "");
            UnfocusCallback(userid);
        }

        /// <summary>
        /// Processes full_state and change messages, receiving a complete spreadsheet's data and
        /// loading it into the spreadsheet.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="command"></param>
        private void ChangeDocument(string data)
        {
            string[] cellValue = data.Replace(CHANGE_PREFIX, "").Replace(END_OF_TEXT, "").Split(':').ToArray();
            if (cellValue.Length >= 2)
                this.SpreadsheetEditCallback(cellValue[0], cellValue[1]);
        }

        /// <summary>
        /// Processes full_state message, receiving a complete spreadsheet's data and
        /// loading it into the spreadsheet.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="command"></param>
        private void FullStateDocumentDocument(string data)
        {
            string[] cellContents = data.Replace(END_OF_TEXT, "").Replace(FULL_STATE_PREFIX, "").Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            this.CreateSpreadsheet();
            if (cellContents.Length > 0)
                foreach (string content in cellContents)
                {
                    string[] cellValue = content.Split(':').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    this.SpreadsheetEditCallback(cellValue[0], cellValue[1]);
                }
        }

        /// <summary>
        /// This method will be in charge of sending messages in a appropriate manner to the client.
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage(string data)
        {
            if (_socketState != null)
            {
                data += END_OF_TEXT;
                AbstractNetworking.Send(_socketState, data);
            }
        }

        /// <summary>
        /// Parses the first packet sent by the server, which contains the documents the current server holds.
        /// </summary>
        /// <param name="data">The first packet's data.</param>
        private void ParseFirstPacket(string data)
        {
            if (data.Equals(FILE_LOAD_ERROR))
            {
                ErrorCallback("Unable to open or create requested spreadhseet. Try again or use a different name");
            }
            else
            {
                string[] documents = data.Replace(END_OF_TEXT, "").Replace(CONNECTION_ACCEPTED_PREFIX, "").Trim().Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                PopulateDocumentListCallback(documents);
            }
            // Notify the listener that the connection was established and the world is ready.
            //_connectionEstablishedCallback(this);
        }

        /// <summary>
        /// Disconnects from the server.
        /// This should reset this client to default state.
        /// </summary>
        public void Disconnect()
        {
            Debug.WriteLine("disconnecting");
            // Sending disconnect message to the server, if state isn't null
            if (_socketState != null)
            {
                AbstractNetworking.Send(_socketState, DISCONNECT);
            }
            // stopping timers
            if (pingTimer != null && serverTimer != null)
            {
                pingTimer.Stop();
                serverTimer.Stop();
            }

            DisconnectSpreadsheetCallback();
        }

        /// <summary>
        /// Disconnect wrapper delegate so it may handle Elapsed events of Timers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Disconnect(object sender, ElapsedEventArgs e)
        {
            Disconnect();
        }
    }
}
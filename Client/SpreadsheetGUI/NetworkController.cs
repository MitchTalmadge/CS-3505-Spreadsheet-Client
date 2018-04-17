using Networking;
using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controls the interactions with the Network for the Spreadsheet.
    /// </summary>
    public class NetworkController
    {
        private static readonly string END_OF_TEXT = ((Char)3).ToString();
        private static readonly string PING = "ping" + END_OF_TEXT; // "ping \3"
        private static readonly string PING_RESPONSE = "ping_response" + END_OF_TEXT; // "ping_response \3"
        private static readonly string FOCUS_PREFIX = "focus"; // “focus A9:unique_1\3”
        private static readonly string UNFOCUS_PREFIX = "unfocus"; // “unfocus unique1\3”

        //////////////////////// Recieve specific Constants
        private static readonly string CONNECTION_ACCEPTED_PREFIX = "connect_accepted"; // "connect_accepted sales\nmarketing ideas\nanother_sheet\3" or "connect_accepted\3"

        private static readonly string FULL_STATE_PREFIX = "full_State"; // "full_state A6:3\nA9:=A6/2\n\3" or "full_state \3"
        private static readonly string CHANGE_PREFIX = "change"; // "change A4:=A1+A3\3"
        private static readonly string FILE_LOAD_ERROR = "file_load_error" + END_OF_TEXT; // "file_load_error \3"

        //////////////////////// Send specific Constants
        public static readonly string REGISTER = "register" + END_OF_TEXT; // "register \3"

        public static readonly string DISCONNECT = "disconnect" + END_OF_TEXT; // "disconnect \3"

        /// <summary>
        /// Delegates from the main form invoked in this controller
        /// </summary>
        private readonly Action<string> ErrorCallback;

        private readonly Action<string> SuccessfulConnection;
        private readonly Action<string[]> PopulateDocuments;

        // This is the same Spreadsheet backing the original document.
        private Spreadsheet backingSheet;

        /// <summary>
        /// This event is fired when the connection to the server has been lost,
        /// whether unexpectedly or by calling the Disconnect method.
        /// </summary>
        public event Action Disconnected;

        /// <summary>
        /// Socket that the connection is made through.
        /// </summary>
        private SocketState _socketState;

        /// <summary>
        /// Creates a Network Controller. All the parameters are delegates which will be called based on events that occur
        /// in this controller. These delegates are all going to affect the GUI somehow.
        /// </summary>
        /// <param name="ErrorCallback"> Whenever an error happens, this will be used to show the user a message describing the problem </param>
        /// <param name="SuccessfulConnection"> When a successful connection happens, we will notify the user that they were able to connect </param>
        /// <param name="PopulateDocuments">triggers populating dropdown with options of documents </param>
        public NetworkController(Action<string> ErrorCallback, Action<string> SuccessfulConnection, Action<string[]> PopulateDocuments)
        {
            this.ErrorCallback = ErrorCallback;
            this.SuccessfulConnection = SuccessfulConnection;
            this.PopulateDocuments = PopulateDocuments;
            this.backingSheet = null;
        }

        /// <summary>
        /// This attempts to connect to the server ip provided.
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
                    AbstractNetworking.Send(state, REGISTER);

                    // Wait for data.
                    AbstractNetworking.GetData(state);
                },
                reason => ErrorCallback(reason));
        }

        /// <summary>
        /// Called when data is received on the socket.
        /// </summary>
        /// <param name="data">The data that was received.</param>
        public void DataReceived(string data)
        {
            System.Diagnostics.Debug.WriteLine(data);
            // We know the first packet has been handled once the world is not null.
            if (data.Equals(DISCONNECT)) Disconnect();
            if (data.Equals(PING)) AbstractNetworking.Send(_socketState, PING_RESPONSE);
            // TODO (Karen needs to fill out PING_RESPONSE)
            if (backingSheet == null)
                ParseFirstPacket(data);
            else
            {
                if (data.StartsWith(FULL_STATE_PREFIX))
                    PopulateDocument(data, FULL_STATE_PREFIX);
                else if (data.StartsWith(CHANGE_PREFIX))
                    PopulateDocument(data, CHANGE_PREFIX);
                else if (data.StartsWith(FOCUS_PREFIX))
                    FocusUnfocus(data, UNFOCUS_PREFIX);// this needs to be changed.
            }

            // Get new data.
            // AbstractNetworking.GetData(_socketState);
        }

        private void FocusUnfocus(string data, string command)
        {
            string[] focusCell = data.Replace(command, "").TrimStart().Split(':');
            if (command.Equals(FOCUS_PREFIX))
            {
                //somthing like FocusCallback(focusCell[0], focusCell[1], userName, true);
            }
            else if (command.Equals(UNFOCUS_PREFIX))
            {
                //somthing like FocusCallback(focusCell[0], focusCell[1], userName, false);
            }
        }

        private void PopulateDocument(string data, string command)
        {
            string[] cellContents = data.Replace(command, "").TrimStart().Split('\n');
            foreach (string content in cellContents)
            {
                string[] cellValue = content.Split(':');
                backingSheet.SetContentsOfCell(cellValue[0], cellValue[1]);
            }
        }

        /// <summary>
        /// This method will be in charge of sending messages in a appropriate manner to the client.
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage(string data)
        {
            System.Diagnostics.Debug.WriteLine(data);
            if (_socketState != null)
            {
                data += ClientNetworking.END_OF_TEXT;
                ClientNetworking.Send(_socketState, data);
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
            else if (!data.StartsWith(CONNECTION_ACCEPTED_PREFIX))
            {
                Disconnect();
                ErrorCallback("The server Handshake message didn't begin with 'connect_accepted'");
            }
            else
            {
                string[] documents = data.Replace(CONNECTION_ACCEPTED_PREFIX, "").Split('\n');
                PopulateDocuments(documents);
                backingSheet = new Spreadsheet();
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
            // Send the disconnect message with the server.
            _socketState.Disconnect();
        }
    }
}
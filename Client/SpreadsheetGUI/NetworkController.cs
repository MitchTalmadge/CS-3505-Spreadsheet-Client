using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controls the interactions with the Network for the game.
    /// </summary>
    public class NetworkController
    {
        private readonly Action<string> ErrorCallback;
        private readonly Action<string> SuccessfulConnection;

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
        /// Create a new Network Controller for the class. THIS IS USED BY THE CLIENT
        /// </summary>
        public NetworkController(Action<string> ErrorCallback, Action<string> SuccessfulConnection)
        {
            this.ErrorCallback = ErrorCallback;
            this.SuccessfulConnection = SuccessfulConnection;
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
                    SuccessfulConnection("Connected to ");

                    _socketState = state;

                    // Listen for when data is received on the socket.
                    _socketState.DataReceived += DataReceived;

                    // Listen for when the socket disconnects.
                    _socketState.Disconnected += () => { Disconnected?.Invoke(); };

                    // Send the register message with the server.
                    AbstractNetworking.Send(state, "register" + (Char)3);

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
            // Get new data.
            AbstractNetworking.GetData(_socketState);
        }

        /// <summary>
        /// Disconnects from the server.
        /// This client instance should no longer be used once this method is called.
        /// </summary>
        public void Disconnect()
        {
            _socketState.Disconnect();
        }
    }
}
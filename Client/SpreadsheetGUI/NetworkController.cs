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
            ClientNetworking.ConnectToServer(server, FirstContact, ConnectFailed);

            // Set the username to be used later.
            // this.name = name + "\n";
        }

        /// <summary>
        /// This is called whenever we are not able to connect properly to a client. Our background worker is
        /// supposed to handle the exception we are going to throw.
        /// </summary>
        /// <param name="reason"></param>
        public void ConnectFailed(String reason)
        {
            ErrorCallback(reason);
        }

        /// <summary>
        /// This is part of the initial handshake. Once we hear it, we can start listening for more regular
        /// world data to come in.
        /// </summary>
        private void FirstContact(SocketState state)
        {
            // the only reason we needed a socketState is because we are changing the callback
            AbstractNetworking.GetData(state);
        }
    }
}
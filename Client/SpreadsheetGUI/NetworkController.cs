using NetworkingLibrary;
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
        /// <summary>
        /// Create a new Network Controller for the class. THIS IS USED BY THE CLIENT
        /// </summary>
        public NetworkController()
        {
        }

        /// <summary>
        /// This attempts to connect to the server ip provided.
        /// </summary>
        /// <param name="server">String ip of the server.</param>
        /// <param name="name">Name to send.</param>
        public void ConnectToServer(String server, String name = null)
        {
            // This is where we connect to the server for the first time. After the setup is done we
            // want our callback to be FirstContact.
            NetworkLibrary.ConnectToServer(FirstContact, server);

            // Set the username to be used later.
            // this.name = name + "\n";
        }

        /// <summary>
        /// This is part of the initial handshake. Once we hear it, we can start listening for more regular
        /// world data to come in.
        /// </summary>
        private void FirstContact(SocketState state)
        {
            // the only reason we needed a socketState is because we are changing the callback
            state.callMe = ReceiveStartup;
            NetworkLibrary.GetData(state);
        }

        /// <summary>
        ///
        ///
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveStartup(SocketState state)
        {
        }
    }
}
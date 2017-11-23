using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Networking;

namespace SpaceWars
{
    /// <summary>
    /// This class keeps track of a single client and handles all required communication to and from that client.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal class ClientCommunicator
    {
        /// <summary>
        /// The SpaceWars server instance.
        /// </summary>
        private readonly SpaceWarsServer _server;

        /// <summary>
        /// The client's SocketState.
        /// </summary>
        private readonly SocketState _state;

        /// <summary>
        /// This event is invoked when the client disconnects.
        /// </summary>
        public event Action Disconnected;

        /// <summary>
        /// Creates an instance from a connected client SocketState.
        /// </summary>
        /// <param name="server">The SpaceWars server instance.</param>
        /// <param name="state">The client's SocketState.</param>
        public ClientCommunicator(SpaceWarsServer server, SocketState state)
        {
            _server = server;
            _state = state;

            // Listen for socket state events.
            _state.DataReceived += OnDataReceived;
            _state.Disconnected += OnDisconnected;
        }

        /// <summary>
        /// Asynchronously begins listening for client data after sending the first packet.
        /// </summary>
        public void BeginListeningAsync()
        {
            new Thread(() =>
            {
                SendFirstPacket();
                AbstractNetworking.GetData(_state);
            }).Start();
        }

        /// <summary>
        /// Sends the first packet that a client should receive.
        /// </summary>
        private void SendFirstPacket()
        {
            //TODO: Client id should be passed into constructor, world size should be loaded in Server from xml.
            // Format: ID<newline>WorldSize<newline>
            AbstractNetworking.Send(_state, "0\n750\n");
        }

        /// <summary>
        /// Called when data is received from the client.
        /// </summary>
        /// <param name="data">The data from the client.</param>
        private void OnDataReceived(string data)
        {
            Console.Out.WriteLine("Data from client: " + data);

            AbstractNetworking.GetData(_state);
        }

        /// <summary>
        /// Called when the client disconnects.
        /// </summary>
        private void OnDisconnected()
        {
            Disconnected?.Invoke();
        }
    }
}
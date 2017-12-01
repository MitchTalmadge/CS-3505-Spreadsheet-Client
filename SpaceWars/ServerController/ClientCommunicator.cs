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
        /// The current client's ship.
        /// Will be null until the nickname packet is recieved. 
        /// </summary>
        public Ship PlayerShip { get; private set; }

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
            
            // Listen for server events.
            _server.WorldUpdated += OnWorldUpdated;

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
            //TODO: Client id should be passed into constructor

            var packet = new StringBuilder();

            // Player ID
            packet.Append(0).Append('\n');

            // World Size
            packet.Append(_server.Configuration.WorldSize).Append('\n');

            // Send packet.
            AbstractNetworking.Send(_state, packet.ToString());
        }

        /// <summary>
        /// Called when the world is updated by the server.
        /// </summary>
        /// <param name="world">The world that was updated.</param>
        private void OnWorldUpdated(World world)
        {
            //TODO: Send a packet to the client.
        }

        /// <summary>
        /// Called when data is received from the client.
        /// </summary>
        /// <param name="data">The data from the client.</param>
        private void OnDataReceived(string data)
        {
            // Check if the player has a ship yet.
            if (PlayerShip == null)
            {
                // Nickname packet is expected.
                var nickname = data.Replace("\n", "");

                // Create a ship for the player.
                PlayerShip = new Ship(nickname);
            }

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
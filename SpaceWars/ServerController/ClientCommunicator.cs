using System;
using System.Text;
using System.Threading;
using Networking;
using Newtonsoft.Json;

namespace SpaceWars
{
    /// <summary>
    /// This class keeps track of a single client and handles all required communication to and from that client.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal class ClientCommunicator
    {
        /// <summary>
        /// This static counter ensures that all new instances have a unique id.
        /// </summary>
        private static int _idCounter;

        /// <summary>
        /// The Id of this client communicator.
        /// Used to match up communicators with ships.
        /// </summary>
        public int Id { get; } = _idCounter++;

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
        /// Determines if the first nickname packet has been received.
        /// </summary>
        private bool _nicknameReceived;

        /// <summary>
        /// Invoked when the client sends their nickname.
        /// </summary>
        public event Action<string> NicknameReceived;

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
            var packet = new StringBuilder();

            // Player ID
            packet.Append(Id).Append('\n');

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
            StringBuilder worldData = new StringBuilder();
            foreach (var ship in world.GetComponents<Ship>())
            {
                worldData.Append(JsonConvert.SerializeObject(ship)).Append("\n");
            }
            foreach (var proj in world.GetComponents<Projectile>())
            {
                worldData.Append(JsonConvert.SerializeObject(proj)).Append("\n");
            }
            foreach (var star in world.GetComponents<Star>())
            {
                worldData.Append(JsonConvert.SerializeObject(star)).Append("\n");
            }
            AbstractNetworking.Send(_state, worldData.ToString());

            //TODO: Clear commands
        }

        /// <summary>
        /// Called when data is received from the client.
        /// </summary>
        /// <param name="data">The data from the client.</param>
        private void OnDataReceived(string data)
        {
            // Check for nickname packet.
            if (!_nicknameReceived)
            {
                _nicknameReceived = true;

                // Trim newline from nickname and invoke event.
                var nickname = data.Replace("\n", "");
                NicknameReceived?.Invoke(nickname);
            }
            else
            {
                // Command packet.
                //TODO: handle commands
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
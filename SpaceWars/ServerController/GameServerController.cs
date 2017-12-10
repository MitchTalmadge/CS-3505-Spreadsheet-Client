using System;
using System.Collections.Generic;
using Networking;
using System.Collections.Concurrent;

namespace SpaceWars
{
    /// <summary>
    /// The main controller for the SpaceWars game server.
    /// Uses the networking library to maintain a connection with multiple clients
    /// and update the world state, notifying clients of changes in state.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public partial class GameServerController
    {
        /// <summary>
        /// The configuration for this server.
        /// </summary>
        internal GameServerConfiguration Configuration { get; }

        /// <summary>
        /// The TcpState that the server is using to accept client connections.
        /// </summary>
        private TcpState _tcpState;

        /// <summary>
        /// A list of connected client communicators.
        /// </summary>
        private readonly IDictionary<int, ClientCommunicator> _clients = new ConcurrentDictionary<int, ClientCommunicator>();

        /// <summary>
        /// Called when a client connects to the server.
        /// Useful for logging purposes.
        /// </summary>
        public event Action ClientConnected;

        /// <summary>
        /// Called when a client fails to connect to the server.
        /// Useful for logging purposes.
        /// </summary>
        public event Action ClientConnectFailed;

        /// <summary>
        /// Called when a client disconnects from the server.
        /// Useful for logging purposes.
        /// </summary>
        public event Action ClientDisconnected;

        /// <summary>
        /// Called when this server stops listening for clients.
        /// Useful for logging purposes.
        /// </summary>
        public event Action ServerDisconnected;

        /// <summary>
        /// Creates a new game server controller that will listen for clients.
        /// </summary>
        public GameServerController(GameServerConfiguration configuration)
        {
            Configuration = configuration;
            AcceptConnectionsAsync();
            StartGameLoopAsync();
        }

        /// <summary>
        /// Starts the process of accepting client connections in a separate thread.
        /// </summary>
        private void AcceptConnectionsAsync()
        {
            _tcpState = ServerNetworking.AwaitClientConnections(11000, ClientConnectionEstablished, ClientConnectionFailed);
        }

        /// <summary>
        /// Called when a client establishes a connection with the server.
        /// </summary>
        /// <param name="state">The client's socket state.</param>
        private void ClientConnectionEstablished(SocketState state)
        {
            // Add a new client communicator.
            var communicator = new ClientCommunicator(this, state);
            _clients.Add(communicator.Id, communicator);

            // Create a ship when the client sends their nickname.
            communicator.NicknameReceived += nickname =>
            {
                var ship = new Ship(communicator.Id, nickname);
                _world.PutComponent(ship);
            };

            // Handle the case where the client disconnects.
            communicator.Disconnected += () =>
            {
                // Remove the client's ship.
                _world.GetComponent<Ship>(communicator.Id).Health = 0;

                // Remove the client communicator.
                _clients.Remove(communicator.Id);

                // Notify listeners.
                ClientDisconnected?.Invoke();
            };

            // Start the listening process.
            communicator.BeginListeningAsync();

            // Notify listeners of a newly connected client.
            ClientConnected?.Invoke();
        }

        /// <summary>
        /// Called when the server cannot connect to a client.
        /// </summary>
        /// <param name="reason">The reason that the connection failed.</param>
        private void ClientConnectionFailed(string reason)
        {
            ClientConnectFailed?.Invoke();
        }

        /// <summary>
        /// Disconnects from the TcpState that accepts client connections.
        /// This server instance may not be used after calling this method.
        /// </summary>
        public void Disconnect()
        {
            _tcpState?.StopAcceptingClientConnections();
            StopGameLoop();
            ServerDisconnected?.Invoke();
        }
    }
}
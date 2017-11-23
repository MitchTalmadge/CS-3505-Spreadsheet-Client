using System;
using System.Collections.Generic;
using Networking;

namespace SpaceWars
{
    /// <summary>
    /// The main controller for the SpaceWars game server.
    /// Uses the networking library to maintain a connection with multiple clients
    /// and update the world state, notifying clients of changes in state.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class SpaceWarsServer
    {
        /// <summary>
        /// The TcpState that the server is using to accept client connections.
        /// </summary>
        private TcpState _tcpState;

        /// <summary>
        /// A list of connected client communicators.
        /// </summary>
        private readonly HashSet<ClientCommunicator> _clients = new HashSet<ClientCommunicator>();

        /// <summary>
        /// Called when a client connects to the server.
        /// Useful for logging purposes.
        /// </summary>
        public event Action ClientConnected;

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

        public SpaceWarsServer()
        {
            BeginAcceptingConnections();
        }

        /// <summary>
        /// Starts the process of accepting client connections.
        /// </summary>
        private void BeginAcceptingConnections()
        {
            _tcpState = ServerNetworking.AwaitClientConnections(ClientConnectionEstablished, ClientConnectionFailed);
        }

        private void ClientConnectionEstablished(SocketState state)
        {
            // Add the client to the list of connected clients.
            var communicator = new ClientCommunicator(this, state);
            _clients.Add(communicator);

            // Handle the case where the client disconnects.
            communicator.Disconnected += () =>
            {
                // Remove the client from the list of connected clients.
                _clients.Remove(communicator);

                // Notify listeners.
                ClientDisconnected?.Invoke();
            };

            // Start the listening process.
            communicator.BeginListeningAsync();

            // Notify listeners of a newly connected client.
            ClientConnected?.Invoke();
        }

        private void ClientConnectionFailed(string reason)
        {
            Console.Out.WriteLine("Connection Failed: " + reason);
        }

        /// <summary>
        /// Disconnects from the TcpState that accepts client connections.
        /// This server instance may not be used after calling this method.
        /// </summary>
        public void Disconnect()
        {
            _tcpState?.StopAcceptingClientConnections();
            ServerDisconnected?.Invoke();
        }
    }
}
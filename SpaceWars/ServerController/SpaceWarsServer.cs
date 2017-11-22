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
        /// A list of socket states for connected clients.
        /// </summary>
        private HashSet<SocketState> _clients = new HashSet<SocketState>();

        public SpaceWarsServer()
        {
            BeginAcceptingConnections();
        }

        /// <summary>
        /// Starts the process of accepting client connections.
        /// </summary>
        private void BeginAcceptingConnections()
        {
            _tcpState = ServerNetworking.AwaitClientConnections(ClientConnectionEstablished, ClientConnectionFailed,
                DataReceived);
        }

        private void ClientConnectionEstablished(SocketState state)
        {
            Console.Out.WriteLine("Connection from client.");
        }

        private void ClientConnectionFailed(string reason)
        {
            Console.Out.WriteLine("Connection Failed: " + reason);
        }

        private void DataReceived(string data)
        {
            Console.Out.WriteLine("Data from client: " + data);
        }

        /// <summary>
        /// Disconnects from the TcpState that accepts client connections.
        /// This server instance may not be used after calling this method.
        /// </summary>
        private void Disconnect()
        {
            _tcpState?.StopAcceptingClientConnections();
        }
    }
}
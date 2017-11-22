using System;
using System.Collections.Generic;
using System.Threading;
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
        private readonly HashSet<SocketState> _clients = new HashSet<SocketState>();

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
            _tcpState = ServerNetworking.AwaitClientConnections(ClientConnectionEstablished, ClientConnectionFailed,
                DataReceived);
        }

        private void ClientConnectionEstablished(SocketState state)
        {
            // Add the client to the list of connected clients.
            _clients.Add(state);

            // Spawn a thread to communicate with the client.
            new Thread(() =>
            {
                SendFirstPacket(state);
                AbstractNetworking.GetData(state);
            }).Start();

            // Notify listeners of a newly connected client.
            ClientConnected?.Invoke();
        }

        /// <summary>
        /// Sends the first packet that a client should receive.
        /// </summary>
        /// <param name="state">The client's socket state.</param>
        private void SendFirstPacket(SocketState state)
        {
            //TODO: Get World Size from xml, compute an id.
            AbstractNetworking.Send(state, "0\n750\n");
        }

        private void ClientConnectionFailed(string reason)
        {
            Console.Out.WriteLine("Connection Failed: " + reason);
        }

        private void DataReceived(string data)
        {
            // When data is null, the connection has been lost.
            if (data == null)
            {
                ClientDisconnected?.Invoke();
                return;
            }

            Console.Out.WriteLine("Data from client: " + data);
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
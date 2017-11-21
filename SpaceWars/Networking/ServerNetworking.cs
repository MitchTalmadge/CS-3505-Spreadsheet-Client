using System;
using System.Net;
using System.Net.Sockets;

namespace Networking
{
    /// <inheritdoc />
    /// <summary>
    /// Server-specific networking code, for connecting to clients. 
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class ServerNetworking : AbstractNetworking
    {
        /// <summary>
        /// Waits for a client to connect to this server.
        /// </summary>
        /// <param name="established">The callback for when a connection to a client has been established.</param>
        /// <param name="failed">The callback for when a connection to a client has failed.</param>
        /// <param name="dataReceived">The callback for when data is received from the new client.</param>
        public void AwaitClientConnection(
            ConnectionEstablished established,
            ConnectionFailed failed,
            DataReceived dataReceived)
        {
            var listener = new TcpListener(IPAddress.Loopback, 11000);
            var clientConnectionState = new ClientConnectionState(listener, established, failed, dataReceived);

            // Start accepting the socket from the client.
            try
            {
                listener.BeginAcceptSocket(AcceptClientSocket, clientConnectionState);
            }
            catch (Exception e)
            {
                failed(e.Message);
            }
        }

        /// <summary>
        /// Called when a client has connected with a socket.
        /// </summary>
        /// <param name="asyncResult">Contains the client connection state.</param>
        private static void AcceptClientSocket(IAsyncResult asyncResult)
        {
            var clientConnectionState = (ClientConnectionState) asyncResult.AsyncState;
            var socket = clientConnectionState.TcpListener.AcceptSocket();

            // Create a socket state from the connection.
            var socketState = new SocketState(
                socket, 
                clientConnectionState.ConnectionEstablished,
                clientConnectionState.ConnectionFailed,
                clientConnectionState.DataReceived);

            // Notify callback of connection established.
            socketState.ConnectionEstablished(socketState);
        }

        /// <summary>
        /// Contains the state of a connection-in-progress from a client,
        /// including callbacks for the server and the TCP Listener being used to connect.
        /// </summary>
        /// <authors>Jiahui Chen, Mitch Talmadge</authors>
        private class ClientConnectionState
        {
            /// <summary>
            /// The TCP Listener that is being used to connect to the client.
            /// </summary>
            public TcpListener TcpListener { get; }

            /// <summary>
            /// The connection established callback.
            /// </summary>
            public ConnectionEstablished ConnectionEstablished { get; }

            /// <summary>
            /// The connection failed callback.
            /// </summary>
            public ConnectionFailed ConnectionFailed { get; }

            /// <summary>
            /// The data received callback.
            /// </summary>
            public DataReceived DataReceived { get; }

            /// <summary>
            /// Creates a client connection state with the given TCP Listener and callbacks.
            /// </summary>
            /// <param name="tcpListener">The TCP Listener that is being used to connect to the client.</param>
            /// <param name="established">The connection established callback.</param>
            /// <param name="failed">The connection failed callback.</param>
            /// <param name="dataReceived">The data received callback.</param>
            public ClientConnectionState(
                TcpListener tcpListener,
                ConnectionEstablished established,
                ConnectionFailed failed,
                DataReceived dataReceived)
            {
                TcpListener = tcpListener;
                ConnectionEstablished = established;
                ConnectionFailed = failed;
                DataReceived = dataReceived;
            }
        }
    }
}
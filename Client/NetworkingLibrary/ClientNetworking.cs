using System;
using System.Net;
using System.Net.Sockets;

namespace Networking
{
    /// <inheritdoc />
    /// <summary>
    /// Client-specific networking code, for connecting to servers.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class ClientNetworking : AbstractNetworking
    {
        public static int DEFAULT_PORT { get; private set; } = 2112;

        /// <summary>
        /// Attempts to connect to the server via a provided hostname.
        /// Saves the callback function in a socket state object for use when data arrives.
        /// </summary>
        /// <param name="hostName">The address to connect to.</param>
        /// <param name="port">The port to connect to.</param>
        /// <param name="established">The callback for when a connection has been established.</param>
        /// <param name="failed">The callback for when a connection has failed.</param>
        public static void ConnectToServer(string hostName, ConnectionEstablished established, ConnectionFailed failed)
        {
            // Create a SocketState.
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var socketState = new SocketState(socket, established, failed);

            // Attempt connection to the address on the default port.
            try
            {
                socket.BeginConnect(hostName, DEFAULT_PORT, ConnectedToServer, socketState);
            }
            catch (Exception e)
            {
                socketState.ConnectionFailed(e.Message);
            }
        }

        /// <summary>
        /// Referenced by the BeginConnect method and is called by the OS
        /// when the socket connects to the server.
        /// </summary>
        /// <param name="asyncResult">Contains the state.</param>
        private static void ConnectedToServer(IAsyncResult asyncResult)
        {
            // Retreive the SocketState from the async result.
            var socketState = (SocketState)asyncResult.AsyncState;
            var socket = socketState.Socket;

            // Attempt to end the connection request.
            try
            {
                socket.EndConnect(asyncResult);
                socketState.ConnectionEstablished(socketState);
            }
            catch (Exception e)
            {
                socketState.ConnectionFailed(e.Message);
            }
        }
    }
}
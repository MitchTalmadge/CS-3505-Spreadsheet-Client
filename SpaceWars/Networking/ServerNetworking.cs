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
        /// Starts an internal loop that will continually accept connections from clients until stopped via the returned TcpState.
        /// </summary>
        /// <param name="port">The port to listen on.</param>
        /// <param name="established">The callback for when a connection to a client has been established.</param>
        /// <param name="failed">The callback for when a connection to a client has failed.</param>
        /// <returns>A TcpState, which can be used to stop accepting connections from clients.</returns>
        public static TcpState AwaitClientConnections(int port, ConnectionEstablished established, ConnectionFailed failed)
        {
            var listener = new TcpListener(IPAddress.Any, port);
            var tcpState = new TcpState(listener, established, failed);

            // Start accepting the socket from the client.
            try
            {
                listener.Start();
                listener.BeginAcceptSocket(AcceptClientSocket, tcpState);
            }
            catch (Exception e)
            {
                failed(e.Message);
            }

            return tcpState;
        }

        /// <summary>
        /// Called when a client has connected with a socket.
        /// </summary>
        /// <param name="asyncResult">Contains the client connection state.</param>
        private static void AcceptClientSocket(IAsyncResult asyncResult)
        {
            var tcpState = (TcpState) asyncResult.AsyncState;

            var socket = tcpState.TcpListener.EndAcceptSocket(asyncResult);

            // Create a socket state from the connection.
            var socketState = new SocketState(socket, tcpState.ConnectionEstablished, tcpState.ConnectionFailed);

            // Notify callback of connection established.
            socketState.ConnectionEstablished(socketState);

            // Exit immediately if we are not supposed to accept any more connections.
            if (tcpState.Stopped)
                return;

            // Accept another connection.
            try
            {
                tcpState.TcpListener.BeginAcceptSocket(AcceptClientSocket, tcpState);
            }
            catch (Exception e)
            {
                tcpState.ConnectionFailed(e.Message);
            }
        }
    }
}
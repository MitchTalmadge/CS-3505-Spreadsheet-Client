using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    /// <summary>
    /// Contains a TcpListener instance that is used to accept sockets from clients.
    /// Includes connection and data callbacks, which are passed to a SocketState once 
    /// a socket has been accepted.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class TcpState
    {
        /// <summary>
        /// The TCP Listener that is being used to connect to the client.
        /// </summary>
        internal TcpListener TcpListener { get; }

        /// <summary>
        /// Determines if this TcpState has been stopped.
        /// If true, no more connections should be accepted.
        /// </summary>
        internal bool Stopped { get; private set; }

        /// <summary>
        /// The connection established callback.
        /// </summary>
        internal AbstractNetworking.ConnectionEstablished ConnectionEstablished { get; }

        /// <summary>
        /// The connection failed callback.
        /// </summary>
        internal AbstractNetworking.ConnectionFailed ConnectionFailed { get; }

        /// <summary>
        /// The data received callback.
        /// </summary>
        internal AbstractNetworking.DataReceived DataReceived { get; }

        /// <summary>
        /// Creates a client connection state with the given TCP Listener and callbacks.
        /// </summary>
        /// <param name="tcpListener">The TCP Listener that is being used to connect to the client.</param>
        /// <param name="established">The connection established callback.</param>
        /// <param name="failed">The connection failed callback.</param>
        /// <param name="dataReceived">The data received callback.</param>
        internal TcpState(
            TcpListener tcpListener,
            AbstractNetworking.ConnectionEstablished established,
            AbstractNetworking.ConnectionFailed failed,
            AbstractNetworking.DataReceived dataReceived)
        {
            TcpListener = tcpListener;
            ConnectionEstablished = established;
            ConnectionFailed = failed;
            DataReceived = dataReceived;
        }

        /// <summary>
        /// Stops accepting client connections on this TcpState.
        /// This instance should be discarded after calling this method.
        /// </summary>
        public void StopAcceptingClientConnections()
        {
            try
            {
                Stopped = true;
                TcpListener.Stop();
            }
            catch (SocketException e)
            {
                ConnectionFailed(e.Message);
            }
        }
    }
}
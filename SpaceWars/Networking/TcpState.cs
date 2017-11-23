using System.Net.Sockets;

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
        /// Creates a client connection state with the given TCP Listener and callbacks.
        /// </summary>
        /// <param name="tcpListener">The TCP Listener that is being used to connect to the client.</param>
        /// <param name="established">The connection established callback.</param>
        /// <param name="failed">The connection failed callback.</param>
        internal TcpState(
            TcpListener tcpListener,
            AbstractNetworking.ConnectionEstablished established,
            AbstractNetworking.ConnectionFailed failed)
        {
            TcpListener = tcpListener;
            ConnectionEstablished = established;
            ConnectionFailed = failed;
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
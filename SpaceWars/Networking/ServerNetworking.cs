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
        public void AwaitClientConnection(
            ConnectionEstablished established,
            ConnectionFailed failed)
        {
            //TODO: TcpListener BeginAcceptSocket
        }

    }
}

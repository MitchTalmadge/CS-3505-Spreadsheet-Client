using System.Net.Sockets;
using System.Text;

namespace Networking
{
    /// <summary>
    /// Holds information about how data is handled when it reaches
    /// a Socket.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class SocketState
    {
        /// <summary>
        /// Socket that receives data
        /// </summary>
        internal Socket Socket { get; }

        /// <summary>
        /// This is the buffer where we will receive data from the socket
        /// </summary>
        internal byte[] DataBuffer = new byte[1000];

        /// <summary>
        /// Function that processes data once it's received. 
        /// </summary>
        internal Networking.HandleData HandleData { get; }

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        // holds pervious data 
        internal StringBuilder DataStringBuilder = new StringBuilder();

        //ID, not used for PS7
        int ID;

        internal SocketState(Socket socket, Networking.HandleData callbackFunction)
        {
            Socket = socket;
            HandleData = callbackFunction;
        }

        /// <returns>The currently held data from the latest response from the server.</returns>
        public string GetData()
        {
            return DataStringBuilder.ToString();
        }
    }
}
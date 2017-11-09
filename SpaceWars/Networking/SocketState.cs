using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    /// <summary>
    /// Holds information about how data is handled when it reaches
    /// a Socket.
    /// </summary>
    public class SocketState
    {
        /// <summary>
        /// Socket that receives data
        /// </summary>
        internal Socket socket { get; }

        /// <summary>
        /// This is the buffer where we will receive data from the socket
        /// </summary>
        internal byte[] dataBuffer = new byte[1000];

        /// <summary>
        /// Function that processes data once it's received. 
        /// </summary>
        internal Networking.HandleData handleData { get; }

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        internal StringBuilder sb = new StringBuilder();

        internal SocketState(Socket socket, String prevData, Networking.HandleData callbackFunction)
        {
            this.socket = socket;
            this.handleData = callbackFunction;
        }
    }
}

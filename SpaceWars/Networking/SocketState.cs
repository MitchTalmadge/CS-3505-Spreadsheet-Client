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
    internal class SocketState
    {
        internal Socket socket { get; }

        internal Networking.Callback callbackFunction { get; }

        internal SocketState(Socket socket, String prevData, Networking.Callback callbackFunction)
        {
            this.socket = socket;
            this.callbackFunction = callbackFunction;
        }
    }
}

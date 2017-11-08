using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    /// <summary>
    /// Static class holding static networking methods.
    /// </summary>
    public static class Networking
    {
        public delegate void Callback(byte[] data);

        /// <summary>
        /// Attempts to connect to the server via a provided hostname. 
        /// Saves the callback function in a socket state object for use when data arrives.
        /// </summary>
        /// <param name="callbackFunction"></param>
        /// <param name="hostname"></param>
        /// <returns></returns>
        public static Socket ConnectToServer(Callback callbackFunction, string hostname)
        {
            //parsing host address and creating corresponding socket
            IPAddress address = IPAddress.Parse(hostname);
            Socket socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SocketState socketState = new SocketState(socket, null, callbackFunction);

            //creating connection and passing SocketState object 
            socket.BeginConnect(address, 11000, ConnectedToServer, socketState);
            return socket;
        }

        /// <summary>
        ///  Referenced by the BeginConnect method and is called by the OS
        ///  when the socket connects to the server.
        /// </summary>
        /// <param name="stateAsArObject"></param>
        public static void ConnectedToServer(IAsyncResult stateAsArObject)
        {
            //stateAsArObject contains a field AsyncState which contains a SocketState object
            SocketState socketState = (SocketState)stateAsArObject.AsyncState;
            socketState.socket.EndConnect(stateAsArObject);
            socketState.callbackFunction(new byte[0]);
            socketState.cal
        }

        public static void GetData(SocketState state)
        {

        }

        public static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
            return;
        }

        public static void Send(Socket socket, String data)
        {

        }

        public static void SendCallback(IAsyncResult ar)
        {

        }
    }
}

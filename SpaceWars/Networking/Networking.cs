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
        public delegate void HandleData(byte[] data);

        /// <summary>
        /// Attempts to connect to the server via a provided hostname. 
        /// Saves the callback function in a socket state object for use when data arrives.
        /// </summary>
        /// <param name="callbackFunction"></param>
        /// <param name="hostname"></param>
        /// <returns></returns>
        public static Socket ConnectToServer(HandleData callbackFunction, string hostname)
        {
            //parsing host address and creating corresponding socket and SocketState
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
            Socket socket = socketState.socket;
            socket.EndConnect(stateAsArObject);

            // Wait for data to come from the server
            // We pass the last argument (state) so that the callback knows which connection data was received on
            socket.BeginReceive(socketState.dataBuffer, 0, socketState.dataBuffer.Length, SocketFlags.None, ReceiveCallback, socketState);
            socketState.handleData(socketState.dataBuffer);///???
        }

        public static void GetData(SocketState state)
        {

        }

        /// <summary>
        ///  Called by the OS when new data arrives. This method should check to see how much data has arrived.
        ///  If 0, the connection has been closed (presumably by the server). On greater than zero data, 
        ///  this method should get the SocketState object out of the IAsyncResult (just as above in 2), 
        ///  and call the callMe provided in the SocketState.
        /// </summary>
        /// <param name="stateAsArObject"></param>
        public static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
            // Get the SocketState associated with the received data
            SocketState state = (SocketState)stateAsArObject.AsyncState;

            int numBytes = state.socket.EndReceive(stateAsArObject);

            if (numBytes > 0)
            {
                // Convert the raw bytes to a string
                string data = Encoding.UTF8.GetString(state.dataBuffer, 0, numBytes);

                // Append the data to a growable buffer.
                // We don't know how much data arrived, or if we have an incomplete message.
                state.sb.Append(data);


                // calling delegate on data???
                state.handleData(state.dataBuffer);

                // Wait for more data from the server. This creates an "event loop".
                // ReceiveCallback will be invoked every time new data is available on the socket.
                state.socket.BeginReceive(state.dataBuffer, 0, state.dataBuffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
        }

        public static void Send(Socket socket, String data)
        {

        }

        public static void SendCallback(IAsyncResult ar)
        {

        }
    }
}

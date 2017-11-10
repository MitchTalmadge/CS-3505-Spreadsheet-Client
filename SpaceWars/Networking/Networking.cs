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
        //void return takes in socket state 
        public delegate void HandleData(SocketState state);

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
            SocketState socketState = new SocketState(socket, callbackFunction);

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

            //calls delegate which will usually call GetData
            socketState.handleData(socketState);
        }

        /// <summary>
        /// Is called in delegate (which is passed in/called within the Client). 
        /// Wrapper for BeginReceive, called by client since the client decides if it wants data. 
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState state)
        {
            state.socket.BeginReceive(state.dataBuffer, 0, state.dataBuffer.Length, SocketFlags.None, ReceiveCallback, state);
        }

        /// <summary>
        ///  Called by the OS when new data arrives. Checks to see how much data has arrived.
        ///  If 0, the connection has been closed (presumably by the server). On greater than zero data, 
        ///  this method gets the SocketState object out of the IAsyncResult, 
        ///  and calls the delegate provided in the SocketState.
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

                // calling delegate on socket state containing data
                state.handleData(state);

                // Wait for more data from the server. This creates an "event loop".
                // ReceiveCallback will be invoked every time new data is available on the socket.
                state.socket.BeginReceive(state.dataBuffer, 0, state.dataBuffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
        }

        /// <summary>
        /// Allows data to be sent over a Socket. Converts data into bytes and 
        /// then sends using BeginSend. 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static void Send(Socket socket, String data)
        {
            Byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            socket.BeginSend(dataBytes, 0, 1000, SocketFlags.None, SendCallback, socket);
        }

        /// <summary>
        /// Assists the Send function. It should extract the Socket out of
        /// the IAsyncResult, and then call socket.EndSend
        /// </summary>
        /// <param name="ar"></param>
        public static void SendCallback(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;

            state.socket.EndSend(ar);
        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Networking
{
    /// <summary>
    /// Static class holding static networking methods.
    /// </summary>
    public static class Networking
    {
        /// <summary>
        /// This delegate is called when data is received.
        /// </summary>
        /// <param name="data">The data that was received.</param>
        public delegate void DataReceived(string data);

        /// <summary>
        /// This delegate is called when a connection has been established.
        /// </summary>
        /// <param name="state">The socket state, which contains information about the connection and should be stored for use when sending data.</param>
        public delegate void ConnectionEstablished(SocketState state);

        /// <summary>
        /// This delegate is called when a connection has failed.
        /// </summary>
        /// <param name="reason">The reason for the failed connection.</param>
        public delegate void ConnectionFailed(string reason);

        /// <summary>
        /// Attempts to connect to the server via a provided hostname. 
        /// Saves the callback function in a socket state object for use when data arrives.
        /// </summary>
        /// <param name="hostName">The address to connect to, excluding port.</param>
        /// <param name="established">The callback for when a connection has been established.</param>
        /// <param name="failed">The callback for when a connection has failed.</param>
        /// <param name="dataReceived">The callback for when data is received.</param>
        public static void ConnectToServer(string hostName, ConnectionEstablished established, ConnectionFailed failed,
            DataReceived dataReceived)
        {
            // Parse the address into an IPAddress and create a SocketState.
            var address = Dns.GetHostAddresses(hostName)[0];
            var socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var socketState = new SocketState(socket, established, failed, dataReceived);

            // Attempt connection to the address on the default port.
            try
            {
                socket.BeginConnect(address, 11000, ConnectedToServer, socketState);
            }
            catch (Exception e)
            {
                socketState.Failed(e.Message);
            }
        }

        /// <summary>
        ///  Referenced by the BeginConnect method and is called by the OS
        ///  when the socket connects to the server.
        /// </summary>
        /// <param name="stateAsArObject"></param>
        private static void ConnectedToServer(IAsyncResult stateAsArObject)
        {
            // Retreive the SocketState from the async result.
            var socketState = (SocketState) stateAsArObject.AsyncState;
            var socket = socketState.Socket;

            // Attempt to close the connection.
            try
            {
                socket.EndConnect(stateAsArObject);
                socketState.Established(socketState);
            }
            catch (Exception e)
            {
                socketState.Failed(e.Message);
            }
        }

        /// <summary>
        /// Disconnects the given state from the server.
        /// No messages should be sent after this method is called.
        /// </summary>
        /// <param name="state">The socket state to disconnect</param>
        public static void DisconnectFromServer(SocketState state)
        {
            state.Socket.Disconnect(false);
        }

        /// <summary>
        /// Is called in delegate (which is passed in/called within the Client). 
        /// Wrapper for BeginReceive, called by client since the client decides if it wants data. 
        /// </summary>
        /// <param name="state">The state containing connection information.</param>
        public static void GetData(SocketState state)
        {
            state.Socket.BeginReceive(state.DataBuffer, 0, state.DataBuffer.Length, SocketFlags.None, ReceiveCallback,
                state);
        }

        /// <summary>
        ///  Called by the OS when new data arrives. Checks to see how much data has arrived.
        ///  If 0, the connection has been closed (presumably by the server). On greater than zero data, 
        ///  this method gets the SocketState object out of the IAsyncResult, 
        ///  and calls the delegate provided in the SocketState.
        /// </summary>
        /// <param name="stateAsArObject"></param>
        private static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
            // Get the SocketState associated with the received data
            var state = (SocketState) stateAsArObject.AsyncState;

            var numBytes = state.Socket.EndReceive(stateAsArObject);

            // Account for no data being received (connection closed).
            if (numBytes <= 0)
                return;

            // Convert the raw bytes to a string
            var data = Encoding.UTF8.GetString(state.DataBuffer, 0, numBytes);

            // Append the data to a growable buffer.
            // We don't know how much data arrived, or if we have an incomplete message.
            state.DataStringBuilder.Append(data);

            // calling delegate on socket state containing data
            // TODO: This probably shouldn't be called here.
            state.DataReceived(state.DataStringBuilder.ToString());

            // Wait for more data from the server. This creates an "event loop".
            // ReceiveCallback will be invoked every time new data is available on the socket.
            state.Socket.BeginReceive(state.DataBuffer, 0, state.DataBuffer.Length, SocketFlags.None,
                ReceiveCallback, state);
        }

        /// <summary>
        /// Allows data to be sent over a Socket. 
        /// Converts data into bytes and then sends using BeginSend. 
        /// </summary>
        /// <param name="state">The state containing connection information.</param>
        /// <param name="data">The data to send.</param>
        public static void Send(SocketState state, string data)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);

            state.Socket.BeginSend(dataBytes, 0, 1000, SocketFlags.None, SendCallback, state);
        }

        /// <summary>
        /// Assists the Send function. It should extract the Socket out of
        /// the IAsyncResult, and then call socket.EndSend
        /// </summary>
        private static void SendCallback(IAsyncResult ar)
        {
            var state = (SocketState) ar.AsyncState;

            state.Socket.EndSend(ar);
        }
    }
}
using System;
using System.Net.Sockets;
using System.Text;

namespace Networking
{
    /// <summary>
    /// Networking code that is used by both clients and servers.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public abstract class AbstractNetworking
    {
        /// <summary>
        /// This delegate is called when a connection has been established, whether to a client or a server.
        /// </summary>
        /// <param name="state">The socket state, which contains information about the connection and should be stored for use when sending data.</param>
        public delegate void ConnectionEstablished(SocketState state);

        /// <summary>
        /// This delegate is called when a connection has failed.
        /// </summary>
        /// <param name="reason">The reason for the failed connection.</param>
        public delegate void ConnectionFailed(string reason);

        /// <summary>
        /// Is called in delegate (which is passed in/called within the Client). 
        /// Wrapper for BeginReceive, called by client since the client decides if it wants data. 
        /// </summary>
        /// <param name="state">The state containing connection information.</param>
        public static void GetData(SocketState state)
        {
            try
            {
                state.Socket.BeginReceive(state.DataBuffer, 0, state.DataBuffer.Length, SocketFlags.None,
                    ReceiveCallback,
                    state);
            }
            catch (SocketException)
            {
                state.Disconnect();
            }
            catch (ObjectDisposedException)
            {
                // The socket was disposed previously.
                // Ignore.
            }
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

            int numBytes;

            // Try to read the received data.
            try
            {
                numBytes = state.Socket.EndReceive(stateAsArObject);
            }
            catch (SocketException)
            {
                // The other side has disconnected.
                state.Disconnect();
                return;
            }
            catch (ObjectDisposedException)
            {
                // The socket was disposed previously.
                state.Disconnect();
                return;
            }

            // Account for no data being received (connection closed).
            if (numBytes <= 0)
            {
                state.Disconnect();
                return;
            }

            // Convert the raw bytes to a string
            var data = Encoding.UTF8.GetString(state.DataBuffer, 0, numBytes);
            state.DataStringBuilder.Append(data);

            // Check for newline terminator.
            if (!data.EndsWith("\n"))
            {
                // No newline, so there must be more data.
                GetData(state);
                return;
            }

            // Don't do anything if the socket is disconnected (prevents race condition).
            if (!state.Socket.Connected)
                return;

            // Notify the callback.
            state.FinishReceiveData();
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

            try
            {
                state.Socket.BeginSend(dataBytes, 0, dataBytes.Length, SocketFlags.None, SendCallback, state);
            }
            catch (SocketException)
            {
                // The other side has disconnected.
                state.Disconnect();
            }
            catch (ObjectDisposedException)
            {
                // The socket was disposed previously.
                state.Disconnect();
            }
        }

        /// <summary>
        /// Assists the Send function. It should extract the Socket out of
        /// the IAsyncResult, and then call socket.EndSend
        /// </summary>
        private static void SendCallback(IAsyncResult ar)
        {
            var state = (SocketState) ar.AsyncState;

            try
            {
                state.Socket.EndSend(ar);
            }
            catch (SocketException)
            {
                // The other side has disconnected.
                state.Disconnect();
            }
            catch (ObjectDisposedException)
            {
                // The socket was disposed previously.
                state.Disconnect();
            }
        }
    }
}
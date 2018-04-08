using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkingLibrary
{
    public delegate void NetworkCallback(SocketState state);

    public static class NetworkLibrary
    {
        public static int DEFAULT_PORT { get; private set; } = 2112;

        /// <summary>
        /// This function is "called" by the operating system when the remote site acknowledges connect request
        /// the endConnect method finished the connection status and then gets ready to call the callback method sent from form1
        /// </summary>
        /// <param name="ar"></param>
        public static void ConnectedCallback(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;

            try
            {
                // Complete the connection.
                ss.theSocket.EndConnect(ar);
            }
            catch (Exception)
            {
                // If something went wrong connecting, notify the callback that this happened.
                ss.connected = false;
            }
            // The callMe is the delegate method that has been passed around a few times until now after this connection
            //the callback function is called
            ss.callMe(ss);
        }

        /// <summary>
        /// This is the first networking method form1 is going to call. This method creates a new socket
        /// and then begins connecting, and sends ConnectedCallback method as the callback to execute
        /// </summary>
        /// <param name="callMe"></param>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static Socket ConnectToServer(NetworkCallback callMe, string hostName)
        {
            //This is the first method that gets called. We create the MakeSocket for the first time here
            SocketState state;

            System.Diagnostics.Debug.WriteLine("Connecting to " + hostName);
            // Create a TCP/IP socket
            System.Diagnostics.Debug.WriteLine("Connection with the indicated server was: "
               + MakeSocket(hostName, out Socket socket, out IPAddress ipAddress)
            );
            state = new SocketState(socket, callMe);
            // Once we make the socket we pass around the socketState in order to fill in the data for
            // receiving data
            state.theSocket.BeginConnect(ipAddress, DEFAULT_PORT, ConnectedCallback, state);
            return socket;
        }

        /// <summary>
        /// This will be called everytime data needs to be updated. ReceiveCallback is called after this, and information
        /// is encoded into the byteBuffer provided by the socketstate class
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState state)
        {
            // Start an event loop to receive data from the server.
            try
            {
                state.theSocket.BeginReceive(state.byteBuffer, 0, state.byteBuffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
            catch (Exception)
            {
                // If something goes wrong, tell the client so they can handle it.
                state.connected = false;
                state.callMe(state);
            }
        }

        /// <summary>
        /// Called right after the getting data is finished. This will be responsible for converting the byteBuffer data into a string
        /// to add to our buffer
        /// </summary>
        /// <param name="ar"></param>
        public static void ReceiveCallback(IAsyncResult ar)
        {
            // the data will already be placed into the bytebuffer because of the BeginReceive call above
            SocketState state = (SocketState)ar.AsyncState;

            int bytesRead = 0;
            try
            {
                bytesRead = state.theSocket.EndReceive(ar);
            }
            catch (Exception)
            {
                // If something goes wrong here, let the client know.
                state.connected = false;
                state.callMe(state);
            }

            if (bytesRead > 0)
            {
                // If we got data, decode it w/ UTF8 and add it to the state.
                String message = Encoding.UTF8.GetString(state.byteBuffer, 0, bytesRead);
                state.objectMessage.Append(message);
                state.callMe(state);
            }
        }

        /// <summary>
        /// Send the provided data to the socket.
        /// </summary>
        /// <param name="ss">Socket to send data to.</param>
        /// <param name="data">Data to send.</param>
        public static void Send(Socket ss, String data, bool closed)
        {
            byte[] sendData = Encoding.UTF8.GetBytes(data);
            try
            {
                ss.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, ss);
            }
            catch (Exception) { }
            if (closed)
            {
                ss.Close();
            }
        }

        /// <summary>
        /// Handle callback from a data send. In our case, we simply close the connection.
        /// </summary>
        public static void SendCallback(IAsyncResult ar)
        {
            Socket ss = (Socket)ar.AsyncState;
            try
            {
                ss.EndSend(ar);
            }
            catch (Exception) { } // If something goes wrong, we have no way of notifying the client, so just handle it so we don't crash (chances are it'll break somewhere else too).
        }

        /// <summary>
        /// Copied code from Professor Kopta's implementation of lab FancyChatClient
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="socket"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private static bool MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
        {
            ipAddress = IPAddress.None;
            socket = null;
            try
            {
                // Endpoint for socket
                IPHostEntry ipHostInfo;
                // Checking if we are using an URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostName);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                    {
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                        }
                    }
                    // No IPV4 addresses found.
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid address: " + hostName);
                        throw new ArgumentException("Invalid address");
                    }
                }
                catch (Exception)
                {
                    // this is when we see if we need to use an ipAdress
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                // Now we are ready to create the TCP/IP socket
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
                socket.NoDelay = true;
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured " + e);
                throw new ArgumentException("Invalid address");
            }
        }
    }

    public class SocketState
    {
        // The actual socket we are connecting with.
        public readonly Socket theSocket;

        public SocketState(Socket s, NetworkCallback callMe)
        {
            this.theSocket = s;
            this.callMe = callMe;
            byteBuffer = new byte[1024];
            objectMessage = new StringBuilder();
            // True unless proven otherwise:
            this.connected = true;
        }

        // Two representations of data from the network. One simply holds the bytes, one is those bytes decoded using UTF8
        public byte[] byteBuffer { get; set; }

        // Represents callback in client for when we recieve more data.
        public NetworkCallback callMe { get; set; }

        // This represents whether our client is still connected to the server. If it's not, we need to
        // let them know and let them reconnect.
        public Boolean connected { get; set; }

        public int id { get; set; }
        public StringBuilder objectMessage { get; set; }
    }
}
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SpaceWars
{
    /// <summary>
    /// This section of the SpaceWars class handles networking.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public partial class SpaceWarsClient
    {
        /// <summary>
        /// This delegate is called when a connection to SpaceWars was established.
        /// May be called on another thread.
        /// </summary>
        /// <param name="spaceWarsClient">The connected SpaceWars client.</param>
        public delegate void ConnectionEstablished(SpaceWarsClient spaceWarsClient);

        /// <summary>
        /// The provided ConnectionEstablished delegate implementation.
        /// </summary>
        private readonly ConnectionEstablished _connectionEstablishedCallback;

        /// <summary>
        /// This delegate is called when a connection to SpaceWars failed.
        /// </summary>
        /// <param name="reason">Why the connection failed.</param>
        public delegate void ConnectionFailed(string reason);

        /// <summary>
        /// The provided ConnectionFailed delegate implementation.
        /// </summary>
        private readonly ConnectionFailed _connectionFailedCallback;

        /// <summary>
        /// This event is fired when the connection to the server has been lost, 
        /// whether unexpectedly or by calling the Disconnect method.
        /// </summary>
        public event Action Disconnected;

        /// <summary>
        /// Socket that the connection is made through.
        /// </summary>
        private SocketState _socketState;

        /// <summary>
        /// Receives boolean array of indicators of user input-movement for ships. Index 0
        /// is the forward indicator, index 1 is the right indicator, index 2 is the left 
        /// indicator, and index 3 is the firing indicator. 
        /// Ships can move forward, left, right, and fire projectiles. 
        /// Action instructions are sent to the Server through this method.
        /// </summary>
        /// <param name="indicators"></param>
        public void SendCommand(bool[] indicators)
        {
            if (!indicators[0] && !indicators[1] && !indicators[2] && !indicators[3])
                return;

            var commandBuilder = new StringBuilder("(");
            if (indicators[0])
            {
                commandBuilder.Append('T');
            }
            if (indicators[1])
            {
                commandBuilder.Append('R');
            }
            if (indicators[2])
            {
                commandBuilder.Append('L');
            }
            if (indicators[3])
            {
                commandBuilder.Append('F');
            }
            commandBuilder.Append(')');

            AbstractNetworking.Send(_socketState, commandBuilder + "\n");
        }

        /// <summary>
        /// Attempts to connect to the given hostname using the given nickname.
        /// </summary>
        /// <param name="hostname">The server address, excluding the port.</param>
        /// <param name="nickname">The nickname to use for the player connecting.</param>
        private void Connect(string hostname, string nickname)
        {
            // Connect to the server.
            ClientNetworking.ConnectToServer(
                hostname,
                state =>
                {
                    _socketState = state;

                    // Listen for when data is received on the socket.
                    _socketState.DataReceived += DataReceived;

                    // Listen for when the socket disconnects.
                    _socketState.Disconnected += () =>
                    {
                        Disconnected?.Invoke();
                    };

                    // Start the thread that continually receives data.
                    new Thread(() =>
                    {
                        // Send the nickname of the user.
                        AbstractNetworking.Send(state, nickname + '\n');

                        // Wait for data.
                        AbstractNetworking.GetData(state);
                    }).Start();
                },
                reason => _connectionFailedCallback(reason)
            );
        }

        /// <summary>
        /// Disconnects from the server. 
        /// This game instance should no longer be used once this method is called.
        /// </summary>
        public void Disconnect()
        {
            _socketState.Disconnect();
        }

        /// <summary>
        /// Called when data is received on the socket.
        /// </summary>
        /// <param name="data">The data that was received.</param>
        public void DataReceived(string data)
        {
            // We know the first packet has been handled once the world is not null.
            if (GameWorld == null)
                ParseFirstPacket(data);
            else
                ParseJsonPacket(data);

            // Get new data.
            AbstractNetworking.GetData(_socketState);
        }

        /// <summary>
        /// Parses the first packet sent by the server, which contains the player's id and the world's size.
        /// </summary>
        /// <param name="data">The first packet's data.</param>
        private void ParseFirstPacket(string data)
        {
            var splitData = data.Split('\n');

            // Parse the first packet, containing our player id and the world size.
            var worldSize = int.Parse(splitData[1]);

            // Create a world from the parsed data.
            GameWorld = new World(worldSize);

            // Notify the listener that the connection was established and the world is ready.
            _connectionEstablishedCallback(this);
        }

        /// <summary>
        /// Parses a json packet sent by the server, which contains information about the game components in the world.
        /// </summary>
        /// <param name="data">The json packet's data, where each JSON object is separated by a newline.</param>
        private void ParseJsonPacket(string data)
        {
            var splitData = data.Split('\n');

            // For each json object
            foreach (var rawJson in splitData)
            {
                // Ignore empty items
                if (rawJson == "")
                    continue;

                try
                {
                    // Parse the data as a json object.
                    var parsedJson = JObject.Parse(rawJson);

                    // Determine the json type.
                    if (parsedJson["ship"] != null)
                    {
                        var ship = JsonConvert.DeserializeObject<Ship>(rawJson);
                        GameWorld.UpdateComponent(ship);
                    }
                    else if (parsedJson["proj"] != null)
                    {
                        var projectile = JsonConvert.DeserializeObject<Projectile>(rawJson);
                        // Remove dead projectiles.
                        if (!projectile.Active)
                        {
                            GameWorld.RemoveComponent(projectile);
                        }
                        else
                        {
                            GameWorld.UpdateComponent(projectile);
                        }
                    }
                    else if (parsedJson["star"] != null)
                    {
                        var star = JsonConvert.DeserializeObject<Star>(rawJson);
                        GameWorld.UpdateComponent(star);
                    }
                }
                catch (Exception e)
                {
                    Debug.Print("JSON Parsing Failed: " + e.Message);
                }
            }

            // Notify event listeners of an updated world.
            WorldModified?.Invoke();
        }
    }
}
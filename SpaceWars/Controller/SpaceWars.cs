using System;
using System.Collections.Generic;
using System.Linq;
using Networking;

namespace SpaceWars
{
    /// <summary>
    /// The main controller for the SpaceWars game.
    /// Uses the networking library to maintain a connection with the game server and notify listeners of changes to game components.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class SpaceWars
    {
        /// <summary>
        /// This delegate is called when a connection to SpaceWars was established.
        /// </summary>
        /// <param name="spaceWars">The connected SpaceWars client.</param>
        public delegate void ConnectionEstablished(SpaceWars spaceWars);

        /// <summary>
        /// This delegate is called when a connection to SpaceWars failed.
        /// </summary>
        /// <param name="reason">Why the connection failed.</param>
        public delegate void ConnectionFailed(string reason);

        /// <summary>
        /// The provided ConnectionEstablished delegate implementation.
        /// </summary>
        private readonly ConnectionEstablished _establishedCallback;

        /// <summary>
        /// The provided ConnectionFailed delegate implementation.
        /// </summary>
        private readonly ConnectionFailed _failedCallback;

        /// <summary>
        /// This delegate handles cases where any game component is updated (a ship, projectile, etc.)
        /// </summary>
        public delegate void GameComponentsListener();

        /// <summary>
        /// This event is fired whenever a game component (ship, projectile, etc.) is updated from the server.
        /// </summary>
        public event GameComponentsListener OnGameComponentsUpdated;

        /// <summary>
        /// Socket that the connection is made through.
        /// </summary>
        private SocketState _socketState;

        /// <summary>
        /// The dimensions of the game world (both sides use same length).
        /// </summary>
        public int WorldSize { get; private set; }

        /// <summary>
        /// The current ship that the player controls. 
        /// May be null if there is no established connection.
        /// </summary>
        public Ship PlayerShip => _ships[PlayerId];

        /// <summary>
        /// The nickname of the current player.
        /// </summary>
        public string PlayerNickname { get; }

        /// <summary>
        /// The ID of the current player.
        /// </summary>
        public int PlayerId { get; private set; }

        /// <summary>
        /// A mapping of each known ship in the game to that ship's ID.
        /// </summary>
        private readonly Dictionary<int, Ship> _ships = new Dictionary<int, Ship>();

        /// <summary>
        /// All known ships in the game.
        /// </summary>
        public IEnumerable<Ship> Ships => _ships.Values.ToList().AsReadOnly();

        //TODO: Projectiles, etc.

        /// <summary>
        /// Creates a new SpaceWars instance that has not been connected.
        /// Attempts to establish a connection to the given game server, using the given nickname.
        /// If a connection cannot be established, an exception is thrown. 
        /// </summary>
        /// <param name="hostName">The server address, excluding the port.</param>
        /// <param name="nickname">The nickname to use for the player connecting.</param>
        /// <param name="established">The callback for when a connection is established.</param>
        /// <param name="failed">The callback for when a connection has failed.</param>
        internal SpaceWars(string hostName, string nickname, ConnectionEstablished established, ConnectionFailed failed)
        {
            PlayerNickname = nickname;

            _establishedCallback = established;
            _failedCallback = failed;

            try
            {
                Networking.Networking.ConnectToServer(HandleData, hostName);
            }
            catch (Exception e)
            {
                _failedCallback(e.Message);
            }
        }

        /// <summary>
        /// Disconnects from the server. 
        /// This game instance should no longer be used once this method is called.
        /// </summary>
        public void Disconnect()
        {
            Networking.Networking.DisconnectFromServer(_socketState);
        }

        /// <summary>
        /// Method satisfying the HandleData delegate in Networking, is passed into
        /// the ConnectToServer call and is a wrapper for the ConnectedCallBack delegate in this class.
        /// </summary>
        /// <param name="state"></param>
        public void HandleData(SocketState state)
        {
            // When the stored socket state is null, we have connected for the first time.
            if (_socketState == null)
            {
                _socketState = state;

                // Parse the first packet, containing our player id and the world size.
                var splitData = _socketState.GetData().Split('\n');
                PlayerId = int.Parse(splitData[0]);
                WorldSize = int.Parse(splitData[1]);

                // Notify the connection callback that the game has connected.
                _establishedCallback(this);
                return;
            }

            //TODO: parse data as json
        }
    }
}
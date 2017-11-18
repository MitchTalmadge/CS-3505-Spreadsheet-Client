namespace SpaceWars
{
    /// <summary>
    /// The main controller for the SpaceWars game.
    /// Uses the networking library to maintain a connection with the 
    /// game server and notify listeners of changes to game components.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public partial class SpaceWarsClient
    {
        /// <summary>
        /// This delegate acts as a listener for the WorldModified event.
        /// </summary>
        /// <see cref="WorldModified"/>
        public delegate void WorldModificationListener();

        /// <summary>
        /// This event is fired whenever the world is modified.
        /// </summary>
        public event WorldModificationListener WorldModified;

        /// <summary>
        /// The nickname of the current player.
        /// </summary>
        public string PlayerNickname { get; }

        /// <summary>
        /// The game world.
        /// </summary>
        public World GameWorld { get; private set; }

        /// <summary>
        /// Creates a new SpaceWars instance and attempts a connection to the given hostname.
        /// The connection established/failed delegates are used to notify of connection status.
        /// This instance should not be used until the ConnectionEstablished delegate is invoked.
        /// </summary>
        /// <param name="hostname">The server address, excluding the port.</param>
        /// <param name="nickname">The nickname to use for the player connecting.</param>
        /// <param name="connectionEstablished">The callback for when a connection is established.</param>
        /// <param name="connectionFailed">The callback for when a connection has failed.</param>
        internal SpaceWarsClient(string hostname, string nickname, ConnectionEstablished connectionEstablished, ConnectionFailed connectionFailed)
        {
            PlayerNickname = nickname;

            _connectionEstablishedCallback = connectionEstablished;
            _connectionFailedCallback = connectionFailed;

            Connect(hostname, nickname);
        }
    }
}
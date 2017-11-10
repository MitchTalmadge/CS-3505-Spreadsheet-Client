using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars
{
    /// <summary>
    /// The main controller for the SpaceWars game.
    /// Uses the networking library to maintain a connection with the game server and notify listeners of changes to game components.
    /// </summary>
    public class SpaceWars
    {
        /// <summary>
        /// This delegate handles cases where any game component is updated (a ship, projectile, etc.)
        /// </summary>
        public delegate void GameComponentsListener();

        /// <summary>
        /// This event is fired whenever a game component (ship, projectile, etc.) is updated from the server.
        /// </summary>
        public event GameComponentsListener OnGameComponentsUpdated;

        /// <summary>
        /// The dimensions of the game world (both sides use same length).
        /// </summary>
        public int WorldSize { get; }

        /// <summary>
        /// The current ship that the player controls. 
        /// May be null if there is no established connection.
        /// </summary>
        public Ship PlayerShip { get; }

        /// <summary>
        /// Determines if there is an established connection.
        /// </summary>
        /// <returns>True if a connection has been established.</returns>
        public bool IsConnected => PlayerShip != null;

        /// <summary>
        /// Contains all the currently loaded ships in the game.
        /// </summary>
        private Dictionary<int, Ship> _ships;

        //TODO: Projectiles, etc.

        /// <summary>
        /// Attempts to establish a connection to the given game server, using the given nickname.
        /// If a connection cannot be established, an exception is thrown. 
        /// Otherwise, the connection has been established and the game is ready.
        /// </summary>
        /// <param name="hostName">The server address, excluding the port.</param>
        /// <param name="nickname">The nickname to use for the player connecting.</param>
        public SpaceWars(string hostName, string nickname)
        {
            throw new SpaceWarsConnectionFailedException("We didn't even try to connect :(");
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// A custom exception for when a connection to the Space Wars server could not be established.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class SpaceWarsConnectionFailedException : Exception
    {
        /// <inheritdoc />
        public SpaceWarsConnectionFailedException()
        {
        }

        /// <inheritdoc />
        public SpaceWarsConnectionFailedException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public SpaceWarsConnectionFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
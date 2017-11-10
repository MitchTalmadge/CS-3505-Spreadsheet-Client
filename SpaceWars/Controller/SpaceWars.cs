using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars
{
    public class SpaceWars
    {
        /// <summary>
        /// The current ship that the player controls. 
        /// May be null if there is no established connection.
        /// </summary>
        public Ship PlayerShip { get; }

        public SpaceWars()
        {
        }

        /// <summary>
        /// Determines if there is an established connection.
        /// </summary>
        /// <returns>True if a connection has been established.</returns>
        public bool IsConnected()
        {
            return PlayerShip != null;
        }

        /// <summary>
        /// Attempts to establish a connection to the given game server, using the given nickname.
        /// </summary>
        /// <param name="hostName">The server address, excluding the port.</param>
        /// <param name="nickname">The nickname to use for the player connecting.</param>
        /// <returns></returns>
        public bool ConnectToServer(string hostName, string nickname)
        {
            //TODO: Connect
            throw new NotImplementedException();
        }



    }
}
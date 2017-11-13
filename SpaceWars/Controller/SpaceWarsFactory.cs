namespace SpaceWars
{
    /// <summary>
    /// This class is used for creating SpaceWars instances.
    /// When a connection fails, this class prevents the user from accidentally interacting with the SpaceWars instance.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class SpaceWarsFactory
    {
        /// <summary>
        /// Attempts to connect to a SpaceWars server with the given details.
        /// </summary>
        /// <param name="hostName">The hostname to connect to.</param>
        /// <param name="nickname">The nickname of the player connecting.</param>
        /// <param name="established">The callback for when a connection is established.</param>
        /// <param name="failed">The callback for when a connection has failed.</param>
        public static void ConnectToSpaceWars(string hostName, string nickname, SpaceWars.ConnectionEstablished established,
            SpaceWars.ConnectionFailed failed)
        {
            var spaceWars = new SpaceWars(hostName, nickname, established, failed);
        }

    }
}

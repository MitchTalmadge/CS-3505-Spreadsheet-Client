using Properties;

namespace SpaceWars
{
    /// <summary>
    /// This configuration is used by the SpaceWarsServer and should be passed into its constructor.
    /// This allows the creator of the server instance to configure the server.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class SpaceWarsServerConfiguration
    {
        /// <summary>
        /// The width and height of the world.
        /// </summary>
        public int WorldSize { get; }

        /// <summary>
        /// The number of milliseconds that each frame should take up.
        /// Determines the framerate. (FPS = 1000 / MsPerFrame)
        /// </summary>
        public int MsPerFrame { get; }

        /// <summary>
        /// The number of frames to wait before subsequent shots are fired.
        /// This is to provide spacing between shots fired when the client is holding down the fire key.
        /// </summary>
        public int FramesPerShot { get; }

        /// <summary>
        /// How many frames until a ship is respawned when they die.
        /// </summary>
        public int RespawnRate { get; }

        /// <summary>
        /// The stars to place in the world.
        /// </summary>
        public Star[] Stars { get; }

        /// <summary>
        /// Creates a new SpaceWars server configuration.
        /// </summary>
        /// <param name="worldSize">The width and height of the world.</param>
        /// <param name="msPerFrame">The number of milliseconds that each frame should take up. Determines the framerate. (FPS = 1000 / MsPerFrame)</param>
        /// <param name="framesPerShot">The number of frames to wait before subsequent shots are fired. This is to provide spacing between shots fired when the client is holding down the fire key.</param>
        /// <param name="respawnRate">How many frames until a ship is respawned when they die.</param>
        /// <param name="stars">The stars to place in the world.</param>
        public SpaceWarsServerConfiguration(
            int worldSize,
            int msPerFrame,
            int framesPerShot,
            int respawnRate,
            Star[] stars)
        {
            WorldSize = worldSize;
            MsPerFrame = msPerFrame;
            FramesPerShot = framesPerShot;
            RespawnRate = respawnRate;
            Stars = stars;
        }

        /// <summary>
        /// Creates a new SpaceWars server configuration from the values in a properties file.
        /// </summary>
        /// <param name="properties">The properties file containing the values to load.</param>
        public SpaceWarsServerConfiguration(PropertiesFile properties)
        {
            
        }
       
        /// <summary>
        /// Serializes this configuration to the given properties file.
        /// </summary>
        /// <param name="properties">The properties file to write to.</param>
        public void SerializeToPropertiesFile(PropertiesFile properties)
        {
            //TODO: Write to properties
        }

        /// <summary>
        /// Loads a default configuration.
        /// </summary>
        /// <returns>The default configuration.</returns>
        public static SpaceWarsServerConfiguration Defaults()
        {
            return new SpaceWarsServerConfiguration(
                750,
                10,
                6,
                50,
                new[] {new Star(0, new Vector2D(0, 0), 0.01)}
            );
        }
    }
}
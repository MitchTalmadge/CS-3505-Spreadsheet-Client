using System;
using System.Collections.Generic;
using System.Globalization;
using Properties;

namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// This configuration is used by the SpaceWarsServer and should be passed into its constructor.
    /// This allows the creator of the server instance to configure the server.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class SpaceWarsServerConfiguration : IPropertySerializable
    {
        /// <summary>
        /// The width and height of the world.
        /// </summary>
        public int WorldSize { get; set; } = 750;

        /// <summary>
        /// The number of milliseconds that each frame should take up.
        /// Determines the framerate. (FPS = 1000 / MsPerFrame)
        /// </summary>
        public int MsPerFrame { get; set; } = 10;

        /// <summary>
        /// The number of frames to wait before subsequent shots are fired.
        /// This is to provide spacing between shots fired when the client is holding down the fire key.
        /// </summary>
        public int FramesPerShot { get; set; } = 6;

        /// <summary>
        /// How many frames until a ship is respawned when they die.
        /// </summary>
        public int RespawnRate { get; set; } = 50;

        /// <summary>
        /// The stars to place in the world.
        /// </summary>
        public Star[] Stars { get; set; } = {new Star(0, new Vector2D(0, 0), 0.01)};

        public IEnumerable<Property> ToProperties()
        {
            // Add simple fields.
            var properties = new List<Property>
            {
                new Property(nameof(WorldSize), WorldSize.ToString()),
                new Property(nameof(MsPerFrame), MsPerFrame.ToString()),
                new Property(nameof(FramesPerShot), FramesPerShot.ToString()),
                new Property(nameof(RespawnRate), RespawnRate.ToString())
            };

            // Add all stars.
            foreach (var star in Stars)
            {
                properties.Add(new Property(
                    nameof(Star),
                    attributes: new Dictionary<string, string>
                    {
                        ["x"] = star.Location.GetX().ToString(),
                        ["y"] = star.Location.GetY().ToString(),
                        ["mass"] = star.Mass.ToString()
                    }
                ));
            }

            return properties;
        }

        public void FromProperties(IEnumerable<Property> properties)
        {
            var stars = new List<Star>();

            foreach (var property in properties)
            {
                switch (property.Key)
                {
                    case nameof(WorldSize):
                        if (int.TryParse(property.Value, out var worldSize))
                            WorldSize = worldSize;
                        break;
                    case nameof(MsPerFrame):
                        if (int.TryParse(property.Value, out var msPerFrame))
                            MsPerFrame = msPerFrame;
                        break;
                    case nameof(FramesPerShot):
                        if (int.TryParse(property.Value, out var framesPerShot))
                            FramesPerShot = framesPerShot;
                        break;
                    case nameof(RespawnRate):
                        if (int.TryParse(property.Value, out var respawnRate))
                            RespawnRate = respawnRate;
                        break;
                    case nameof(Star):
                        if (double.TryParse(property.Attributes["x"], out var x)
                            && double.TryParse(property.Attributes["y"], out var y)
                            && double.TryParse(property.Attributes["mass"], out var mass))
                        {
                            stars.Add(new Star(stars.Count, new Vector2D(x, y), mass));
                        }
                        break;
                }
            }

            Stars = stars.ToArray();
        }
    }
}
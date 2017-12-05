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
        public int MsPerFrame { get; set; } = 16;

        /// <summary>
        /// The number of frames to wait before subsequent shots are fired.
        /// This is to provide spacing between shots fired when the client is holding down the fire key.
        /// </summary>
        public int FramesPerShot { get; set; } = 6;

        /// <summary>
        /// How many frames until a ship is respawned when they die.
        /// </summary>
        public int RespawnRate { get; set; } = 300;

        /// <summary>
        /// How many hitpoints ships should start with.
        /// </summary>
        public int ShipHitpoints { get; set; } = 5;

        /// <summary>
        /// How many units per frame that projectiles travel.
        /// </summary>
        public double ProjectileSpeed { get; set; } = 15;

        /// <summary>
        /// How many units per frame that ships accellerate when thrusting.
        /// </summary>
        public double ShipEngineStrength { get; set; } = 0.08;

        /// <summary>
        /// The degrees that a ship can rotate per frame.
        /// </summary>
        public double ShipTurningRate { get; set; } = 2;

        /// <summary>
        /// How close a projectile must get to collide with a ship.
        /// </summary>
        public double ShipCollisionRadius { get; set; } = 20;

        /// <summary>
        /// How close a projectile or ship must get to collide with a star.
        /// </summary>
        public double StarCollisionRadius { get; set; } = 35;

        /// <summary>
        /// Determines if the explosive game mode is enabled.
        /// </summary>
        public bool ExplosiveGameMode { get; set; } = false;

        /// <summary>
        /// The stars to place in the world.
        /// </summary>
        public Star[] Stars { get; set; } = {new Star(new Vector2D(0, 0), 0.01)};

        public IEnumerable<Property> ToProperties()
        {
            // Add simple fields.
            var properties = new List<Property>
            {
                new Property(nameof(WorldSize), WorldSize.ToString(),
                    comment: "The width and height of the game world."),
                new Property(nameof(MsPerFrame), MsPerFrame.ToString(),
                    comment: "The number of milliseconds to spend per frame. FPS = 1000 / MsPerFrame."),
                new Property(nameof(FramesPerShot), FramesPerShot.ToString(),
                    comment: "The number of frames to pause between each firing of a projectile."),
                new Property(nameof(RespawnRate), RespawnRate.ToString(),
                    comment: "How many frames before a dead ship respawns."),
                new Property(nameof(ShipHitpoints), ShipHitpoints.ToString(),
                    comment: "How many hitpoints ships should start with."),
                new Property(nameof(ProjectileSpeed), ProjectileSpeed.ToString(CultureInfo.InvariantCulture),
                    comment: "How many units per frame that projectiles travel."),
                new Property(nameof(ShipEngineStrength), ShipEngineStrength.ToString(CultureInfo.InvariantCulture),
                    comment: "How many units per frame that ships accellerate when thrusting."),
                new Property(nameof(ShipTurningRate), ShipTurningRate.ToString(CultureInfo.InvariantCulture),
                    comment: "The degrees that a ship can rotate per frame."),
                new Property(nameof(ShipCollisionRadius), ShipCollisionRadius.ToString(CultureInfo.InvariantCulture),
                    comment: "How close a projectile must get to collide with a ship."),
                new Property(nameof(StarCollisionRadius), StarCollisionRadius.ToString(CultureInfo.InvariantCulture),
                    comment: "How close a projectile or ship must get to collide with a star."),
                new Property(nameof(ExplosiveGameMode), ExplosiveGameMode.ToString(),
                    comment:
                    "Set to true to enable the explosive game mode, where a large number of projectiles are spawned each time a sihp dies.")
            };

            // Add all stars.
            foreach (var star in Stars)
            {
                properties.Add(new Property(
                    nameof(Star),
                    attributes: new Dictionary<string, string>
                    {
                        ["x"] = star.Location.GetX().ToString(CultureInfo.InvariantCulture),
                        ["y"] = star.Location.GetY().ToString(CultureInfo.InvariantCulture),
                        ["mass"] = star.Mass.ToString(CultureInfo.InvariantCulture)
                    },
                    comment: "The location and mass of a star."
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
                        if (int.TryParse(property.Value, out var parsedInt))
                            WorldSize = parsedInt;
                        break;
                    case nameof(MsPerFrame):
                        if (int.TryParse(property.Value, out parsedInt))
                            MsPerFrame = parsedInt;
                        break;
                    case nameof(FramesPerShot):
                        if (int.TryParse(property.Value, out parsedInt))
                            FramesPerShot = parsedInt;
                        break;
                    case nameof(RespawnRate):
                        if (int.TryParse(property.Value, out parsedInt))
                            RespawnRate = parsedInt;
                        break;
                    case nameof(ShipHitpoints):
                        if (int.TryParse(property.Value, out parsedInt))
                            ShipHitpoints = parsedInt;
                        break;
                    case nameof(ProjectileSpeed):
                        if (double.TryParse(property.Value, out var parsedDouble))
                            ProjectileSpeed = parsedDouble;
                        break;
                    case nameof(ShipEngineStrength):
                        if (double.TryParse(property.Value, out parsedDouble))
                            ShipEngineStrength = parsedDouble;
                        break;
                    case nameof(ShipTurningRate):
                        if (double.TryParse(property.Value, out parsedDouble))
                            ShipTurningRate = parsedDouble;
                        break;
                    case nameof(ShipCollisionRadius):
                        if (double.TryParse(property.Value, out parsedDouble))
                            ShipCollisionRadius = parsedDouble;
                        break;
                    case nameof(StarCollisionRadius):
                        if (double.TryParse(property.Value, out parsedDouble))
                            StarCollisionRadius = parsedDouble;
                        break;
                    case nameof(ExplosiveGameMode):
                        if (bool.TryParse(property.Value, out var parsedBool))
                            ExplosiveGameMode = parsedBool;
                        break;
                    case nameof(Star):
                        if (double.TryParse(property.Attributes["x"], out var x)
                            && double.TryParse(property.Attributes["y"], out var y)
                            && double.TryParse(property.Attributes["mass"], out var mass))
                        {
                            stars.Add(new Star(new Vector2D(x, y), mass));
                        }
                        break;
                }
            }

            Stars = stars.ToArray();
        }
    }
}
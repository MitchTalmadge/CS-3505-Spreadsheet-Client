using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a spaceship.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class Ship : GameComponent
    {
        /// <summary>
        /// The size of a single ship in the sprite sheet.
        /// </summary>
        private static readonly Size ShipSpriteSize = new Size(36, 44);

        /// <summary>
        /// The size of a ship's crop region.
        /// </summary>
        private static readonly Size ShipCropSize = new Size(ShipSpriteSize.Width - 1, ShipSpriteSize.Height - 1);

        /// <summary>
        /// The size to draw the ship.
        /// </summary>
        private static readonly Size ShipDrawSize = new Size(36, 44);

        /// <summary>
        /// The Id of this ship.
        /// </summary>
        [JsonProperty("ship")]
        private int _shipId;

        /// <inheritdoc/>
        public override int Id => _shipId;

        /// <summary>
        /// The name of this ship (aka the player's name).
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Determines if the ship is currently thrusting forward.
        /// This is used to change appearance of the ship by adding exhaust.
        /// Client use only.
        /// </summary>
        [JsonProperty("thrust")] private bool _thrusting;

        /// <summary>
        /// The current velocity of the ship.
        /// Server use only.
        /// </summary>
        [JsonIgnore]
        public Vector2D Velocity { get; set; } = new Vector2D(0, 0);

        /// <summary>
        /// The hitpoints remaining for this ship.
        /// Ranges from 0 to 5, where 5 is full health and 0 is temporarily dead (waiting for respawn).
        /// </summary>
        [JsonProperty("hp")]
        public double Health { get; set; }

        /// <summary>
        /// The number of frames remaining until the ship respawns.
        /// Server use only.
        /// </summary>
        [JsonIgnore]
        public int RespawnFrames { get; set; }

        /// <summary>
        /// The number of frames remaining until a projectile can be fired again.
        /// Server use only.
        /// </summary>
        [JsonIgnore]
        public int ProjectileCooldown { get; set; }

        /// <summary>
        /// The score of this ship.
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; set; }

        /// <summary>
        /// Ship's constructor, initializes Ship data.
        /// </summary>
        /// <param name="id">The ID of the ship. Should be unique.</param>
        /// <param name="name">The nickname of the ship.</param>
        public Ship(int id, string name)
        {
            _shipId = id;
            Name = name;
        }

        /// <inheritdoc />
        /// <summary>
        /// The ship to be drawn is based on the ID. A ship with ID 0 is red, ID 1 is orange, etc. until ID 8 which is red again.
        /// Thrusting ships have a different image that includes exhaust.
        /// </summary>
        public override Tuple<Bitmap, Rectangle, Size> GetDrawingDetails()
        {
            // Do not draw if the health is 0.
            if (Health.Equals(0))
                return null;

            var colorIndex = Id % 8;

            // Column is determined by the color index.
            var spriteStartPoint = new Point(colorIndex * ShipSpriteSize.Width, 0);

            // Move down one row for thrusting ships
            if (_thrusting)
                spriteStartPoint.Y = ShipSpriteSize.Height;

            return new Tuple<Bitmap, Rectangle, Size>(
                Resources.ships, 
                new Rectangle(spriteStartPoint, ShipCropSize), 
                ShipDrawSize);
        }

        /// <summary>
        /// Represents the valid commands that can be sent to the server.
        /// </summary>
        /// <authors>Jiahui Chen, Mitch Talmadge</authors>
        public enum Command
        {
            Thrust,
            Left,
            Right,
            Fire
        }
    }
}
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
        /// </summary>
        [JsonProperty("thrust")] private bool _thrusting;

        /// <summary>
        /// The hitpoints remaining for this ship.
        /// Ranges from 0 to 5, where 5 is full health and 0 is temporarily dead (waiting for respawn).
        /// </summary>
        [JsonProperty("hp")]
        public int Health { get; private set; }

        /// <summary>
        /// The score of this ship.
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// The ship to be drawn is based on the ID. A ship with ID 0 is red, ID 1 is orange, etc. until ID 8 which is red again.
        /// Thrusting ships have a different image that includes exhaust.
        /// </summary>
        public override Tuple<Bitmap, Rectangle, Size> GetDrawingDetails()
        {
            // Do not draw if the health is 0.
            if (Health == 0)
                return null;

            var colorIndex = Id % 8;

            // Column is determined by the color index.
            var spriteStartPoint = new Point(colorIndex * ShipSpriteSize.Width, 0);

            // Move down one row for thrusting ships
            if (_thrusting)
                spriteStartPoint.Y = ShipSpriteSize.Height;

            return new Tuple<Bitmap, Rectangle, Size>(
                Resources.ships, 
                new Rectangle(spriteStartPoint, ShipSpriteSize), 
                ShipDrawSize);
        }
    }
}
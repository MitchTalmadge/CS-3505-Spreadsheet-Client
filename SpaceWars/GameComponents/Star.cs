using Newtonsoft.Json;
using SpaceWars.Properties;
using System;
using System.Drawing;

namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Star.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class Star : GameComponent
    {
        /// <summary>
        /// The size of a star in the sprite sheet.
        /// Four times the size of a ship.
        /// </summary>
        private static readonly Size StarSpriteSize = new Size(144, 176);

        /// <summary>
        /// The Id of this Star.
        /// </summary>
        [JsonProperty("star")] private int _starId;

        /// <inheritdoc/>
        public override int Id => _starId;

        /// <summary>
        /// Represents this Star's mass.
        /// </summary>
        [JsonProperty("mass")] private int _mass;

        /// <inheritdoc/>
        public override Tuple<Bitmap, Rectangle> GetDrawingDetails()
        {
            return new Tuple<Bitmap, Rectangle>(Resources.star,
                new Rectangle(new Point(StarSpriteSize.Width, 0), StarSpriteSize));
        }
    }
}
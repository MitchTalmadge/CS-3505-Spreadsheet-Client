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
        /// </summary>
        private static readonly Size StarSpriteSize = new Size(256, 256);

        /// <summary>
        /// The size of the star to be drawn.
        /// </summary>
        private static readonly Size StarDrawSize = new Size(100, 100);

        /// <summary>
        /// The Id of this Star.
        /// </summary>
        [JsonProperty("star")] private int _starId;

        /// <inheritdoc/>
        public override int Id => _starId;

        /// <summary>
        /// Represents this Star's mass.
        /// </summary>
        [JsonProperty("mass")] private double _mass;

        /// <inheritdoc/>
        public override Tuple<Bitmap, Rectangle, Size> GetDrawingDetails()
        {
            return new Tuple<Bitmap, Rectangle, Size>(
                Resources.star,
                new Rectangle(new Point(0,0), StarSpriteSize), 
                StarDrawSize);
        }
    }
}
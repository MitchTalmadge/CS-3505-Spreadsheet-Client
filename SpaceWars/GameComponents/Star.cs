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
        /// The size of a star's crop region.
        /// </summary>
        private static readonly Size StarCropSize = new Size(StarSpriteSize.Width - 1, StarSpriteSize.Height - 1);

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
        [JsonProperty("mass")]
        public double Mass { get; private set; }

        /// <summary>
        /// Creates a star.
        /// </summary>
        /// <param name="id">The ID of the star.</param>
        /// <param name="location">The location of the star.</param>
        /// <param name="mass">The mass of the star.</param>
        public Star(int id, Vector2D location, double mass)
        {
            _starId = id;
            Location = location;
            Mass = mass;
        }

        /// <inheritdoc/>
        public override Tuple<Bitmap, Rectangle, Size> GetDrawingDetails()
        {
            return new Tuple<Bitmap, Rectangle, Size>(
                Resources.star,
                new Rectangle(new Point(0,0), StarCropSize), 
                StarDrawSize);
        }
    }
}
using Newtonsoft.Json;
using SpaceWars.Properties;
using System;
using System.Drawing;


namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Projectile.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class Projectile : GameComponent
    {
        /// <summary>
        /// The size of a single projectile in the sprite sheet.
        /// A fourth of the size of a ship.
        /// </summary>
        private static readonly Size ProjectileSpriteSize = new Size(9, 11);

        /// <summary>
        /// The Id of this Projectile.
        /// </summary>
        [JsonProperty("proj")] private int _projectileId;

        /// <inheritdoc/>
        public override int Id => _projectileId;

        /// <summary>
        /// Indicates whether or not this Projectile is deactivated or not.
        /// </summary>
        [JsonProperty("alive")]
        public bool Active { get; private set; }

        /// <summary>
        /// ID of the ship that created the projectile. Can be used 
        /// to draw the projectiles with a different color or image.
        /// </summary>
        [JsonProperty("owner")]
        public int OwnerShipId { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// There are 2 sprites for projectiles to be drawn, one frame is the projectile
        /// "exploding" and the other is just a regular projectile.
        /// </summary>
        public override Tuple<Bitmap, Rectangle> GetDrawingDetails()
        {
            // Don't draw if inactive.
            if (!Active)
                return null;

            return new Tuple<Bitmap, Rectangle>(Resources.projectile,
                new Rectangle(new Point(ProjectileSpriteSize.Width, 0), ProjectileSpriteSize));
        }
    }
}
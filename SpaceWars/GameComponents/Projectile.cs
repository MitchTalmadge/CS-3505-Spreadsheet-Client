using Newtonsoft.Json;
using SpaceWars.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

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
        /// </summary>
        private static readonly Size ProjectileSpriteSize = new Size(64,64);

        /// <summary>
        /// The size of a projectile's crop region.
        /// </summary>
        private static readonly Size ProjectileCropSize = new Size(ProjectileSpriteSize.Width - 1, ProjectileSpriteSize.Height - 1);

        /// <summary>
        /// The size to draw a projectile.
        /// </summary>
        private static readonly Size ProjectileDrawSize = new Size(11, 11);

        /// <summary>
        /// Counts the number of Projectiles currently istantiated. 
        /// Increments on each ship's creation and becomes a new Projectile's ID.
        /// </summary>
        private static int ProjectileCount;

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

        /// <summary>
        /// Projectile's constructor, initializes Projectile data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shipID"></param>
        public Projectile(int shipID)
        {
            OwnerShipId = shipID;
            _projectileId = ++ProjectileCount;
        }

        /// <inheritdoc />
        public override Tuple<Bitmap, Rectangle, Size> GetDrawingDetails()
        {
            // Don't draw if inactive.
            if (!Active)
                return null;

            return new Tuple<Bitmap, Rectangle, Size>(
                Resources.projectile,
                new Rectangle(new Point(0, 0), ProjectileCropSize),
                ProjectileDrawSize);
        }
    }
}
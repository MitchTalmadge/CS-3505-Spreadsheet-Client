using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpaceWars
{

    /// <inheritdoc />
    /// <summary>
    /// Represents a Projectile.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    class Projectile : GameComponent
    {
        /// <summary>
        /// The Id of this Projectile.
        /// </summary>
        [JsonProperty("proj")]
        private int _projectileId;

        /// <summary>
        /// Indicates whether or not this Projectile is deactivated or not.
        /// </summary>
        [JsonProperty("alive")]
        public bool Active { get; private set; }

        public override Tuple<Bitmap, Rectangle> GetDrawingDetails()
        {
            throw new NotImplementedException();
        }

        protected override int GetId()
        {
            throw new NotImplementedException();
        }
    }
}

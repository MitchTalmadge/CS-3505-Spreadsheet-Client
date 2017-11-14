using Newtonsoft.Json;
using SpaceWars.Properties;
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
    /// Represents a Star.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    class Star : GameComponent
    {
        /// <summary>
        /// The size of a star in the sprite sheet.
        /// Four times the size of a ship.
        /// </summary>
        private static readonly Size StarSpriteSize = new Size(144, 176);

        /// <summary>
        /// The Id of this Star.
        /// </summary>
        [JsonProperty("star")]
        private int starID;

        /// <summary>
        /// Represents this Star's mass.
        /// </summary>
        [JsonProperty("star")]
        private int mass;

        public override Tuple<Bitmap, Rectangle> GetDrawingDetails()
        {
            return new Tuple<Bitmap, Rectangle>(Resources.star,
                new Rectangle(new Point(StarSpriteSize.Width, 0), StarSpriteSize));
        }

        /// <inheritdoc />
        protected override int GetId()
        {
            return starID;
        }
    }
}

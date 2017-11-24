using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <summary>
    /// Represents a generic game component, like a ship or a projectile.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public abstract class GameComponent
    {
        /// <summary>
        /// The ID of this game component.
        /// </summary>
        [JsonIgnore]
        public abstract int Id { get; }

        /// <summary>
        /// The location of this component in world coordinates.
        /// </summary>
        [JsonProperty("loc")]
        public Vector2D Location { get; protected set; } = new Vector2D(0, 0);

        /// <summary>
        /// The direction that this component is facing in world coordinates.
        /// </summary>
        [JsonProperty("dir")]
        public Vector2D Direction { get; protected set; } = new Vector2D(0, -1);

        /// <summary>
        /// Gives details for which image should be drawn, and how it should be cropped.
        /// </summary>
        /// <returns>A tuple containing the image bitmap and a rectangle determining where the image should be cropped, and a size for its scale.</returns>
        public abstract Tuple<Bitmap, Rectangle, Size> GetDrawingDetails();
    }
}
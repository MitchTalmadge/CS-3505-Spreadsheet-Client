using System.Windows.Forms;
using Newtonsoft.Json;

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
        public int Id => GetId();

        /// <summary>
        /// Determines the ID of this game component.
        /// Since each component uses a different field for ID, like "ship" for ships, 
        /// this is used to consolidate all data into an "Id" property.
        /// </summary>
        /// <returns>The ID of this game component.</returns>
        protected abstract int GetId();

        /// <summary>
        /// The location of this component in world coordinates.
        /// </summary>
        [JsonProperty("loc")]
        public Vector2D Location { get; } = new Vector2D(0, 0);

        /// <summary>
        /// The direction that this component is facing in world coordinates.
        /// </summary>
        [JsonProperty("dir")]
        public Vector2D Direction { get; } = new Vector2D(0, 0);
    }
}
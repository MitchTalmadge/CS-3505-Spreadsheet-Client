using System.Windows.Forms;

namespace SpaceWars
{
    /// <summary>
    /// Represents a generic game component, like a ship or a projectile.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public abstract class GameComponent
    {

        /// <summary>
        /// Draws this component as it should be displayed in the world.
        /// </summary>
        /// <param name="e">Contains the Graphics instance to use when drawing.</param>
        public abstract void DrawComponent(PaintEventArgs e);

    }
}

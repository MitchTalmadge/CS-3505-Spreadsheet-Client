using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// The panel used for drawing the game world.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public sealed class WorldPanel : Panel
    {
        /// <summary>
        /// The game components to be drawn when this component is painted.
        /// </summary>
        private List<GameComponent> _gameComponents;

        public WorldPanel()
        {
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw background
            using (var brush = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(brush, ClientRectangle);

            // Draw double border
            e.Graphics.DrawRectangle(Pens.White, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1); // Outer
            e.Graphics.DrawRectangle(Pens.Aqua, 2, 2, ClientSize.Width - 5, ClientSize.Height - 5); // Inner

            // Draw game components
            /*foreach (var gameComponent in _gameComponents)
            {
                //TODO: Draw game component
            }*/
        }

        /// <summary>
        /// Schedules the given game components to be drawn on the next tick.
        /// </summary>
        /// <param name="gameComponents">The components to draw, in the order to be drawn.</param>
        public void DrawGameComponents(IEnumerable<GameComponent> gameComponents)
        {
            _gameComponents = new List<GameComponent>(gameComponents);
        }
    }
}
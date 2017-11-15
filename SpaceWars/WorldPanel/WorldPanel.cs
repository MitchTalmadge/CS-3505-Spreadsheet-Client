using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        /// The size of this world, which has the same length on each side.
        /// </summary>
        private int WorldSize => Size.Height;

        /// <summary>
        /// The game components to be drawn when this component is painted.
        /// </summary>
        private IEnumerable<GameComponent> _gameComponents = new GameComponent[0];

        public WorldPanel()
        {
            BackColor = Color.Transparent;
            DoubleBuffered = true;
        }

        /// <summary>
        /// Schedules the given game components to be drawn on the next tick.
        /// </summary>
        /// <param name="gameComponents">The components to draw, in the order to be drawn.</param>
        public void DrawGameComponents(IEnumerable<GameComponent> gameComponents)
        {
            _gameComponents = gameComponents.ToArray();

            // Invalidate this component for redrawing.
            try
            {
                Invoke(new MethodInvoker(Refresh));
            }
            catch (ObjectDisposedException)
            {
                //ignored
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Draws the background, border, and individual game components.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw background
            using (var brush = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(brush, ClientRectangle);

            // Draw double border
            e.Graphics.DrawRectangle(Pens.White, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1); // Outer
            e.Graphics.DrawRectangle(Pens.Aqua, 2, 2, ClientSize.Width - 5, ClientSize.Height - 5); // Inner

            // Draw game components
            foreach (var gameComponent in _gameComponents)
            {
                // Ensure there is something to draw.
                var imageDetails = gameComponent.GetDrawingDetails();
                if (imageDetails == null)
                    continue;

                // Translate the graphics object so that drawing at (0, 0) will put the component in the correct place.
                var translation = WorldVectorToImagePoint(gameComponent.Location);
                e.Graphics.TranslateTransform(translation.X, translation.Y);
                e.Graphics.RotateTransform(gameComponent.Direction.ToAngle());

                // Draw the component at (0, 0)
                var image = imageDetails.Item1;
                var cropRegion = imageDetails.Item2;
                var drawSize = imageDetails.Item3;

                //Centering image 
                var offset = 0 - .5 * drawSize.Width;
                e.Graphics.DrawImage(image, new Rectangle((int)offset, (int)offset, drawSize.Width, drawSize.Height), cropRegion,
                    GraphicsUnit.Pixel);

                // Restore the original transformation of the graphics.
                e.Graphics.ResetTransform();
            }
        }

        /// <summary>
        /// Converts the given world-space based vector to an image-space based point.
        /// </summary>
        /// <param name="vector">The world-space vector to convert.</param>
        /// <returns>A new point containing the converted vector coordinates.</returns>
        private Point WorldVectorToImagePoint(Vector2D vector)
        {
            return new Point((int) vector.GetX() + WorldSize / 2, (int) vector.GetY() + WorldSize / 2);
        }
    }
}
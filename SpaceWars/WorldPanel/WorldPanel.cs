using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        /// The SpaceWars client for which this panel is drawing the world.
        /// </summary>
        private readonly SpaceWarsClient _spaceWarsClient;

        /// <summary>
        /// These multipliers account for differences in world size versus screen size.
        /// </summary>
        private double[] _scaleMultipliers;

        /// <summary>
        /// The game components to be drawn when this component is painted.
        /// </summary>
        private IEnumerable<GameComponent> _gameComponents = new GameComponent[0];

        /// <inheritdoc />
        /// <summary>
        /// Creates a new WorldPanel.
        /// </summary>
        /// <param name="spaceWarsClient">The SpaceWars client for which this panel is drawing the world.</param>
        public WorldPanel(SpaceWarsClient spaceWarsClient)
        {
            _spaceWarsClient = spaceWarsClient;
            _spaceWarsClient.WorldModified += OnWorldModified;

            BackColor = Color.Transparent;
            DoubleBuffered = true;

            // Compute scale of world when the size of this panel changes.
            SizeChanged += (sender, args) =>
            {
                _scaleMultipliers = new[]
                    {(double) Width / _spaceWarsClient.GameWorld.Size, (double) Height / _spaceWarsClient.GameWorld.Size};
            };
        }

        /// <summary>
        /// Called when the world is modified in the SpaceWars client.
        /// </summary>
        private void OnWorldModified()
        {
            _gameComponents = GetGameComponentsToDraw();

            // Invalidate this component for redrawing.
            try
            {
                if (IsHandleCreated)
                    Invoke(new MethodInvoker(Refresh));
            }
            catch (ObjectDisposedException)
            {
                //ignored
            }
        }

        /// <summary>
        /// Retrieves and returns the game components in the order they should be drawn.
        /// </summary>
        /// <returns>An IEnumerable containing all the components to draw in the order they should be drawn.</returns>
        private IEnumerable<GameComponent> GetGameComponentsToDraw()
        {
            // Draw first
            foreach (var projectile in _spaceWarsClient.GameWorld.GetComponents<Projectile>())
            {
                yield return projectile;
            }

            // Draw second
            foreach (var star in _spaceWarsClient.GameWorld.GetComponents<Star>())
            {
                yield return star;
            }

            // Draw third
            foreach (var ship in _spaceWarsClient.GameWorld.GetComponents<Ship>())
            {
                yield return ship;
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
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

                // Draw the component at (0, 0)
                var image = imageDetails.Item1;
                var cropRegion = imageDetails.Item2;
                var drawSize = imageDetails.Item3;

                // Modify the draw size based on the scale of the world.
                drawSize = new Size((int) (_scaleMultipliers[0] * drawSize.Width),
                    (int) (_scaleMultipliers[1] * drawSize.Height));

                // This offset is to center the image.
                var offset = 0 - .5 * drawSize.Width;

                e.Graphics.DrawImage(image, new Rectangle((int) offset, (int) offset, drawSize.Width, drawSize.Height),
                    cropRegion,
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
            // Convert the world coordinates to screen coordinates by adding half the size of the screen.
            return new Point((int) (vector.GetX() * _scaleMultipliers[0]) + Width / 2,
                (int) (vector.GetY() * _scaleMultipliers[1]) + Height / 2);
        }
    }
}
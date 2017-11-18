using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// The panel used for drawing the scores of players.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public sealed class ScoreboardPanel : Panel
    {
        /// <summary>
        /// The SpaceWars client containing scores.
        /// </summary>
        private readonly SpaceWarsClient _spaceWarsClient;

        /// <summary>
        /// An array of ships, sorted by their scores.
        /// </summary>
        private IEnumerable<Ship> _shipsSortedByScore = new Ship[0];

        /// <summary>
        /// The pen used when drawing the border.
        /// </summary>
        private static readonly Pen BorderPen = new Pen(Color.FromArgb(200, 255, 255, 255), 2)
        {
            Alignment = PenAlignment.Inset
        };

        /// <inheritdoc />
        /// <summary>
        /// Creates a Scoreboard Panel that represents the scores in a SpaceWars client.
        /// </summary>
        /// <param name="spaceWarsClient">The SpaceWars client that holds player scores.</param>
        public ScoreboardPanel(SpaceWarsClient spaceWarsClient)
        {
            _spaceWarsClient = spaceWarsClient;
            _spaceWarsClient.WorldModified += OnWorldModified;
            DoubleBuffered = true;

            BackColor = Color.FromArgb(80, 255, 255, 255);

            CreateHeader();
        }

        /// <summary>
        /// Updates scores every time game components are updated.
        /// </summary>
        private void OnWorldModified()
        {
            // Sort the ships by their scores descending
            _shipsSortedByScore = _spaceWarsClient.GameWorld.GetComponents<Ship>().OrderByDescending(ship => ship.Score);

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
        /// Creates the "Scoreboard" header at the top of the panel.
        /// </summary>
        private void CreateHeader()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Label
            {
                Text = Resources.Scoreboard_Header,
                Font = new Font(new FontFamily("OCR A Extended"), 20, FontStyle.Underline),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Location = new Point(10, 10),
                AutoSize = true,
                Parent = this
            };
        }

        /// <inheritdoc />
        /// <summary>
        /// Draws all ship stats.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw background
            using (var brush = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(brush, ClientRectangle);

            // Draw border
            e.Graphics.DrawRectangle(BorderPen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);

            // Draw stats.
            var offset = 0;
            foreach (var ship in _shipsSortedByScore)
            {
                DrawShipStats(ship, offset, e.Graphics);
                offset++;
            }
        }

        /// <summary>
        /// Draws a ship's statistics (score, health, name).
        /// </summary>
        /// <param name="ship">The ship whose stats should be drawn.</param>
        /// <param name="offset">The number of ships that this ship is offset. For example, the second ship would be offset = 1.</param>
        /// <param name="graphics">The graphics object to draw with.</param>
        private void DrawShipStats(Ship ship, int offset, Graphics graphics)
        {
            Font font = new Font(new FontFamily("OCR A Extended"), 15, FontStyle.Regular);
            Brush brush = new SolidBrush(Color.DarkSlateBlue);

            // Translate the graphics to where we will draw.
            graphics.TranslateTransform(0, 40 + offset * 50);

            graphics.DrawString(ship.Name, font, brush, new Point(0, 0));
            graphics.DrawString(ship.Score.ToString(), font, brush, new Point(100, 0));

            graphics.DrawRectangle(new Pen(brush), 10, 30, ClientSize.Width - 20, 15);

            //fills health bar (rectangle) proportionately to health of ship
            double health = Convert.ToDouble(ship.Health);
            graphics.FillRectangle(brush, 10, 30,
                (float) (health / 5.0) * (ClientSize.Width - 20), 15);
        }
    }
}
using System.CodeDom;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

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
        /// The pen used when drawing the border.
        /// </summary>
        private static readonly Pen BorderPen = new Pen(Color.FromArgb(200, 255, 255, 255), 2)
        {
            Alignment = PenAlignment.Inset
        };

        public ScoreboardPanel()
        {
            BackColor = Color.FromArgb(80, 255, 255, 255);

            CreateHeader();
        }

        /// <summary>
        /// Creates the "Scoreboard" header at the top of the panel.
        /// </summary>
        private void CreateHeader()
        {
            var header = new Label
            {
                Text = "Scoreboard",
                Font = new Font(new FontFamily("OCR A Extended"), 20, FontStyle.Underline),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Location = new Point(10, 10),
                AutoSize = true,
                Parent = this
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw background
            using (var brush = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(brush, ClientRectangle);

            // Draw border
            e.Graphics.DrawRectangle(BorderPen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceWars
{
    public partial class GameForm : Form
    {
        private readonly WorldPanel _worldPanel;

        public GameForm()
        {
            InitializeComponent();

            _worldPanel = new WorldPanel
            {
                Location = new Point(0, 0),
                Margin = new Padding(20),
                Parent = tableLayoutPanel
            };
            tableLayoutPanel.SetColumn(_worldPanel, 0);
            tableLayoutPanel.SetRow(_worldPanel, 0);

            // World size is 750 by default.
            SetWorldSize(750);
        }

        /// <summary>
        /// Changes the size of the world panel.
        /// </summary>
        /// <param name="size">The size to use for both height and width of the world panel.</param>
        private void SetWorldSize(int size)
        {
            _worldPanel.Size = new Size(size, size);
            tableLayoutPanel.LayoutSettings.ColumnStyles[0].Width = size + _worldPanel.Margin.Horizontal;
            tableLayoutPanel.LayoutSettings.RowStyles[0].Height = size + _worldPanel.Margin.Vertical;
        }
    }
}
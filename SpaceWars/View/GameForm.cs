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
        private WorldPanel _worldPanel;

        public GameForm()
        {
            InitializeComponent();

            CreateWorldPanel();
        }

        private void CreateWorldPanel()
        {
            _worldPanel = new WorldPanel
            {
                Margin = new Padding(20),
                Location = new Point(20, 20),
                Parent = mainLayoutPanel
            };

            mainLayoutPanel.SetCellPosition(_worldPanel, new TableLayoutPanelCellPosition(0, 0));

            SetWorldSize(750);
        }

        /// <summary>
        /// Changes the size of the world panel.
        /// </summary>
        /// <param name="size">The size to use for both height and width of the world panel.</param>
        private void SetWorldSize(int size)
        {
            _worldPanel.Size = new Size(size, size);
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            new MainMenuForm().Show();
        }

        private void GameForm_Resize(object sender, EventArgs e)
        {
            CenterToScreen();
        }
    }
}
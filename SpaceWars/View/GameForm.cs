using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceWars.Properties;

namespace SpaceWars
{
    public partial class GameForm : Form
    {
        /// <summary>
        /// The panel that the game ultimately takes place on. Ships, stars, etc. are drawn here.
        /// </summary>
        private WorldPanel _worldPanel;

        /// <summary>
        /// The mp3 player for the background music
        /// </summary>
        private Mp3Player _mp3Player;

        public GameForm()
        {
            InitializeComponent();

            CreateWorldPanel();

            StartMusic();
        }

        /// <summary>
        /// Creates the World Panel that the game is played on.
        /// </summary>
        private void CreateWorldPanel()
        {
            _worldPanel = new WorldPanel
            {
                Margin = new Padding(20),
                Location = new Point(20, 20),
                Size = new Size(750, 750),
                Parent = mainLayoutPanel
            };

            mainLayoutPanel.SetCellPosition(_worldPanel, new TableLayoutPanelCellPosition(0, 0));
        }

        /// <summary>
        /// Plays the background music on a loop.
        /// </summary>
        private void StartMusic()
        {
            _mp3Player = new Mp3Player(Resources.game_music);
            _mp3Player.StartPlaying();
        }

        /// <summary>
        /// Stops the background music.
        /// </summary>
        private void StopMusic()
        {
            _mp3Player.StopPlaying();
        }

        /// <summary>
        /// Loads the main menu when the game window is closed.
        /// </summary>
        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            new MainMenuForm().Show();
            StopMusic();
        }

        /// <summary>
        /// Centers the window onscreen whenever the size is updated (which can only happen automatically, since the user cannot resize the window).
        /// </summary>
        private void GameForm_Resize(object sender, EventArgs e)
        {
            CenterToScreen();
        }
    }
}
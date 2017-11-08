using System;
using System.Drawing;
using System.Windows.Forms;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// The game window for the space wars program, which is displayed once connection to a server has been established.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
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
                Margin = new Padding(10),
                Location = new Point(10, 10),
                Size = new Size(750, 750),
                Parent = _mainLayoutPanel
            };

            _mainLayoutPanel.SetCellPosition(_worldPanel, new TableLayoutPanelCellPosition(0, 0));
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
        /// Centers the window onscreen whenever the size is updated (which can only happen automatically, since the user cannot resize the window).
        /// </summary>
        private void GameForm_Resize(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        /// <summary>
        /// Loads the main menu when the game window is closed.
        /// </summary>
        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            OpenMainMenu();
        }

        /// <summary>
        /// Disconnects from the server and opens the main menu.
        /// </summary>
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            OpenMainMenu();
        }

        /// <summary>
        /// Opens the Main Menu window and disposes this window.
        /// </summary>
        private void OpenMainMenu()
        {
            new MainMenuForm().Show();
            StopMusic();
            Dispose();
        }
    }
}
using System;
using System.Collections.Generic;
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
        /// The Space Wars instance being played.
        /// </summary>
        private readonly SpaceWars _spaceWars;

        /// <summary>
        /// The panel that the game ultimately takes place on. Ships, stars, etc. are drawn here.
        /// </summary>
        private WorldPanel _worldPanel;

        /// <summary>
        /// The panel that the players' scores appear on.
        /// </summary>
        private ScoreboardPanel _scoreboardPanel;

        /// <summary>
        /// The mp3 player for the background music
        /// </summary>
        private Mp3Player _mp3Player;

        /// <summary>
        /// The server may sometimes disconnect unexpectedly. 
        /// This keeps track of whether the disconnect was intentional (by the user clicking disconnect or closing the window).
        /// </summary>
        private bool _disconnectIntended;

        /// <inheritdoc />
        /// <summary>
        /// Creates a new Game Form that is based on the given Space Wars instance.
        /// </summary>
        /// <param name="spaceWars">The connected Space Wars instance.</param>
        public GameForm(SpaceWars spaceWars)
        {
            _spaceWars = spaceWars;

            InitializeComponent();
            InitializeWorldPanel();
            InitializeScoreboardPanel();
            InitializeDisconnectButton();
            _worldPanel.Focus();

            // Controls
            InitializeControls();

            StartMusic();

            // Subscribe to game component changes.
            _spaceWars.GameComponentsUpdated += OnGameComponentsUpdated;

            // Subscribe to connection lost event.
            _spaceWars.ConnectionLost += OnConnectionLost;
        }

        /// <summary>
        /// Called when any game component is updated in the SpaceWars client.
        /// </summary>
        private void OnGameComponentsUpdated()
        {
            // Redraw the game components
            _worldPanel.DrawGameComponents(GetGameComponentsToDraw());
        }

        /// <summary>
        /// Retrieves and returns the game components in the order they should be drawn.
        /// </summary>
        /// <returns>An IEnumerable containing all the components to draw in the order they should be drawn.</returns>
        private IEnumerable<GameComponent> GetGameComponentsToDraw()
        {
            // Draw first
            foreach (var projectile in _spaceWars.Projectiles)
            {
                yield return projectile;
            }

            // Draw second
            foreach (var star in _spaceWars.Stars)
            {
                yield return star;
            }

            // Draw third
            foreach (var ship in _spaceWars.Ships)
            {
                yield return ship;
            }
        }

        /// <summary>
        /// Creates the World Panel that the game is played on.
        /// </summary>
        private void InitializeWorldPanel()
        {
            _worldPanel = new WorldPanel(_spaceWars)
            {
                Margin = new Padding(10),
                Location = new Point(10, 10),
                Size = new Size(750, 750),
                Parent = this
            };
        }

        /// <summary>
        /// Creates the Scoreboard Panel that the players' scores appear on.
        /// </summary>
        private void InitializeScoreboardPanel()
        {
            _scoreboardPanel = new ScoreboardPanel
            {
                Margin = new Padding(10),
                Location = new Point(_worldPanel.Width + 20, 10),
                Size = new Size(300, _worldPanel.Height),
                Parent = this
            };
        }

        /// <summary>
        /// Positions and sizes the disconnect button.
        /// </summary>
        private void InitializeDisconnectButton()
        {
            _disconnectButton.MinimumSize = new Size(_worldPanel.Width + _scoreboardPanel.Width + 10, 80);
            _disconnectButton.Margin = new Padding(10);
            _disconnectButton.Location = new Point(10, _worldPanel.Height + 20);
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
            Disconnect();
        }

        /// <summary>
        /// Disconnects from the server and opens the main menu.
        /// </summary>
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        /// <see cref="OnConnectionLost"/>
        private void Disconnect()
        {
            _disconnectIntended = true;
            _spaceWars.Disconnect();
        }

        /// <summary>
        /// Opens the main menu when the connection to the server has been lost.
        /// </summary>
        /// <see cref="OpenMainMenu"/>
        private void OnConnectionLost()
        {
            Invoke(new MethodInvoker(() =>
            {
                OpenMainMenu();

                // Show a warning dialog that the connection was lost, with a different message depending on if disconnecting was intentional.
                MessageBox.Show(
                    _disconnectIntended
                        ? Resources.GameForm_ConnectionLost_Intended
                        : Resources.GameForm_ConnectionLost_Unexpected,
                    Resources.GameForm_ConnectionLost_Caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }));
        }

        /// <summary>
        /// Opens the Main Menu window and disposes this window.
        /// </summary>
        private void OpenMainMenu()
        {
            StopMusic();

            new MainMenuForm().Show();
            Dispose();
        }
    }
}
using System;
using System.Windows.Forms;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// The main menu of the space wars program.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public partial class MainMenuForm : Form
    {
        /// <summary>
        /// The mp3 player for the background music
        /// </summary>
        private Mp3Player _mp3Player;

        /// <summary>
        /// Determines if we are currently connecting to a server.
        /// </summary>
        private bool Connecting
        {
            get => _connectButton.Text == Resources.MainMenuForm_ConnectButton_Connecting;
            set => _connectButton.Text =
                value
                    ? Resources.MainMenuForm_ConnectButton_Connecting
                    : Resources.MainMenuForm_ConnectButton_Default;
        }

        public MainMenuForm()
        {
            InitializeComponent();

            StartMusic();
        }

        /// <summary>
        /// Plays the background music on a loop.
        /// </summary>
        private void StartMusic()
        {
            _mp3Player = new Mp3Player(Resources.main_menu_music);
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
        /// Attempts to connect to the SpaceWars server with the details entered.
        /// </summary>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void Connect()
        {
            // Make sure we are not connecting to a server.
            if (Connecting)
                return;

            // Make sure there was entry.
            if (_hostNameTextBox.TextLength == 0 || _nicknameTextBox.TextLength == 0)
                return;

            // Attempt connection
            Connecting = true;
            SpaceWarsClientFactory.ConnectToSpaceWars(_hostNameTextBox.Text, _nicknameTextBox.Text,
                ConnectionEstablished,
                ConnectionFailed);
        }

        /// <summary>
        /// Called when a connection to a SpaceWars server was established.
        /// Creates the game form and closes the main menu.
        /// </summary>
        /// <param name="spaceWarsClient">The connected SpaceWars client.</param>
        private void ConnectionEstablished(SpaceWarsClient spaceWarsClient)
        {
            Invoke(new MethodInvoker(() =>
            {
                new GameForm(spaceWarsClient).Show();
                StopMusic();
                Dispose();
            }));
        }

        /// <summary>
        /// Called when a connection to the SpaceWars server has failed.
        /// Displays a warning message dialog.
        /// </summary>
        /// <param name="reason">Why the connection failed.</param>
        private void ConnectionFailed(string reason)
        {
            Invoke(new MethodInvoker(() =>
            {
                Connecting = false;
                MessageBox.Show(Resources.MainMenuForm_ConnectionFailed_Prefix + reason,
                    Resources.MainMenuForm_ConnectionFailed_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }));
        }

        /// <summary>
        /// Stops the application context when this main menu window is closed via the top-right "X" button.
        /// </summary>
        private void MainMenuForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SpaceWarsApplicationContext.Instance.ExitThread();
        }

        /// <summary>
        /// Called when a key is pressed down.
        /// Tries to connect when the enter key is pressed.
        /// </summary>
        private void MainMenuForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Connect();
        }
    }
}
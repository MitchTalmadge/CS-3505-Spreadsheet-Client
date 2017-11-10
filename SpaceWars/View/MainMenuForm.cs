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

        private void ConnectButton_Click(object sender, System.EventArgs e)
        {
            // Attempt to connect to the server (in constructor of SpaceWars)
            try
            {
                new GameForm(new SpaceWars(ServerTextBox.Text, NameTextBox.Text)).Show();
                StopMusic();
                Dispose();
            }
            catch (SpaceWarsConnectionFailedException ex)
            {
                // Connection Failed
                MessageBox.Show(Resources.MainMenuForm_ConnectionFailed_Prefix + ex.Message,
                    Resources.MainMenuForm_ConnectionFailed_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Stops the application context when this main menu window is closed via the top-right "X" button.
        /// </summary>
        private void MainMenuForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SpaceWarsApplicationContext.Instance.ExitThread();
        }
    }
}
using System.Media;
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
        /// The music player for the background music
        /// </summary>
        private SoundPlayer _musicPlayer;

        public MainMenuForm()
        {
            InitializeComponent();

            StartMusic();
        }

        /// <summary>
        /// Plays the main menu background music.
        /// </summary>
        private void StartMusic()
        {
            _musicPlayer = new SoundPlayer(Resources.main_music);
            _musicPlayer.PlayLooping();
        }

        private void ConnectButton_Click(object sender, System.EventArgs e)
        {
            new GameForm().Show();
            Dispose();
        }
    }
}

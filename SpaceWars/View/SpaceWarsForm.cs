using System.Media;
using System.Windows.Forms;
using SpaceWars.Properties;

namespace SpaceWars
{
    public partial class SpaceWarsForm : Form
    {
        private SoundPlayer _musicPlayer;

        public SpaceWarsForm()
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
    }
}

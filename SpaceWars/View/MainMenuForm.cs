using System.Drawing;
using System.Drawing.Text;
using System.Media;
using System.Runtime.InteropServices;
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

            EnableCustomFonts();

            StartMusic();
        }

        /// <summary>
        /// Loads custom fonts into memory for use within the application, then assigns them to the correct labels.
        /// </summary>
        private void EnableCustomFonts()
        {
            // Get font details
            var fontLength = Resources.game_font.Length;
            var fontdata = Resources.game_font;

            // Store font in memory
            var data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontdata, 0, data, fontLength);

            using (var fontCollection = new PrivateFontCollection())
            {
                // Add font from memory to collection
                fontCollection.AddMemoryFont(data, fontLength);

                // Set fonts of labels
                logoLabel.Font = new Font(fontCollection.Families[0], logoLabel.Font.Size);
                ServerAddressLabel.Font = new Font(fontCollection.Families[0], ServerAddressLabel.Font.Size);
                NicknameLabel.Font = new Font(fontCollection.Families[0], NicknameLabel.Font.Size);
                ConnectButton.Font = new Font(fontCollection.Families[0], ConnectButton.Font.Size);
            }
        }

        /// <summary>
        /// Plays the background music on a loop.
        /// </summary>
        private void StartMusic()
        {
            _musicPlayer = new SoundPlayer(Resources.main_music);
            _musicPlayer.PlayLooping();
        }

        /// <summary>
        /// Stops the background music.
        /// </summary>
        private void StopMusic()
        {
            _musicPlayer.Stop();
        }

        private void ConnectButton_Click(object sender, System.EventArgs e)
        {
            new GameForm().Show();
            StopMusic();
            Dispose();
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
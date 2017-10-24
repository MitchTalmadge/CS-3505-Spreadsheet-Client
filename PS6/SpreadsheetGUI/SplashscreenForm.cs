using System.Media;
using System.Threading;
using System.Windows.Forms;
using SpreadsheetGUI.Properties;
using Timer = System.Timers.Timer;

namespace SpreadsheetGUI
{
    /// <inheritdoc />
    /// <summary>
    /// Splashscreen that plays silly music on startup and then closes itself after 4 seconds.
    /// </summary>
    /// <authors>Jiahui Chen and Mitch Talmadge</authors>
    public partial class SplashscreenForm : Form
    {
        public SplashscreenForm()
        {
            InitializeComponent();

            // Play music and close after 4 seconds.
            new Thread(() =>
            {
                new SoundPlayer(Resources.splashscreen_sound).Play();
                Thread.Sleep(4000);
                Invoke((MethodInvoker) Close);
            }).Start();
        }
    }
}
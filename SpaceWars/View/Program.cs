using System;
using System.Windows.Forms;

namespace SpaceWars
{
    internal class SpaceWarsApplicationContext : ApplicationContext
    {
        private static SpaceWarsApplicationContext _instance;

        /// <summary>
        /// The singleton instance of this application context.
        /// </summary>
        internal static SpaceWarsApplicationContext Instance =>
            _instance ?? (_instance = new SpaceWarsApplicationContext());

        private SpaceWarsApplicationContext()
        {
            new MainMenuForm().Show();
        }

    }

    /// <summary>
    /// Starts the space wars program.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(SpaceWarsApplicationContext.Instance);
        }
    }
}

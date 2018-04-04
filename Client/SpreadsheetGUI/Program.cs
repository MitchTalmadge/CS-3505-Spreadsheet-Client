using System;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <inheritdoc />
    /// <summary>
    /// An application context for the Spreadsheet GUI which keeps track of multiple spreadsheet windows.
    /// </summary>
    /// <authors>Jiahui Chen, Tarun Sunkaraneni, Mark Van der Merwe and Mitch Talmadge</authors>
    internal class SpreadsheetApplicationContext : ApplicationContext
    {
        private static SpreadsheetApplicationContext _instance;

        /// <summary>
        /// The singleton instance of this application context.
        /// </summary>
        internal static SpreadsheetApplicationContext Instance =>
            _instance ?? (_instance = new SpreadsheetApplicationContext());

        /// <summary>
        /// The number of currently open spreadsheets.
        /// </summary>
        private int OpenSpreadsheets { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Private constructor to prevent multiple instantiations of the singleton context.
        /// </summary>
        private SpreadsheetApplicationContext()
        {
        }

        /// <summary>
        /// Creates and displays a new spreadsheet in the current application context.
        /// </summary>
        internal void OpenSpreadsheet()
        {
            // Create a spreadsheet
            SpreadsheetForm spreadsheet = new SpreadsheetForm();
            OpenSpreadsheets++;

            // Add a listener for when the spreadsheet form is closed.
            // This also shuts down the application when all spreadsheets are closed.
            spreadsheet.FormClosed += (o, e) =>
            {
                if (--OpenSpreadsheets <= 0) ExitThread();
            };

            // Show the new spreadsheet.
            spreadsheet.Show();
        }
    }

    /// <summary>
    /// The main entry class for the Spreadsheet GUI.
    /// </summary>
    /// <authors>Jiahui Chen and Mitch Talmadge</authors>
    internal static class Program
    {
        /// <summary>
        /// Creates an application context and opens a single spreadsheet.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start splashscreen
            Application.Run(new SplashscreenForm());

            // Open one spreadsheet.
            SpreadsheetApplicationContext.Instance.OpenSpreadsheet();

            // Run the singleton Spreadsheet Application Context
            Application.Run(SpreadsheetApplicationContext.Instance);
        }
    }
}
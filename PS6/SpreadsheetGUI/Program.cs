using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
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
            var spreadsheet = new SpreadsheetForm();
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

    static class Program
    {
        /// <summary>
        /// Creates an application context and opens a single spreadsheet.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Open one spreadsheet.
            SpreadsheetApplicationContext.Instance.OpenSpreadsheet();

            // Run the singleton Spreadsheet Application Context
            Application.Run(SpreadsheetApplicationContext.Instance);
        }
    }
}
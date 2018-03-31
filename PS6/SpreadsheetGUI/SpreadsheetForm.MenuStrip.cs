using System;
using System.Windows.Forms;
using SpreadsheetGUI.Properties;

namespace SpreadsheetGUI
{
    /// <inheritdoc />
    /// <summary>
    /// A partial class of SpreadsheetForm to organize menu strip logic.
    /// </summary>
    /// <authors>Jiahui Chen, Tarun Sunkaraneni, Mark Van der Merwe and Mitch Talmadge</authors>
    public partial class SpreadsheetForm
    {
        /// <summary>
        /// Called when the File -> New menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetApplicationContext.Instance.OpenSpreadsheet();
        }

        /// <summary>
        /// Called when the File -> Open menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSpreadsheet();
        }

        /// <summary>
        /// Called when the File -> Open menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void DisconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisconnectSpreadsheet();
        }

        /// <summary>
        /// Called when the File -> Close menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Called when the Help -> About Spreadsheet menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void AboutSpreadsheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var instructions = Resources.ResourceManager.GetObject("Instructions");
            // ReSharper disable once PossibleNullReferenceException
            MessageBox.Show(instructions.ToString(), Resources.SpreadsheetForm_About_Spreadsheet_Dialog_Caption,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Called when Upgrade -> Professional Version is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void professionalVersionToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start("https://products.office.com/en-us/excel");
        }
    }
}
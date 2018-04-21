using SpreadsheetGUI.Properties;
using System;
using System.Windows.Forms;

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
            OpenSpreadsheet(this.connectedServerTextBox.Text);
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
            DisconnectSpreadsheet();
            Close();
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
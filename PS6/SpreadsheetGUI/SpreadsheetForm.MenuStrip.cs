using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetGUI.Properties;

namespace SpreadsheetGUI
{
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
        /// Called when the File -> Save menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If we know where we opened from, save there. Otherwise act like the "Save As" button.
            if (_openedFilePath != null)
            {
                _spreadsheet.Save(_openedFilePath);
            }
            else
            {
                // Trigger the "File -> Save As" event listener.
                SaveAsToolStripMenuItem_Click(sender, e);
            }
        }

        /// <summary>
        /// Called when the File -> Save As menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Save to specified file
        }

        /// <summary>
        /// Called when the File -> Open menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Open a file
        }

        /// <summary>
        /// Called when the File -> Close menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Close spreadsheet
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
            MessageBox.Show(instructions.ToString(), Resources.SpreadsheetForm_About_Spreadsheet_Dialog_Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
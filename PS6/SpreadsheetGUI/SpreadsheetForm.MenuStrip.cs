using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetGUI.Properties;
using SS;

namespace SpreadsheetGUI
{
    /// <inheritdoc />
    /// <summary>
    /// A partial class of SpreadsheetForm to organize menu strip logic.
    /// </summary>
    /// <authors>Jiahui Chen and Mitch Talmadge</authors>
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
            Save();
        }

        /// <summary>
        /// Called when the File -> Save As menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        /// <summary>
        /// Saves the spreadsheet to the most recently opened or saved file path. 
        /// If no file was opened/saved before, or chooseFile is true, asks the user to select a place to save.
        /// </summary>
        /// <param name="chooseFile">True if the user should choose a file to save to.</param>
        private void Save(bool chooseFile = false)
        {
            // Save to the currently open file (if we have saved/opened before)
            if (!chooseFile && _openedFilePath != null)
            {
                _spreadsheet.Save(_openedFilePath);
            }
            else
            {
                // File was not opened or has not been saved before.

                // Ask the user to select a file to save to.
                var fileDialog = new SaveFileDialog {Filter = Resources.SpreadsheetForm_File_Extension_Filter};

                // If no file was chosen (cancelled) then return.
                if (fileDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                // Save the spreadsheet and record its path.
                var filePath = fileDialog.FileName;
                _spreadsheet.Save(filePath);
                _openedFilePath = filePath;
            }
        }

        /// <summary>
        /// Called when the File -> Open menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        /// <summary>
        /// Opens a spreadsheet file from the File Explorer method.
        /// </summary>
        private void Open()
        {
            //opens menu for user to choose file to open
            var fileDialogue = new OpenFileDialog { Filter = Resources.SpreadsheetForm_File_Extension_Filter };

            //if no file was opened, then return.
            if (fileDialogue.ShowDialog(this) != DialogResult.OK)
                return;

            //saves current spreadsheet, clears current contents, then open new spreadsheet in current window
            if (Changes() == true)
            {
                spreadsheetPanel.Clear();
                editorContentTextBox.Clear();
                editorValueTextBox.Clear();
                editorNameTextBox.Clear();
                string filepath = fileDialogue.FileName;
                _spreadsheet = new Spreadsheet(filepath, IsValid, Normalize, SpreadsheetVersion);
            }
        }

        /// <summary>
        /// Called when the File -> Close menu item is clicked.
        /// </summary>
        /// <param name="sender">The menu item clicked.</param>
        /// <param name="e">A click event.</param>
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Changes() == true) {
                // Close the window.
                Close();
            }            
        }

        /// <summary>
        /// Helper method for opening and closing spreadsheet files.
        /// Saves changes in current spreadsheet before closing current 
        /// spreadsheet or opening a new spreadsheet.
        /// </summary>
        private bool Changes()
        {
            // Allow the user to save changes first.
            if (_spreadsheet.Changed)
            {
                var result = MessageBox.Show(Resources.SpreadsheetForm_Unsaved_Changes_Text,
                    Resources.SpreadsheetForm_Unsaved_Changes_Caption,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3);

                switch (result)
                {
                    case DialogResult.Yes:
                        Save();
                        break;
                    case DialogResult.Cancel:
                        // Don't do anything if we cancel.
                        return false;
                }
            }
            return true;
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
    }
}
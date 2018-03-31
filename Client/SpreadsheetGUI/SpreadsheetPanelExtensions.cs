using SS;

namespace SpreadsheetGUI
{
    internal static class SpreadsheetPanelExtensions
    {
        /// <summary>
        /// Selects the cell above the current cell.
        /// </summary>
        public static void MoveSelectionUp(this SpreadsheetPanel panel)
        {
            panel.GetSelection(out var col, out var row);
            if (row > 0)
                panel.SetSelection(col, --row);
        }

        /// <summary>
        /// Selects the cell below the current cell.
        /// </summary>
        public static void MoveSelectionDown(this SpreadsheetPanel panel)
        {
            panel.GetSelection(out var col, out var row);
            if (row < 98)
                panel.SetSelection(col, ++row);
        }

        /// <summary>
        /// Selects the cell to the left of the current cell.
        /// </summary>
        public static void MoveSelectionLeft(this SpreadsheetPanel panel)
        {
            panel.GetSelection(out var col, out var row);
            if (col > 0)
                panel.SetSelection(--col, row);
        }

        /// <summary>
        /// Selects the cell below the current cell.
        /// </summary>
        public static void MoveSelectionRight(this SpreadsheetPanel panel)
        {
            panel.GetSelection(out var col, out var row);
            if (col < 25)
                panel.SetSelection(++col, row);
        }
    }
}
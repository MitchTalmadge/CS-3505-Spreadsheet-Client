Jiahui Chen
u0980890
Mitch Talmadge
u1031378

For our spreadsheet, our extra features are:
	- The ability to Save and Save As. If the current file has been opened from a previously saved file
	  or has already been saved to a file, the Save button under the file menu will save it to this 
	  location without opening the file-choosing dialog.
    - A fun splashscreen (turn on your speakers!)
	- There is a "Professional Version" which can be accessed through the "Upgrade" menu's "Professional Version" 
	  option.

A design decision we made was to split up the listener methods and functionality of the menu strip, cell editor, and spreadsheet panel
portions of the GUI into partial classes, so that we did not have one massive SpreadsheetForm.cs class. We kept related helper methods
in these partial classes as well.
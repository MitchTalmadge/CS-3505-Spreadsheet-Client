﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpreadsheetGUI.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SpreadsheetGUI.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Spreadsheet 3500
        ///~~~~~~~~~~~~~~~~
        ///Designed by Jiahui Chen (u0980890) and Mitch Talmadge (u1031378)
        ///
        ///INSTRUCTIONS:
        ///
        ///Menu Bar
        ///--------------------------------------------------
        ///- Creating a new Spreadsheet:
        ///    - Click on the File -&gt; New menu item, which will create a brand new (unsaved) spreadsheet in a new window.
        ///
        ///- Saving a Spreadsheet:
        ///    - There are two ways to save a Spreadsheet: &quot;Save&quot; and &quot;Save As&quot;.
        ///        - Save: If the spreadsheet has been opened from an existing file, or has been sa [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Instructions {
            get {
                return ResourceManager.GetString("Instructions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap splashscreen_image {
            get {
                object obj = ResourceManager.GetObject("splashscreen_image", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        /// </summary>
        internal static System.IO.UnmanagedMemoryStream splashscreen_sound {
            get {
                return ResourceManager.GetStream("splashscreen_sound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to About Spreadsheet.
        /// </summary>
        internal static string SpreadsheetForm_About_Spreadsheet_Dialog_Caption {
            get {
                return ResourceManager.GetString("SpreadsheetForm_About_Spreadsheet_Dialog_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Spreadsheet files (*.sprd)|*.sprd|All files (*.*)|*.*.
        /// </summary>
        internal static string SpreadsheetForm_File_Extension_Filter {
            get {
                return ResourceManager.GetString("SpreadsheetForm_File_Extension_Filter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to N O !.
        /// </summary>
        internal static string SpreadsheetForm_Formula_Error_Value {
            get {
                return ResourceManager.GetString("SpreadsheetForm_Formula_Error_Value", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A circular dependency was found!.
        /// </summary>
        internal static string SpreadsheetForm_inputTextBox_Circular_Dependency {
            get {
                return ResourceManager.GetString("SpreadsheetForm_inputTextBox_Circular_Dependency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid Cell Input.
        /// </summary>
        internal static string SpreadsheetForm_inputTextBox_Invalid_Cell_Input {
            get {
                return ResourceManager.GetString("SpreadsheetForm_inputTextBox_Invalid_Cell_Input", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name of the cell is invalid!.
        /// </summary>
        internal static string SpreadsheetForm_inputTextBox_Invalid_Cell_Name {
            get {
                return ResourceManager.GetString("SpreadsheetForm_inputTextBox_Invalid_Cell_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The syntax of the formula entered is invalid!.
        /// </summary>
        internal static string SpreadsheetForm_inputTextBox_Invalid_Formula {
            get {
                return ResourceManager.GetString("SpreadsheetForm_inputTextBox_Invalid_Formula", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file that you have selected is not a valid spreadsheet..
        /// </summary>
        internal static string SpreadsheetForm_OpenSpreadsheet_LoadFailMessage {
            get {
                return ResourceManager.GetString("SpreadsheetForm_OpenSpreadsheet_LoadFailMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot load Spreadsheet.
        /// </summary>
        internal static string SpreadsheetForm_OpenSpreadsheet_LoadFailTitle {
            get {
                return ResourceManager.GetString("SpreadsheetForm_OpenSpreadsheet_LoadFailTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  - Spreadsheet 3500.
        /// </summary>
        internal static string SpreadsheetForm_Title_Suffix {
            get {
                return ResourceManager.GetString("SpreadsheetForm_Title_Suffix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsaved Changes.
        /// </summary>
        internal static string SpreadsheetForm_Unsaved_Changes_Caption {
            get {
                return ResourceManager.GetString("SpreadsheetForm_Unsaved_Changes_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to save your changes to the spreadsheet?.
        /// </summary>
        internal static string SpreadsheetForm_Unsaved_Changes_Text {
            get {
                return ResourceManager.GetString("SpreadsheetForm_Unsaved_Changes_Text", resourceCulture);
            }
        }
    }
}

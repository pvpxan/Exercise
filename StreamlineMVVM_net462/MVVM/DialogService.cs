using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    public enum WindowMessageResult
    {
        Undefined,
        Yes,
        No,
        Ok,
        Continue,
        Cancel,
        Exit,
        Accept,
        Decline,
        Error,
        Misc1,
        Misc2,
        Misc3,
    }

    public enum WindowMessageButtons
    {
        Default,
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel,
        Exit,
        ContinueCancel,
        AcceptDecline,
        Misc,
    }

    public enum WindowMessageIcon
    {
        Application,
        Asterisk,
        Error,
        Exclamation,
        Hand,
        Information,
        Question,
        Shield,
        Warning,
        WinLogo
    }

    public enum WindowButtonFocus
    {
        Left,
        Center,
        Right,
    }

    // Makes it easier to change this for later.
    public static class ColorSets
    {
        public static Brush Background { get; } = Brushes.White;

        public static Brush ContentHeaderColor { get; } = Brushes.DarkBlue;
        public static Brush ContentBodyColor { get; } = Brushes.Black;

        public static Brush HyperLinkColor { get; } = Brushes.Blue;
        public static Brush HyperLinkMouseOverColor { get; } = Brushes.Red;
        public static Brush HyperLinkMouseDisabledColor { get; } = Brushes.Gray;

        // If your Hex string is not well formed, this will return black.
        public static Brush HexConverter(string hexCode)
        {
            Brush brush = Brushes.Black;

            if (string.IsNullOrEmpty(hexCode) || ValidateHexCode(hexCode) == false)
            {
                return brush;
            }

            var converter = new BrushConverter();
            try
            {
                brush = (Brush)converter.ConvertFromString(hexCode);
            }
            catch
            {
                brush = Brushes.Black;
            }

            return brush;
        }

        public static bool ValidateHexCode(string hexCode)
        {
            if (string.IsNullOrEmpty(hexCode) || hexCode[0] != '#')
            {
                return false;
            }

            return int.TryParse(hexCode.Trim('#'), System.Globalization.NumberStyles.HexNumber, null, out int hexValue);
        }
    }

    public class MiscWindowsMessageButtons
    {
        public string Misc1 { get; set; } = "";
        public string Misc2 { get; set; } = "";
        public string Misc3 { get; set; } = "";
    }

    public class DialogData
    {
        // These if present are used to determine what window this dialog should open under if model.
        public Window ParentWindow { get; set; } = null;
        public ViewModelBase ParentViewModelBase { get; set; } = null;
        // ------------------------

        public string WindowTitle { get; set; } = "";
        public Brush Background { get; set; } = ColorSets.Background;
        public bool Topmost { get; set; } = true;
        public WindowStyle DialogWindowStyle { get; set; } = WindowStyle.ToolWindow;
        public WindowStartupLocation DialogStartupLocation { get; set; } = WindowStartupLocation.CenterOwner;
        public string WindowIconURI { get; set; } = "";

        public bool RequireResult { get; set; } = false;
        public bool CancelAsync { get; set; } = false;

        public Brush ContentHeaderColor { get; set; } = ColorSets.ContentHeaderColor;
        public string ContentHeader { get; set; } = "";

        public Brush ContentBodyColor { get; set; } = ColorSets.ContentBodyColor;
        public string ContentBody { get; set; } = "";

        public Brush HyperLinkColor { get; set; } = ColorSets.HyperLinkColor;
        public Brush HyperLinkMouseOverColor { get; set; } = ColorSets.HyperLinkMouseOverColor;
        public Brush HyperLinkMouseDisabledColor { get; set; } = ColorSets.HyperLinkMouseDisabledColor;
        public string HyperLinkText { get; set; } = "";
        public string HyperLinkUri { get; set; } = "";

        public WindowMessageIcon MessageIcon { get; set; } = WindowMessageIcon.Information;
        public WindowMessageButtons MessageButtons { get; set; } = WindowMessageButtons.Ok;
        public WindowButtonFocus MessageButtonFocus { get; set; } = WindowButtonFocus.Center;
        public MiscWindowsMessageButtons MiscButtoms { get; set; } = new MiscWindowsMessageButtons();

        // Pass Args to Dialog
        public object Parameter1 { get; set; } = null;
        public object Parameter2 { get; set; } = null;

        // Control to Focus on launch
        public UIElement FocusUIElement { get; set; } = null;
    }

    public static class DialogService
    {
        // When this class is first accessed, we need to know what access level it has.
        private static bool checkApplicationUIThreadAccess()
        {
            try
            {
                return Application.Current.Dispatcher.CheckAccess();
            }
            catch
            {
                return false;
            }
        }

        public static ShutdownMode CurrentApplicationShutdownMode()
        {
            try
            {
                if (checkApplicationUIThreadAccess())
                {
                    return Application.Current.ShutdownMode;
                }
                
                return ShutdownMode.OnLastWindowClose;
            }
            catch
            {
                return ShutdownMode.OnLastWindowClose;
            }
        }

        // Takes a ViewModel based user control and opens a window with that control as content. Provides the ability to set Application object ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogBaseWindowViewModel viewModel, UserControl userControl , Window parentWindow, ShutdownMode shutdownMode)
        {
            bool applicationUIThreadAccess = checkApplicationUIThreadAccess();
            if (applicationUIThreadAccess)
            {
                // Param shutdownMode can used to prevent the Application.Current from going null. It can be turned off by setting ApplicationExplicitShutdown.
                // This should not throw. Possible investigation should be made to check on this.
                Application.Current.ShutdownMode = shutdownMode;
            }

            // Create new instance of the DialogBaseWindow with the viewModel.
            DialogBaseWindow dialogBaseWindow = new DialogBaseWindow(viewModel, userControl);

            // We need to find a dispatcher to display this message.
            Dispatcher dispatcher = null;
            if (parentWindow == null && applicationUIThreadAccess == false)
            {
                return WindowMessageResult.Undefined;
            }
            else if (parentWindow == null && applicationUIThreadAccess)
            {
                try
                {
                    dialogBaseWindow.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
                }
                catch
                {
                    // This should not throw. Possible investigation should be made to check on this.
                    if (Application.Current.Windows.Count > 0)
                    {
                        dialogBaseWindow.Owner = Application.Current.Windows[0];
                    }
                    else
                    {
                        dialogBaseWindow.Owner = null;
                    }
                }

                if (dialogBaseWindow.Owner == null)
                {
                    dispatcher = Application.Current.Dispatcher;
                }
                else
                {
                    dispatcher = dialogBaseWindow.Owner.Dispatcher;
                }
            }
            else // This should ONLY happen if parentWindow is NOT null.
            {
                dialogBaseWindow.Owner = parentWindow;
                dispatcher = parentWindow.Dispatcher;
            }

            try
            {
                if (dispatcher.CheckAccess() == false)
                {
                    return WindowMessageResult.Undefined;
                }

                WindowMessageResult result = WindowMessageResult.Undefined;
                dispatcher.Invoke((Action)delegate
                {
                    dialogBaseWindow.ShowDialog();

                    result = (dialogBaseWindow.DataContext as DialogBaseWindowViewModel).UserDialogResult;
                });

                return result;
            }
            catch
            {
                return WindowMessageResult.Undefined;
            }
        }

        // OpenDialog with default Application Object ShutdownMode set to the current ShutdownMode
        public static WindowMessageResult OpenDialog(DialogBaseWindowViewModel viewModel, UserControl userControl, Window parentWindow)
        {
            return OpenDialog(viewModel, userControl, parentWindow, CurrentApplicationShutdownMode());
        }

        // OpenDialog with a null parent window object and passed in ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogBaseWindowViewModel viewModel, UserControl userControl, ShutdownMode shutdownMode)
        {
            return OpenDialog(viewModel, userControl, null, shutdownMode);
        }

        // OpenDialog with no parent window and a default Application Objects ShutdownMode set to the current ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogBaseWindowViewModel viewModel, UserControl userControl)
        {
            return OpenDialog(viewModel, userControl, null, CurrentApplicationShutdownMode());
        }
    }
}

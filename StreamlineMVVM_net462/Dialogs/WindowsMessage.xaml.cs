using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    /// <summary>
    /// Interaction logic for WindowsMessage.xaml
    /// </summary>
    public partial class WindowsMessage : UserControl
    {
        public WindowsMessage(DialogBaseWindowViewModel dialogBaseWindowViewModel)
        {
            try
            {
                this.DataContext = dialogBaseWindowViewModel;
                InitializeComponent();

                switch (dialogBaseWindowViewModel.ViewModelDialogData.MessageButtons)
                {
                    case WindowMessageButtons.AcceptDecline:
                    case WindowMessageButtons.ContinueCancel:
                    case WindowMessageButtons.OkCancel:
                    case WindowMessageButtons.YesNo:
                        if (dialogBaseWindowViewModel.ViewModelDialogData.MessageButtonFocus == WindowButtonFocus.Center)
                        {
                            dialogBaseWindowViewModel.ViewModelDialogData.MessageButtonFocus = WindowButtonFocus.Left;
                        }
                        break;

                    case WindowMessageButtons.Default:
                    case WindowMessageButtons.Exit:
                    case WindowMessageButtons.Ok:
                        if (dialogBaseWindowViewModel.ViewModelDialogData.MessageButtonFocus == WindowButtonFocus.Left ||
                            dialogBaseWindowViewModel.ViewModelDialogData.MessageButtonFocus == WindowButtonFocus.Right)
                        {
                            dialogBaseWindowViewModel.ViewModelDialogData.MessageButtonFocus = WindowButtonFocus.Center;
                        }
                        break;

                    case WindowMessageButtons.YesNoCancel:
                        // TODO(DB): Determine if there needs to be logic here.
                        break;

                    case WindowMessageButtons.Misc:
                        // TODO(DB): Determine if there needs to be logic here.
                        break;

                    default:
                        // TODO(DB): Determine if there needs to be logic here.
                        break;
                }

                switch (dialogBaseWindowViewModel.ViewModelDialogData.MessageButtonFocus)
                {
                    case WindowButtonFocus.Left:
                        dialogBaseWindowViewModel.ViewModelDialogData.FocusUIElement = leftButton;
                        break;

                    case WindowButtonFocus.Center:
                        dialogBaseWindowViewModel.ViewModelDialogData.FocusUIElement = centerButton;
                        break;

                    case WindowButtonFocus.Right:
                        dialogBaseWindowViewModel.ViewModelDialogData.FocusUIElement = rightButton;
                        break;
                }
            }
            catch
            {
                // TODO (DB): Create some sort of error log in a default windows directory.
            }
        }

        private void hyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            using (Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)))
            {
                e.Handled = true;
            }
        }
    }

    public class WindowsMessageViewModel : DialogBaseWindowViewModel
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // TODO (DB): There might be some functionality added that requires this section.
        // ---------------------------------------------------------------------------------------------------------------------------------------------

        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public WindowsMessageViewModel(DialogData data) : base(data)
        {
            MessageIcon = getIcon(data.MessageIcon);
            ContentHeader = data.ContentHeader;

            if (data.ContentBody.Length > 0)
            {
                ContentBodyVisibility = Visibility.Visible;
                ContentBody = data.ContentBody;
            }

            if (data.HyperLinkUri.Length > 0)
            {
                HyperLinkIsEnabled = true;
                HyperLinkVisibility = Visibility.Visible;

                HyperLinkUri = data.HyperLinkUri;

                if (data.HyperLinkText.Length > 0)
                {
                    HyperLinkText = data.HyperLinkText;
                }
                else
                {
                    HyperLinkText = data.HyperLinkUri;
                }
            }

            // Subscribes to the Loaded/Rendered Event of the DialogBaseWindow.
            // OnContentLoaded += windowContentLoaded; // Currently nothing is loading so will save this for potential future additions.
            OnContentRendered += windowContentRendered;

            switch (data.MessageButtons)
            {
                case WindowMessageButtons.Default:

                    break;

                case WindowMessageButtons.Ok:

                    CenterContent = "Ok";
                    CenterIsEnabled = true;
                    CenterVisibility = Visibility.Visible;

                    CenterButton = new RelayCommand(okCommand);

                    break;

                case WindowMessageButtons.OkCancel:

                    LeftContent = "Ok";
                    LeftIsEnabled = true;
                    LeftVisibility = Visibility.Visible;

                    LeftButton = new RelayCommand(okCommand);

                    RightContent = "Cancel";
                    RightIsEnabled = true;
                    RightVisibility = Visibility.Visible;

                    RightButton = new RelayCommand(cancelCommand);

                    break;

                case WindowMessageButtons.YesNo:

                    LeftContent = "Yes";
                    LeftIsEnabled = true;
                    LeftVisibility = Visibility.Visible;

                    LeftButton = new RelayCommand(yesCommand);

                    RightContent = "No";
                    RightIsEnabled = true;
                    RightVisibility = Visibility.Visible;

                    RightButton = new RelayCommand(noCommand);

                    break;

                case WindowMessageButtons.YesNoCancel:

                    LeftContent = "Yes";
                    LeftIsEnabled = true;
                    LeftVisibility = Visibility.Visible;

                    LeftButton = new RelayCommand(yesCommand);

                    CenterContent = "No";
                    CenterIsEnabled = true;
                    CenterVisibility = Visibility.Visible;

                    CenterButton = new RelayCommand(noCommand);

                    RightContent = "Cancel";
                    RightIsEnabled = true;
                    RightVisibility = Visibility.Visible;

                    RightButton = new RelayCommand(cancelCommand);

                    break;

                case WindowMessageButtons.Exit:

                    CenterContent = "Exit";
                    CenterIsEnabled = true;
                    CenterVisibility = Visibility.Visible;

                    CenterButton = new RelayCommand(exitCommand);

                    break;

                case WindowMessageButtons.ContinueCancel:

                    LeftContent = "Continue";
                    LeftIsEnabled = true;
                    LeftVisibility = Visibility.Visible;

                    LeftButton = new RelayCommand(continueCommand);

                    RightContent = "Cancel";
                    RightIsEnabled = true;
                    RightVisibility = Visibility.Visible;

                    RightButton = new RelayCommand(cancelCommand);

                    break;

                case WindowMessageButtons.AcceptDecline:

                    LeftContent = "Accept";
                    LeftIsEnabled = true;
                    LeftVisibility = Visibility.Visible;

                    LeftButton = new RelayCommand(acceptCommand);

                    RightContent = "Decline";
                    RightIsEnabled = true;
                    RightVisibility = Visibility.Visible;

                    RightButton = new RelayCommand(declineCommand);

                    break;

                case WindowMessageButtons.Misc:

                    if (string.IsNullOrEmpty(data.MiscButtoms.Misc1) == false)
                    {
                        LeftContent = data.MiscButtoms.Misc1;
                        LeftIsEnabled = true;
                        LeftVisibility = Visibility.Visible;

                        LeftButton = new RelayCommand(misc1Command);
                    }
                    else
                    {
                        if (data.MessageButtonFocus == WindowButtonFocus.Left)
                        {
                            data.MessageButtonFocus = WindowButtonFocus.Center;
                        }
                    }

                    if (string.IsNullOrEmpty(data.MiscButtoms.Misc2) == false)
                    {
                        CenterContent = data.MiscButtoms.Misc2;
                        CenterIsEnabled = true;
                        CenterVisibility = Visibility.Visible;

                        CenterButton = new RelayCommand(custom2Command);
                    }
                    else
                    {
                        if (data.MessageButtonFocus == WindowButtonFocus.Center)
                        {
                            data.MessageButtonFocus = WindowButtonFocus.Right;
                        }
                    }

                    if (string.IsNullOrEmpty(data.MiscButtoms.Misc3) == false)
                    {
                        RightContent = data.MiscButtoms.Misc3;
                        RightIsEnabled = true;
                        RightVisibility = Visibility.Visible;

                        RightButton = new RelayCommand(custom3Command);
                    }
                    else
                    {
                        if (data.MessageButtonFocus == WindowButtonFocus.Right)
                        {
                            data.MessageButtonFocus = WindowButtonFocus.Left;
                        }
                    }

                    break;
            }
        }

        private BitmapSource getIcon(WindowMessageIcon icontype)
        {
            try
            {
                Icon icon = (Icon)typeof(SystemIcons).GetProperty(Convert.ToString(icontype), BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                // TODO (DB): This probably does not need to have anything here.
                return null;
            }
        }

        private void windowContentLoaded(object sender, RoutedEventArgs e)
        {
            //if (CurrentDialogBaseWindow == null)
            //{
            //    return;
            //}

            //TODO (DB): Possible add some loaded events.
        }

        private void windowContentRendered(object sender, EventArgs e)
        {
            if (CurrentDialogBaseWindow == null)
            {
                return;
            }

            try
            {
                Dispatcher dispatcher = CurrentDialogBaseWindow.Dispatcher;
                if (dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
                }
            }
            catch
            {
                // TODO (DB): This probably does not need to have anything here.
            }
        }

        // Bound Variables
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        private BitmapSource _MessageIcon = null;
        public BitmapSource MessageIcon
        {
            get { return _MessageIcon; }
            set
            {
                _MessageIcon = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MessageIcon"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Media.Brush _ContentHeaderColor = System.Windows.Media.Brushes.DarkBlue;
        public System.Windows.Media.Brush ContentHeaderColor
        {
            get { return _ContentHeaderColor; }
            set
            {
                _ContentHeaderColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentHeaderColor"));
            }
        }

        // -----------------------------------------------------
        private string _ContentHeader = "";
        public string ContentHeader
        {
            get { return _ContentHeader; }
            set
            {
                _ContentHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentHeader"));
            }
        }

        // -----------------------------------------------------
        private Visibility _ContentBodyVisibility = Visibility.Collapsed;
        public Visibility ContentBodyVisibility
        {
            get { return _ContentBodyVisibility; }
            set
            {
                _ContentBodyVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentBodyVisibility"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Media.Brush _ContentBodyColor = System.Windows.Media.Brushes.Black;
        public System.Windows.Media.Brush ContentBodyColor
        {
            get { return _ContentBodyColor; }
            set
            {
                _ContentBodyColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentBodyColor"));
            }
        }

        // -----------------------------------------------------
        private string _ContentBody = "";
        public string ContentBody
        {
            get { return _ContentBody; }
            set
            {
                _ContentBody = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentBody"));
            }
        }

        // -----------------------------------------------------
        private bool _HyperLinkIsEnabled = false;
        public bool HyperLinkIsEnabled
        {
            get { return _HyperLinkIsEnabled; }
            set
            {
                _HyperLinkIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _HyperLinkVisibility = Visibility.Collapsed;
        public Visibility HyperLinkVisibility
        {
            get { return _HyperLinkVisibility; }
            set
            {
                _HyperLinkVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkVisibility"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Media.Brush _HyperLinkColor = System.Windows.Media.Brushes.Blue;
        public System.Windows.Media.Brush HyperLinkColor
        {
            get { return _HyperLinkColor; }
            set
            {
                _HyperLinkColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkColor"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Media.Brush _HyperLinkMouseOverColor = System.Windows.Media.Brushes.Red;
        public System.Windows.Media.Brush HyperLinkMouseOverColor
        {
            get { return _HyperLinkMouseOverColor; }
            set
            {
                _HyperLinkMouseOverColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkMouseOverColor"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Media.Brush _HyperLinkMouseDisabledColor = System.Windows.Media.Brushes.Gray;
        public System.Windows.Media.Brush HyperLinkMouseDisabledColor
        {
            get { return _HyperLinkMouseDisabledColor; }
            set
            {
                _HyperLinkMouseDisabledColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkMouseDisabledColor"));
            }
        }

        // -----------------------------------------------------
        private string _HyperLinkUri = "";
        public string HyperLinkUri
        {
            get { return _HyperLinkUri; }
            set
            {
                _HyperLinkUri = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkUri"));
            }
        }

        // -----------------------------------------------------
        private string _HyperLinkText = "";
        public string HyperLinkText
        {
            get { return _HyperLinkText; }
            set
            {
                _HyperLinkText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkText"));
            }
        }

        // -----------------------------------------------------
        private string _LeftContent = "";
        public string LeftContent
        {
            get { return _LeftContent; }
            set
            {
                _LeftContent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeftContent"));
            }
        }

        // -----------------------------------------------------
        private bool _LeftIsEnabled = false;
        public bool LeftIsEnabled
        {
            get { return _LeftIsEnabled; }
            set
            {
                _LeftIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeftIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _LeftVisibility = Visibility.Hidden;
        public Visibility LeftVisibility
        {
            get { return _LeftVisibility; }
            set
            {
                _LeftVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeftVisibility"));
            }
        }

        // -----------------------------------------------------
        private string _CenterContent = "";
        public string CenterContent
        {
            get { return _CenterContent; }
            set
            {
                _CenterContent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CenterContent"));
            }
        }

        // -----------------------------------------------------
        private bool _CenterIsEnabled = false;
        public bool CenterIsEnabled
        {
            get { return _CenterIsEnabled; }
            set
            {
                _CenterIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CenterIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _CenterVisibility = Visibility.Hidden;
        public Visibility CenterVisibility
        {
            get { return _CenterVisibility; }
            set
            {
                _CenterVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CenterVisibility"));
            }
        }

        // -----------------------------------------------------
        private string _RightContent = "";
        public string RightContent
        {
            get { return _RightContent; }
            set
            {
                _RightContent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RightContent"));
            }
        }

        // -----------------------------------------------------
        private bool _RightIsEnabled = false;
        public bool RightIsEnabled
        {
            get { return _RightIsEnabled; }
            set
            {
                _RightIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RightIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _RightVisibility = Visibility.Hidden;
        public Visibility RightVisibility
        {
            get { return _RightVisibility; }
            set
            {
                _RightVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RightVisibility"));
            }
        }

        // Bound Commands
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        public ICommand LeftButton { get; private set; }
        public ICommand CenterButton { get; private set; }
        public ICommand RightButton { get; private set; }

        // Special Command Handling
        // ------------------------------------------------------
        private void okCommand(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Ok);
        }

        private void cancelCommand(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Cancel);
        }

        private void yesCommand(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Yes);
        }

        private void noCommand(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.No);
        }

        private void exitCommand(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Exit);
        }

        private void continueCommand(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Continue);
        }

        private void acceptCommand(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Accept);
        }

        private void declineCommand(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Decline);
        }

        private void misc1Command(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Misc1);
        }

        private void custom2Command(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Misc2);
        }

        private void custom3Command(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Misc3);
        }
    }
}

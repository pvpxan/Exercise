using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    /// <summary>
    /// Interaction logic for DialogBaseWindow.xaml
    /// </summary>
    public partial class DialogBaseWindow : Window
    {
        private DialogBaseWindowViewModel windowViewModel = null;

        public DialogBaseWindow(DialogBaseWindowViewModel viewModel, UserControl userControl)
        {
            try
            {
                this.DataContext = viewModel;
                windowViewModel = viewModel;
                bool setViewModelDialogBaseWindow = viewModel.SetDialogBaseWindow(this);

                InitializeComponent();

                try
                {
                    // These are some conditions that are needed for everything to work correctly.
                    if (userControl == null ||
                        userControl.DataContext != viewModel ||
                        setViewModelDialogBaseWindow == false)
                    {
                        // NOTE: This will cause the following suppressed error that only shows in debugger output:
                        // 'System.InvalidOperationException' in PresentationFramework.dll
                        // This happens since the window is already closed before a Show() or ShowDialog() method can actually do anything.
                        this.Close();
                        return;
                    }
                }
                catch
                {
                    // TODO (DB): Think about logging this if necessary.
                }

                try
                {
                    if (viewModel.ViewModelDialogData.WindowIconURI.Length > 0)
                    {
                        Icon = BitmapFrame.Create(new Uri(viewModel.ViewModelDialogData.WindowIconURI, UriKind.RelativeOrAbsolute));
                    }
                }
                catch
                {
                    // TODO (DB):  Find a way to extract the application icon and assign it.
                }

                this.dialogBaseWindowGrid.Children.Add(userControl);
                FocusElementControl(viewModel.ViewModelDialogData.FocusUIElement);

                // General Window properties. Not bound since some are not technically content and one is not able to be bound.
                WindowStartupLocation = viewModel.ViewModelDialogData.DialogStartupLocation; // Cannot be bound since it is a DependencyProperty
                WindowStyle = viewModel.ViewModelDialogData.DialogWindowStyle;
                Topmost = viewModel.ViewModelDialogData.Topmost;
                Title = viewModel.ViewModelDialogData.WindowTitle;
                Background = viewModel.ViewModelDialogData.Background;

                Loaded += contentLoaded;
                ContentRendered += contentRendered;
                Closing += windowClosing;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Window load error: " + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void contentLoaded(object sender, RoutedEventArgs e)
        {
            windowViewModel.ContentLoaded(sender, e);
        }

        private void contentRendered(object sender, EventArgs e)
        {
            windowViewModel.ContentRendered(sender, e);
        }

        private void windowClosing(object sender, CancelEventArgs e)
        {
            if (windowViewModel.RequireResult &&
                windowViewModel.UserDialogResult == WindowMessageResult.Undefined)
            {
                e.Cancel = true;
                return;
            }

            // HACK: Cleans up bitmapimage of icon.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void FocusElementControl(UIElement uiElement)
        {
            KeyboardHelper.Focus(uiElement, this.Dispatcher, DispatcherPriority.Render);
        }

        // Ugly, but necessary.
        public void CloseWithResult()
        {
            try
            {
                this.Dispatcher.Invoke((Action)delegate
                {
                    // HACK: There might be occations and I do not know how, but the window no longer is considered modal or what not.
                    bool isThisModal = false;
                    try
                    {
                        isThisModal = (bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
                    }
                    catch
                    {
                        // TODO (DB): Think about logging this if necessary.
                    }

                    if (isThisModal)
                    {
                        try
                        {
                            DialogResult = true;
                        }
                        catch
                        {
                            // TODO (DB): Think about logging this if necessary.
                            this.Close();
                        }
                    }
                    else
                    {
                        try
                        {
                            DialogResult = (DialogResult == true) ? true : (bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
                            return;
                        }
                        catch
                        {
                            // TODO (DB): Think about logging this if necessary.
                            this.Close();
                        }
                    }
                });
            }
            catch
            {
                // TODO (DB): Think about logging this if necessary.
            }
        }
    }

    public class DialogBaseWindowViewModel : ViewModelBase
    {
        public DialogData ViewModelDialogData { get; private set; } = null;

        public bool RequireResult { get; private set; } = false;
        public bool CancelAsync { get; private set; } = false;

        public WindowMessageResult UserDialogResult { get; set; } = WindowMessageResult.Undefined;

        // This window object sort of pushes the bounds of MVVM.
        public DialogBaseWindow CurrentDialogBaseWindow { get; private set; } = null;
        public bool SetDialogBaseWindow(DialogBaseWindow dialogBaseWindow)
        {
            if (dialogBaseWindow == null)
            {
                return false;
            }

            if (dialogBaseWindow.DataContext != this)
            {
                return false;
            }

            CurrentDialogBaseWindow = dialogBaseWindow;
            return true;
        }

        public Action<object, RoutedEventArgs> OnContentLoaded { get; set; } = null;
        public void ContentLoaded(object sender, RoutedEventArgs e)
        {
            if (OnContentLoaded != null)
            {
                OnContentLoaded(sender, e);
            }
        }

        public Action<object, EventArgs> OnContentRendered { get; set; } = null;
        public void ContentRendered(object sender, EventArgs e)
        {
            if (OnContentRendered != null)
            {
                OnContentRendered(sender, e);
            }
        }

        public DialogBaseWindowViewModel(DialogData data)
        {
            ViewModelDialogData = data;

            RequireResult = data.RequireResult;
            CancelAsync = data.CancelAsync;
        }

        public void CloseDialogWithResult(WindowMessageResult result)
        {
            CancelAsync = true; // If method uses this to control running of async threads, this will force it to close when the window closes.
            UserDialogResult = result;

            if (CurrentDialogBaseWindow == null)
            {
                return;
            }

            CurrentDialogBaseWindow.CloseWithResult();
        }

        // Handles Window closing.
        public void CloseWindow()
        {
            CurrentDialogBaseWindow.CloseWithResult();
        }
    }
}

using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace oloCateringExercise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Statics.Initialization();
            this.DataContext = new MainWindowViewModel();
        }
    }

    public static class Statics
    {
        public static Version CurrentFileVersion { get; } = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
        public static string CurrentUser { get; } = Environment.UserName.ToLower();
        public static string[] Args { get; } = Environment.GetCommandLineArgs();
        public static string ProgramPath { get; } = getProgramPath();

        public static void Initialization()
        {
            LogWriter.SetPath(ProgramPath, CurrentUser, "CatPersonSearcher");
        }

        private static string getProgramPath()
        {
            try
            {
                if (AppDomain.CurrentDomain.BaseDirectory[AppDomain.CurrentDomain.BaseDirectory.Length - 1] == '\\')
                {
                    return AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
                }
                else
                {
                    return Environment.CurrentDirectory;
                }
            }
            catch
            {
                // Doubt this will ever happen.
                return "";
            }
        }
    }
}

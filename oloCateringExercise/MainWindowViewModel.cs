using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using StreamlineMVVM;

namespace oloCateringExercise
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Generate = new RelayCommand(generateCommand);
        }

        // -----------------------------------------------------
        public ICommand Generate { get; private set; }
        private void generateCommand(object parameter)
        {
            if (ProgressBarVisibility == Visibility.Visible)
            {
                return;
            }

            MenuResults.Clear();
            ProgressBarVisibility = Visibility.Visible;

            MenuReader menuReader = new MenuReader();
            menuReader.ReadComplete = populateData;
            menuReader.ReadMenu(MenuAddress);
        }

        private void populateData(WebMenu webMenu)
        {
            MenuResults = new ObservableCollection<MenuItem>(webMenu.MenuItems);
            ProgressBarVisibility = Visibility.Hidden;
        }

        // -----------------------------------------------------
        private ObservableCollection<MenuItem> _MenuResults = new ObservableCollection<MenuItem>();
        public ObservableCollection<MenuItem> MenuResults
        {
            get { return _MenuResults; }
            set
            {
                _MenuResults = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MenuResults"));
            }
        }

        // -----------------------------------------------------
        private string _MenuAddress = "https://www.olo.com/menu.json";
        public string MenuAddress
        {
            get { return _MenuAddress; }
            set
            {
                _MenuAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MenuAddress"));
            }
        }

        // -----------------------------------------------------
        private Visibility _ProgressBarVisibility = Visibility.Hidden;
        public Visibility ProgressBarVisibility
        {
            get { return _ProgressBarVisibility; }
            set
            {
                _ProgressBarVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressBarVisibility"));
            }
        }
    }
}

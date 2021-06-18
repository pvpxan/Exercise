using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

// https://www.technical-recipes.com/2017/how-to-use-interaction-triggers-to-handle-user-initiated-events-in-wpf-mvvm/
// https://www.technical-recipes.com/2016/using-relaycommand-icommand-to-handle-events-in-wpf-and-mvvm/

namespace StreamlineMVVM
{
    // START MVVM RelayCommand Class ----------------------------------------------------------------------------------------------------
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> predicateExecute;
        private readonly Action<T> actionExecute;

        public RelayCommand(Action<T> execute) : this(execute, null)
        {
            actionExecute = execute;
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            actionExecute = execute;
            predicateExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return predicateExecute == null || predicateExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            actionExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> predicateExecute;
        private readonly Action<object> actionExecute;

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
            actionExecute = execute;
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            actionExecute = execute;
            predicateExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return predicateExecute == null || predicateExecute(parameter);
        }

        public void Execute(object parameter)
        {
            actionExecute(parameter);
        }

        // Ensures WPF commanding infrastructure asks all RelayCommand objects whether their
        // associated views should be enabled whenever a command is invoked 
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        private event EventHandler CanExecuteChangedInternal;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChangedInternal.Raise(this);
        }
    }

    public static class EventRaiser
    {
        public static void Raise(this EventHandler handler, object sender)
        {
            handler?.Invoke(sender, EventArgs.Empty);
        }

        public static void Raise<T>(this EventHandler<EventArgs<T>> handler, object sender, T value)
        {
            handler?.Invoke(sender, new EventArgs<T>(value));
        }

        public static void Raise<T>(this EventHandler<T> handler, object sender, T value) where T : EventArgs
        {
            handler?.Invoke(sender, value);
        }

        public static void Raise<T>(this EventHandler<EventArgs<T>> handler, object sender, EventArgs<T> value)
        {
            handler?.Invoke(sender, value);
        }
    }

    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
    // END MVVM RelayCommand Class ------------------------------------------------------------------------------------------------------
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    public class DataMessaging
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(DialogData data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error triggering DataMessaging event.", Ex);
            }
        }

        public Action<DialogData> OnDataTransmittedEvent { get; set; }
    }

    public class TextAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(string data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error triggering Text Aggregator event.", Ex);
            }
        }

        public Action<string> OnDataTransmittedEvent { get; set; }
    }

    public class IntAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(int data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error triggering Int Aggregator event.", Ex);
            }
        }

        public Action<int> OnDataTransmittedEvent { get; set; }
    }

    public class BoolAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(bool data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error triggering Bool Aggregator event.", Ex);
            }
        }

        public Action<bool> OnDataTransmittedEvent { get; set; }
    }

    public class IInputElementAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(IInputElement data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error triggering IInputElement Aggregator event.", Ex);
            }
        }

        public Action<IInputElement> OnDataTransmittedEvent { get; set; }
    }

    public class UIElementAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(UIElement data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher == null)
                {
                    OnDataTransmittedEvent(data);
                    return;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            catch
            {

            }
        }

        public Action<UIElement> OnDataTransmittedEvent { get; set; }
    }

    public class ObjectAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(object data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error triggering Object Aggregator event.", Ex);
            }
        }

        public Action<object> OnDataTransmittedEvent { get; set; }
    }
}

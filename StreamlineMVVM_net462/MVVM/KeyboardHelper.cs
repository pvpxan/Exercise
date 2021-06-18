using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    // Thanks https://github.com/punker76
    // Thanks https://stackoverflow.com/questions/5400570/how-to-make-checkbox-focus-border-appear-when-calling-checkbox-focus/5401707#5401707
    // This is a Hack used to simulate the dotted line around a control when tab navigation is used.
    public sealed class KeyboardHelper
    {
        private static KeyboardHelper _Instance;

        private readonly PropertyInfo _AlwaysShowFocusVisual;
        private readonly MethodInfo _ShowFocusVisual;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static KeyboardHelper()
        {
        }

        private KeyboardHelper()
        {
            var type = typeof(KeyboardNavigation);

            _AlwaysShowFocusVisual = type.GetProperty("AlwaysShowFocusVisual", BindingFlags.NonPublic | BindingFlags.Static);
            _ShowFocusVisual = type.GetMethod("ShowFocusVisual", BindingFlags.NonPublic | BindingFlags.Static);
        }

        internal static KeyboardHelper Instance => _Instance ?? (_Instance = new KeyboardHelper());

        internal void ShowFocusVisualInternal()
        {
            _ShowFocusVisual.Invoke(null, null);
        }

        internal bool AlwaysShowFocusVisualInternal
        {
            get { return (bool)_AlwaysShowFocusVisual.GetValue(null, null); }
            set { _AlwaysShowFocusVisual.SetValue(null, value, null); }
        }

        public static void Focus(UIElement uiElement, Dispatcher dispatcher, DispatcherPriority dispatcherPriority)
        {
            if (dispatcher == null || uiElement == null)
            {
                return;
            }

            try
            {
                dispatcher.BeginInvoke(new Action(() =>
                {
                    var keyboardHack = KeyboardHelper.Instance;
                    var oldValue = keyboardHack.AlwaysShowFocusVisualInternal;

                    keyboardHack.AlwaysShowFocusVisualInternal = true;

                    try
                    {
                        Keyboard.Focus(uiElement);
                        keyboardHack.ShowFocusVisualInternal();
                    }
                    finally
                    {
                        keyboardHack.AlwaysShowFocusVisualInternal = oldValue;
                    }
                }), dispatcherPriority);
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error attempting to run Keyboard Focus method.", Ex);
            }
        }
    }
}

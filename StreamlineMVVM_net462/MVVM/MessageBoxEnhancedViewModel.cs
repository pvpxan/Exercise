using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamlineMVVM
{
    public static partial class MessageBoxEnhanced
    {
        // These extra overloads are only good if ViewModels are registered with the Factory class.

        // Sets Window Title and Message Content on target window.
        public static WindowMessageResult Show(ViewModelBase viewModelBase, string windowTitle, string contentHeader)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader), viewModelBase);
        }

        // Sets Window Title, Message Content, Message Buttons, and Message Icon on target window.
        public static WindowMessageResult Show(ViewModelBase viewModelBase, string windowTitle, string contentHeader, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, "", "", "", windowMessageButtons, windowMessageIcon), viewModelBase);
        }

        // Sets Window Title, Message Header, Message Body, Message Buttons, and Message Icon on target window.
        public static WindowMessageResult Show(ViewModelBase viewModelBase, string windowTitle, string contentHeader, string contentBody, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, "", "", windowMessageButtons, windowMessageIcon), viewModelBase);
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink, Message Buttons, and Message Icon on target window.
        public static WindowMessageResult Show(ViewModelBase viewModelBase, string windowTitle, string contentHeader, string contentBody, string hyperLink, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, "", hyperLink, windowMessageButtons, windowMessageIcon), viewModelBase);
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink Text, Message HyperLink Uri, Message Buttons, and Message Icon on target window.
        public static WindowMessageResult Show(ViewModelBase viewModelBase, string windowTitle, string contentHeader, string contentBody, string hyperLinkText, string hyperLinkUri, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, hyperLinkText, hyperLinkUri, windowMessageButtons, windowMessageIcon), viewModelBase);
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink Text, Message HyperLink Uri, Message Buttons, Message Icon, and Color Set on target window.
        public static WindowMessageResult Show(ViewModelBase viewModelBase, string windowTitle, string contentHeader, string contentBody, string hyperLinkText, string hyperLinkUri, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon, WindowMessageColorSet windowMessageColorSet)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, hyperLinkText, hyperLinkUri, windowMessageButtons, windowMessageIcon), viewModelBase);
        }
    }
}

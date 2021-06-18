using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamlineMVVM
{
    public static partial class MessageBoxEnhanced
    {
        // Basic set of overload methdods for the Show method.

        // Sets Window Title and Message Content.
        public static WindowMessageResult Show(string windowTitle, string contentHeader)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader));
        }

        // Sets Window Title, Message Content, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, "", "", "", windowMessageButtons, windowMessageIcon));
        }

        // Sets Window Title, Message Header, Message Body, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, "", "", windowMessageButtons, windowMessageIcon));
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, string hyperLink, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, "", hyperLink, windowMessageButtons, windowMessageIcon));
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink Text, Message HyperLink Uri, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, string hyperLinkText, string hyperLinkUri, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, hyperLinkText, hyperLinkUri, windowMessageButtons, windowMessageIcon));
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink Text, Message HyperLink Uri, Message Buttons, Message Icon, and Color Set.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, string hyperLinkText, string hyperLinkUri, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon, WindowMessageColorSet windowMessageColorSet)
        {
            return OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, hyperLinkText, hyperLinkUri, windowMessageButtons, windowMessageIcon));
        }
    }
}

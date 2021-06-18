using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StreamlineMVVM
{
    // Using these requires a reference in the XAML:
    // xmlns:ext="clr-namespace:StreamlineMVVM;assembly=StreamlineMVVM"

    public class Extensions : DependencyObject
    {
        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter",
            typeof(object),
            typeof(Extensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        [AttachedPropertyBrowsableForChildren]
        public static void SetCommandParameter(DependencyObject depObject, object value) { depObject.SetValue(CommandProperty, value); }
        public static object GetCommandParameter(DependencyObject depObject) { return depObject.GetValue(CommandProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(Extensions),
            new FrameworkPropertyMetadata(defaultICommand, FrameworkPropertyMetadataOptions.None));
        [AttachedPropertyBrowsableForChildren]
        public static void SetCommand(DependencyObject depObject, ICommand value) { depObject.SetValue(CommandProperty, value); }
        public static ICommand GetCommand(DependencyObject depObject) { return (ICommand)depObject.GetValue(CommandProperty); }
        private static ICommand defaultICommand { get; set; } = new RelayCommand(defaultICommandCommand);
        private static void defaultICommandCommand(object parameter)
        {
            // Does nothing
        }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius",
            typeof(double),
            typeof(Extensions),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetCornerRadius(UIElement element, double value) { element.SetValue(CornerRadiusProperty, value); }
        public static double GetCornerRadius(UIElement element) { return (double)element.GetValue(CornerRadiusProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty FocusThicknessProperty = DependencyProperty.RegisterAttached(
            "FocusThickness",
            typeof(double),
            typeof(Extensions),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetFocusThickness(UIElement element, double value) { element.SetValue(FocusThicknessProperty, value); }
        public static double GetFocusThickness(UIElement element) { return (double)element.GetValue(FocusThicknessProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty FocusBrushProperty = DependencyProperty.RegisterAttached(
            "FocusBrush",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetFocusBrush(UIElement element, SolidColorBrush value) { element.SetValue(FocusBrushProperty, value); }
        public static SolidColorBrush GetFocusBrush(UIElement element) { return (SolidColorBrush)element.GetValue(FocusBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty FocusBorderProperty = DependencyProperty.RegisterAttached(
            "FocusBorder",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetFocusBorder(UIElement element, SolidColorBrush value) { element.SetValue(FocusBorderProperty, value); }
        public static SolidColorBrush GetFocusBorder(UIElement element) { return (SolidColorBrush)element.GetValue(FocusBorderProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.RegisterAttached(
            "MouseOverBackground",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetMouseOverBackground(UIElement element, SolidColorBrush value) { element.SetValue(MouseOverBackgroundProperty, value); }
        public static SolidColorBrush GetMouseOverBackground(UIElement element) { return (SolidColorBrush)element.GetValue(MouseOverBackgroundProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty MouseOverBorderProperty = DependencyProperty.RegisterAttached(
            "MouseOverBorder",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetMouseOverBorder(UIElement element, SolidColorBrush value) { element.SetValue(MouseOverBorderProperty, value); }
        public static SolidColorBrush GetMouseOverBorder(UIElement element) { return (SolidColorBrush)element.GetValue(MouseOverBorderProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty MouseDownBackgroundProperty = DependencyProperty.RegisterAttached(
            "MouseDownBackground",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetMouseDownBackground(UIElement element, SolidColorBrush value) { element.SetValue(MouseDownBackgroundProperty, value); }
        public static SolidColorBrush GetMouseDownBackground(UIElement element) { return (SolidColorBrush)element.GetValue(MouseDownBackgroundProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty MouseDownBorderProperty = DependencyProperty.RegisterAttached(
            "MouseDownBorder",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetMouseDownBorder(UIElement element, SolidColorBrush value) { element.SetValue(MouseDownBorderProperty, value); }
        public static SolidColorBrush GetMouseDownBorder(UIElement element) { return (SolidColorBrush)element.GetValue(MouseDownBorderProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty SelectionActiveBackgroundProperty = DependencyProperty.RegisterAttached(
            "SelectionActiveBackground",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetSelectionActiveBackground(UIElement element, SolidColorBrush value) { element.SetValue(SelectionActiveBackgroundProperty, value); }
        public static SolidColorBrush GetSelectionActiveBackground(UIElement element) { return (SolidColorBrush)element.GetValue(SelectionActiveBackgroundProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty SelectionActiveBorderProperty = DependencyProperty.RegisterAttached(
            "SelectionActiveBorder",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetSelectionActiveBorder(UIElement element, SolidColorBrush value) { element.SetValue(SelectionActiveBorderProperty, value); }
        public static SolidColorBrush GetSelectionActiveBorder(UIElement element) { return (SolidColorBrush)element.GetValue(SelectionActiveBorderProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty SelectionInactiveBackgroundProperty = DependencyProperty.RegisterAttached(
            "SelectionInactiveBackground",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetSelectionInactiveBackground(UIElement element, SolidColorBrush value) { element.SetValue(SelectionInactiveBackgroundProperty, value); }
        public static SolidColorBrush GetSelectionInactiveBackground(UIElement element) { return (SolidColorBrush)element.GetValue(SelectionInactiveBackgroundProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty SelectionInactiveBorderProperty = DependencyProperty.RegisterAttached(
            "SelectionInactiveBorder",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetSelectionInactiveBorder(UIElement element, SolidColorBrush value) { element.SetValue(SelectionInactiveBorderProperty, value); }
        public static SolidColorBrush GetSelectionInactiveBorder(UIElement element) { return (SolidColorBrush)element.GetValue(SelectionInactiveBorderProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.RegisterAttached(
            "DisabledBackground",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetDisabledBackground(UIElement element, SolidColorBrush value) { element.SetValue(DisabledBackgroundProperty, value); }
        public static SolidColorBrush GetDisabledBackground(UIElement element) { return (SolidColorBrush)element.GetValue(DisabledBackgroundProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty DisabledBorderProperty = DependencyProperty.RegisterAttached(
            "DisabledBorder",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetDisabledBorder(UIElement element, SolidColorBrush value) { element.SetValue(DisabledBorderProperty, value); }
        public static SolidColorBrush GetDisabledBorder(UIElement element) { return (SolidColorBrush)element.GetValue(DisabledBorderProperty); }

        // ----------------------------------------------------------------------------------------------------
        public enum TextBoxOverlayType
        {
            OverlayGlyph,
            OverlayText,
        }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty TextBoxOverlayProperty = DependencyProperty.Register(
            "TextBoxOverlay",
            typeof(TextBoxOverlayType),
            typeof(Extensions),
            new FrameworkPropertyMetadata(TextBoxOverlayType.OverlayGlyph, FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetTextBoxOverlay(UIElement element, TextBoxOverlayType value) { element.SetValue(TextBoxOverlayProperty, value); }
        public static TextBoxOverlayType GetTextBoxOverlay(UIElement element) { return (TextBoxOverlayType)element.GetValue(TextBoxOverlayProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ContentTextProperty = DependencyProperty.Register(
            "ContentText",
            typeof(string),
            typeof(Extensions),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetContentText(UIElement element, string value) { element.SetValue(ContentTextProperty, value); }
        public static string GetContentText(UIElement element) { return (string)element.GetValue(ContentTextProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.RegisterAttached(
            "ImageSource",
            typeof(BitmapImage),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new BitmapImage(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetImageSource(UIElement element, BitmapImage value) { element.SetValue(ImageSourceProperty, value); }
        public static BitmapImage GetImageSource(UIElement element) { return (BitmapImage)element.GetValue(ImageSourceProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ImageSourcePathProperty = DependencyProperty.Register(
            "ImageSourcePath",
            typeof(string),
            typeof(Extensions),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetImageSourcePath(UIElement element, string value) { element.SetValue(ImageSourcePathProperty, value); }
        public static string GetImageSourcePath(UIElement element) { return (string)element.GetValue(ImageSourcePathProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.RegisterAttached(
            "ImageWidth",
            typeof(double),
            typeof(Extensions),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetImageWidth(UIElement element, double value) { element.SetValue(ImageWidthProperty, value); }
        public static double GetImageWidth(UIElement element) { return (double)element.GetValue(ImageWidthProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.RegisterAttached(
            "ImageHeight",
            typeof(double),
            typeof(Extensions),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetImageHeight(UIElement element, double value) { element.SetValue(ImageHeightProperty, value); }
        public static double GetImageHeight(UIElement element) { return (double)element.GetValue(ImageHeightProperty); }
    }

    public class ComboBoxExt : DependencyObject
    {
        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty DropDownBackgroundBrushProperty = DependencyProperty.RegisterAttached(
            "DropDownBackgroundBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetDropDownBackgroundBrush(UIElement element, SolidColorBrush value) { element.SetValue(DropDownBackgroundBrushProperty, value); }
        public static SolidColorBrush GetDropDownBackgroundBrush(UIElement element) { return (SolidColorBrush)element.GetValue(DropDownBackgroundBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty DropDownBorderBrushProperty = DependencyProperty.RegisterAttached(
            "DropDownBorderBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetDropDownBorderBrush(UIElement element, SolidColorBrush value) { element.SetValue(DropDownBorderBrushProperty, value); }
        public static SolidColorBrush GetDropDownBorderBrush(UIElement element) { return (SolidColorBrush)element.GetValue(DropDownBorderBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty DropDownMouseOverBrushProperty = DependencyProperty.RegisterAttached(
            "DropDownMouseOverBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SeDropDownMouseOverBrush(UIElement element, SolidColorBrush value) { element.SetValue(DropDownMouseOverBrushProperty, value); }
        public static SolidColorBrush GetDropDownMouseOverBrush(UIElement element) { return (SolidColorBrush)element.GetValue(DropDownMouseOverBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty DropDownSelectedBrushProperty = DependencyProperty.RegisterAttached(
            "DropDownSelectedBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetDropDownSelectedBrush(UIElement element, SolidColorBrush value) { element.SetValue(DropDownSelectedBrushProperty, value); }
        public static SolidColorBrush GetDropDownSelectedBrush(UIElement element) { return (SolidColorBrush)element.GetValue(DropDownSelectedBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleBarBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleBarBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleBarBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleBarBrushProperty, value); }
        public static SolidColorBrush GetToggleBarBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleBarBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleBarMouseOverBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleBarMouseOverBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleBarMouseOverBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleBarMouseOverBrushProperty, value); }
        public static SolidColorBrush GetToggleBarMouseOverBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleBarMouseOverBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleBarPressedBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleBarPressedBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleBarPressedBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleBarPressedBrushProperty, value); }
        public static SolidColorBrush GetToggleBarPressedBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleBarPressedBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleBarDisabledBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleBarDisabledBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleBarDisabledBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleBarDisabledBrushProperty, value); }
        public static SolidColorBrush GetToggleBarDisabledBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleBarDisabledBrushProperty); }

        // ----------------------------------------------------------------------------------------------------`
        public static readonly DependencyProperty ToggleButtonBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleButtonBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleButtonBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleButtonBrushProperty, value); }
        public static SolidColorBrush GetToggleButtonBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleButtonBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleButtonMouseOverBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleButtonMouseOverBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleButtonMouseOverBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleButtonMouseOverBrushProperty, value); }
        public static SolidColorBrush GetToggleButtonMouseOverBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleButtonMouseOverBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleButtonPressedBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleButtonPressedBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleButtonPressedBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleButtonPressedBrushProperty, value); }
        public static SolidColorBrush GetToggleButtonPressedBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleButtonPressedBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleButtonDisabledBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleButtonDisabledBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleButtonDisabledBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleButtonDisabledBrushProperty, value); }
        public static SolidColorBrush GetToggleButtonDisabledBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleButtonDisabledBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleGlyphBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleGlyphBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleGlyphBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleGlyphBrushProperty, value); }
        public static SolidColorBrush GetToggleGlyphBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleGlyphBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleGlyphMouseOverBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleGlyphMouseOverBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleGlyphMouseOverBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleGlyphMouseOverBrushProperty, value); }
        public static SolidColorBrush GetToggleGlyphMouseOverBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleGlyphMouseOverBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleGlyphPressedBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleGlyphPressedBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleGlyphPressedBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleGlyphPressedBrushProperty, value); }
        public static SolidColorBrush GetToggleGlyphPressedBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleGlyphPressedBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ToggleGlyphDisabledBrushProperty = DependencyProperty.RegisterAttached(
            "ToggleGlyphDisabledBrush",
            typeof(SolidColorBrush),
            typeof(ComboBoxExt),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetToggleGlyphDisabledBrush(UIElement element, SolidColorBrush value) { element.SetValue(ToggleGlyphDisabledBrushProperty, value); }
        public static SolidColorBrush GetToggleGlyphDisabledBrush(UIElement element) { return (SolidColorBrush)element.GetValue(ToggleGlyphDisabledBrushProperty); }
    }
}

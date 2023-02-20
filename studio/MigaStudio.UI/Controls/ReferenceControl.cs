using Wpf.Ui.Common;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class ReferenceControl : Control
    {
        static ReferenceControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReferenceControl), new FrameworkPropertyMetadata(typeof(ReferenceControl)));
        }


        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
            nameof(DisplayName),
            typeof(string),
            typeof(ReferenceControl),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(string),
            typeof(ReferenceControl),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(SymbolRegular),
            typeof(ReferenceControl),
            new PropertyMetadata(default(SymbolRegular)));

        public SymbolRegular Icon
        {
            get => (SymbolRegular)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        public string DisplayName
        {
            get => (string)GetValue(DisplayNameProperty);
            set => SetValue(DisplayNameProperty, value);
        }
    }
}
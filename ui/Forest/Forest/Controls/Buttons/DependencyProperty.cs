namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    #region ForestButton

    
    public abstract partial class ForestIconButtonBase : ForestButtonBase, IForestIconControl
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;

        static ForestIconButtonBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(Boxing.False));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(Boxing.False));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestIconButtonBase)d).HasIcon = e.NewValue is Geometry;
        }

        public bool HasIcon
        {
            get => (bool)GetValue(HasIconProperty);
            private set => SetValue(HasIconProperty, value);
        }

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public bool IsFilled
        {
            get => (bool)GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, Boxing.Box(value));
        }

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }

    partial class ForestButtonBase
    {
        public static readonly DependencyProperty PaletteProperty;
        public static readonly DependencyProperty CornerRadiusProperty;

        static ForestButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestButtonBase), new FrameworkPropertyMetadata(typeof(ForestButtonBase)));
            
            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestButtonBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));
            
            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestButtonBase),
                new PropertyMetadata(default(CornerRadius)));
        }

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IForestControl)d).InvalidateState();
        }

        public HighlightColorPalette Palette
        {
            get => (HighlightColorPalette)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
    
    #endregion
}
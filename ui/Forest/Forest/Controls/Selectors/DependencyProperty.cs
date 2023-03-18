namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    #region ForestToggleButton

    public abstract partial class ForestListBoxItemBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;
        public static readonly DependencyProperty    PlacementProperty;

        static ForestListBoxItemBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(Boxing.False));

            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));

            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(default(CornerRadius)));

            PlacementProperty = DependencyProperty.Register(
                nameof(Placement),
                typeof(Dock),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(Dock.Left));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestListBoxItemBase), new FrameworkPropertyMetadata(typeof(ForestListBoxItemBase)));
        }



        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestListBoxItemBase)d).InvalidateState();
        }
        
        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestListBoxItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestListBoxItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        public Dock Placement
        {
            get => (Dock)GetValue(PlacementProperty);
            private set => SetValue(PlacementProperty, value);
        }

        public bool HasIcon
        {
            get => (bool)GetValue(HasIconProperty);
            private set => SetValue(HasIconPropertyKey, value);
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

        public static readonly DependencyProperty PaletteProperty;
        public static readonly DependencyProperty CornerRadiusProperty;

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
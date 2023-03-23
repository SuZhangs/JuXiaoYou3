using System.Windows.Shapes;

namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    public class ActionButton : Button
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty; 
        public static readonly DependencyProperty    CornerRadiusProperty;


        static ActionButton()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ActionButton),
                new PropertyMetadata(default(Geometry)));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ActionButton),
                new PropertyMetadata(Boxing.False));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ActionButton),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ActionButton),
                new PropertyMetadata(Boxing.False));

            
            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ActionButton),
                new PropertyMetadata(default(CornerRadius)));
            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActionButton), new FrameworkPropertyMetadata(typeof(ActionButton)));
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
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
    }
}
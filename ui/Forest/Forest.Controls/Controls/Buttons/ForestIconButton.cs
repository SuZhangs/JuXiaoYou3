using System.Windows;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    public class ForestIconButton : ForestButtonBase
    {
        static ForestIconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestIconButton), new FrameworkPropertyMetadata(typeof(ForestIconButton)));
        }
        
        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(Geometry),
            typeof(ForestIconButton),
            new PropertyMetadata(default(Geometry)));


        public static readonly DependencyProperty IsFilledProperty = DependencyProperty.Register(
            nameof(IsFilled),
            typeof(bool),
            typeof(ForestIconButton),
            new PropertyMetadata(Boxing.False));


        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            nameof(IconSize),
            typeof(double),
            typeof(ForestIconButton),
            new PropertyMetadata(17d));

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
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Controls.Renderers;

namespace StoryDiagram.Controls
{
    public class StoryboardCanvas : Control
    {
        public static readonly DependencyProperty GridlineProperty = DependencyProperty.Register(
            nameof(Gridline),
            typeof(Brush),
            typeof(StoryboardCanvas),
            new PropertyMetadata(new SolidColorBrush(Colors.Gray), OnGridlineChanged));

        private static readonly SolidColorBrush DefaultBrush;

        static StoryboardCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StoryboardCanvas), new FrameworkPropertyMetadata(typeof(StoryboardCanvas)));
            DefaultBrush = new SolidColorBrush(Colors.Gray);
        }

        private static void OnGridlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b  = e.NewValue as Brush;
            var sc = (StoryboardCanvas)d;
            sc.CreateGridlinePen(b);
        }

        private Pen _GridlinePen;

        private void CreateGridlinePen(Brush brush)
        {
            _GridlinePen = new Pen(brush ?? DefaultBrush, 1);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.PushGuidelineSet(Xaml.GuidelineSet);

            //
            // 绘制
            this.Normalize(out var w, out var h);
            drawingContext.DrawBackground(w, h, Background)
                          .DrawGridline(w, h, _GridlinePen, 40);
            
            drawingContext.Pop();
            base.OnRender(drawingContext);
        }

        public Brush Gridline
        {
            get => (Brush)GetValue(GridlineProperty);
            set => SetValue(GridlineProperty, value);
        }
    }
}
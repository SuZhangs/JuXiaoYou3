using System.Windows.Documents;

namespace Acorisoft.FutureGL.Forest.Adorners
{

    public class ThumbAdorner : Adorner
    {
        private static readonly SolidColorBrush Brush = new SolidColorBrush(Colors.Red);
        private static readonly Pen             Pen   = new Pen(Brush, 1);

        public ThumbAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }

        private static void DrawCorner(DrawingContext drawingContext, Point pos)
        {
            drawingContext.DrawRectangle(Brush, null, new Rect(pos.X - 2, pos.Y - 2, 4, 4));
        }

        private static void DrawAllCorner(DrawingContext drawingContext, double width, double height)
        {
            DrawCorner(drawingContext, new Point(0, 0));
            DrawCorner(drawingContext, new Point(width, 0));
            DrawCorner(drawingContext, new Point(0, height));
            DrawCorner(drawingContext, new Point(width, height));

            DrawCorner(drawingContext, new Point(width / 2, 0));
            DrawCorner(drawingContext, new Point(0, height / 2));
            DrawCorner(drawingContext, new Point(width, height / 2));
            DrawCorner(drawingContext, new Point(width / 2, height));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var actualHeight = AdornedElement.RenderSize.Height;
            var actualWidth = AdornedElement.RenderSize.Width;
            drawingContext.PushGuidelineSet(Xaml.GuidelineSet);
            drawingContext.DrawRectangle(null, Pen, new Rect(0, 0, actualWidth, actualHeight));
            drawingContext.Pop();
            DrawAllCorner(drawingContext, actualWidth, actualHeight);
            base.OnRender(drawingContext);
        }
    }
}
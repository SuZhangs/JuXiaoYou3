using Acorisoft.FutureGL.MigaStudio.Controls.Models;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class HistogramChart : PolarControl
    {
        static HistogramChart()
        {
            ForegroundProperty.AddOwner(typeof(HistogramChart))
                .OverrideMetadata(typeof(HistogramChart),
                    new FrameworkPropertyMetadata(OnForegroundChanged));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HistogramChart), new FrameworkPropertyMetadata(typeof(HistogramChart)));

            DataProperty = DependencyProperty.Register(
                nameof(Data),
                typeof(HistogramChartDataSet),
                typeof(HistogramChart),
                new PropertyMetadata(default(HistogramChartDataSet), InvalidateVisual));
        }
        
        protected static void InvalidateVisual(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (HistogramChart)d;
            radar.InvalidateVisual();
        }


        private void RenderingAxis(DrawingContext drawingContext)
        {
            var height = ActualHeight;
            var width = ActualWidth;
            var pen = new Pen(DefaultBrush, 1);
            
            RenderingAxis(drawingContext, pen, height, width, 0.0d);
            RenderingAxis(drawingContext, pen, height, width, 0.2d);
            RenderingAxis(drawingContext, pen, height, width, 0.4d);
            RenderingAxis(drawingContext, pen, height, width, 0.6d);
            RenderingAxis(drawingContext, pen, height, width, 0.8d);
        }

        private static void RenderingAxis(DrawingContext drawingContext,Pen pen, double height, double width, double percent)
        {
            var height2 = height - height * percent;
            var rect = new Rect(0, height2, width, .5d);
            drawingContext.DrawRectangle(null, pen, rect);
            
        }
        
        
        private Brush _foreground;

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (HistogramChart)d;

            radar._foreground = radar.Foreground ?? DefaultBrush;
            radar.InvalidateVisual();
        }

        public HistogramChart()
        {
            this.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            PushGuideline(drawingContext);

            //
            //
            RenderingAxis(drawingContext);

            //
            //
            RenderData(drawingContext);

            //
            //
            drawingContext.Pop();
            base.OnRender(drawingContext);
        }

        private void RenderData(DrawingContext drawingContext)
        {
            var dataChart = Data is null || Data.Count == 0 ? DefaultHistogramData : Data;
            var max = dataChart.Maximum;
            var height = ActualHeight;
            var height2 = height - 30;
            var width = ActualWidth - (dataChart.Count - 1) * 10;
            var chartWidth = (int)Math.Clamp(width / dataChart.Count, 5d, 11d);
            var x = 0d;

            foreach (var chart in dataChart)
            {
                var chartHeight = chart.Value / max * height2;
                drawingContext.DrawRectangle(
                    new SolidColorBrush(Xaml.FromHex(chart.Color)), 
                    null,
                    new Rect(x, height - chartHeight, chartWidth, chartHeight));
                x += (chartWidth + 10);
            }
        }

        public static readonly DependencyProperty DataProperty;

        public HistogramChartDataSet Data
        {
            get => (HistogramChartDataSet)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}
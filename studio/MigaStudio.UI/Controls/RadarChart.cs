using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using Acorisoft.FutureGL.MigaStudio.Controls.Models;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class RadarChart : PolarControl
    {
        struct AxisData
        {
            internal double[]        values;
            internal Pen             pen;
            internal SolidColorBrush bg;
        }

        private Typeface _typeface;
        private Brush    _foreground;

        static RadarChart()
        {
            ForegroundProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnForegroundChanged));

            FontSizeProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnTypefaceChanged));

            FontFamilyProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnTypefaceChanged));


            FontWeightProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnTypefaceChanged));

            FontStretchProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnTypefaceChanged));

            FontStyleProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnTypefaceChanged));
            
            
            DataProperty = DependencyProperty.Register(
                nameof(Data),
                typeof(ChartDataSet),
                typeof(RadarChart),
                new FrameworkPropertyMetadata(default(ChartDataSet), FrameworkPropertyMetadataOptions.AffectsRender, InvalidateVisual));

            AxesProperty = DependencyProperty.Register(
                nameof(Axes),
                typeof(AxisCollection),
                typeof(RadarChart),
                new FrameworkPropertyMetadata(default(AxisCollection), FrameworkPropertyMetadataOptions.AffectsRender, InvalidateVisual));

            ShowAxisNameProperty = DependencyProperty.Register(
                nameof(ShowAxisName),
                typeof(bool),
                typeof(RadarChart),
                new FrameworkPropertyMetadata(Boxing.True, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (RadarChart)d;

            radar._foreground = radar.Foreground ?? DefaultBrush;
            radar.InvalidateVisual();
        }

        private static void OnTypefaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (RadarChart)d;

            radar._typeface ??= new Typeface(
                radar.FontFamily,
                radar.FontStyle,
                radar.FontWeight,
                radar.FontStretch);
            radar.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (GetRAndCenterPoint(out var centerPoint, out var r))
            {
                //
                // Skip Drawing
                return;
            }


            var axes = Axes is null || Axes.Count == 0 ? new AxisCollection() : Axes;
            var data = Data is null || Data.Count == 0 ? new ChartDataSet() : Data;

            /*
             *             3/2 π
             *              |
             *              |
             *      π  ----------  0
             *              |
             *              |
             *            1/2 π
             */
            //
            //
            PushGuideline(drawingContext);

            //
            //
            RenderingAxis(drawingContext, ref centerPoint, r, axes);

            //
            //
            RenderingData(drawingContext, ref centerPoint, r, axes?.Count ?? 0, data);

            //
            //
            drawingContext.Pop();
            base.OnRender(drawingContext);
        }

        private void RenderingAxis(DrawingContext drawingContext, ref Point centerPoint, double r, AxisCollection axes)
        {
            var pen = new Pen(DefaultBrush, 1);
            if (axes is null || axes.Count == 0)
            {
                drawingContext.DrawEllipse(null, pen, centerPoint, r * 1d, r * 1d);
                drawingContext.DrawEllipse(null, pen, centerPoint, r * .8d, r * .8d);
                drawingContext.DrawEllipse(null, pen, centerPoint, r * .6d, r * .6d);
                drawingContext.DrawEllipse(null, pen, centerPoint, r * .4d, r * .4d);
                drawingContext.DrawEllipse(null, pen, centerPoint, r * .2d, r * .2d);
            }
            else
            {
                drawingContext.DrawGeometry(null, pen, GetPolygon(r * 1d, axes.Count, ref centerPoint));
                drawingContext.DrawGeometry(null, pen, GetPolygon(r * .8d, axes.Count, ref centerPoint));
                drawingContext.DrawGeometry(null, pen, GetPolygon(r * .6d, axes.Count, ref centerPoint));
                drawingContext.DrawGeometry(null, pen, GetPolygon(r * .4d, axes.Count, ref centerPoint));
                drawingContext.DrawGeometry(null, pen, GetPolygon(r * .2d, axes.Count, ref centerPoint));
                RenderingAxisLine(drawingContext, pen, r, axes, ref centerPoint);
            }
        }

        private static void RenderingData(DrawingContext drawingContext, ref Point centerPoint, double r, int axisCount, ChartDataSet dataSet)
        {

            var count = Math.Min(axisCount, dataSet.Count);

            for (var i = 0; i < count; i++)
            {
                var set = dataSet[i];
                var color = Xaml.FromHex(set.Color);
                var axisData = new AxisData
                {
                    values = set.Values.Select(x => Math.Clamp(x / set.Maximum, 0, 1)).ToArray(),
                    pen    = new Pen(new SolidColorBrush(color), 1),
                    bg = new SolidColorBrush(color)
                    {
                        Opacity = .45f
                    }
                };

                var geometry = GetPolygon(r, ref axisData.values, ref centerPoint);
                drawingContext.DrawGeometry(axisData.bg, axisData.pen, geometry);
            }
        }

        private void RenderingAxisLine(DrawingContext drawingContext, Pen pen, double r, AxisCollection axes, ref Point centerPoint)
        {
            var count = axes.Count;
            var angle = 0;
            var avg = 360 / count;
            var dpi = VisualTreeHelper.GetDpi(this);
            var showText = ShowAxisName;
            var flowDirection = FlowDirection;
            var culture = CultureInfo.CurrentCulture;
            var fontSize = Math.Clamp(FontSize, 12, 100);
            var ns = new NumberSubstitution();

            _typeface   ??= new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            _foreground ??= new SolidColorBrush(Colors.Gray);

            for (var i = 0; i < count; i++)
            {
                var axis = axes[i];
                var text = new FormattedText(
                    axis.Name,
                    culture,
                    flowDirection,
                    _typeface,
                    fontSize,
                    _foreground,
                    ns,
                    TextFormattingMode.Display,
                    dpi.DpiScaleX * 96);

                drawingContext.DrawLine(pen, centerPoint, GetPoint(angle, r, ref centerPoint));

                // 绘制文字
                if (showText)
                    drawingContext.DrawText(text, GetPoint(angle, r, ref centerPoint));
                angle += avg;
            }
        }

        private static PathGeometry GetPolygon(double r, int count, ref Point centerPoint)
        {
            var points = new Point[count];
            var angle = 0;
            var avg = 360 / count;

            for (var i = 0; i < count; i++)
            {
                points[i] =  GetPoint(angle, r, ref centerPoint);
                angle     += avg;
            }

            var figure = new PathFigure(points[0], points.Skip(1).Select(x => new LineSegment(x, true)), true);
            return new PathGeometry
            {
                Figures = new PathFigureCollection
                {
                    figure
                }
            };
        }

        private static PathGeometry GetPolygon(double r, ref double[] values, ref Point centerPoint)
        {
            var count = values.Length;
            var points = new Point[count];
            var angle = 0;
            var avg = 360 / count;

            for (var i = 0; i < count; i++)
            {
                points[i] =  GetPoint(angle, r * values[i], ref centerPoint);
                angle     += avg;
            }

            var figure = new PathFigure(points[0], points.Skip(1).Select(x => new LineSegment(x, true)), true);
            return new PathGeometry
            {
                Figures = new PathFigureCollection
                {
                    figure
                }
            };
        }
        
        
        protected static void InvalidateVisual(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (RadarChart)d;
            radar.InvalidateVisual();
        }


        public static readonly DependencyProperty AxesProperty;
        public static readonly DependencyProperty DataProperty;
        public static readonly DependencyProperty ShowAxisNameProperty;

        public bool ShowAxisName
        {
            get => (bool)GetValue(ShowAxisNameProperty);
            set => SetValue(ShowAxisNameProperty, value);
        }

        public ChartDataSet Data
        {
            get => (ChartDataSet)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public AxisCollection Axes
        {
            get => (AxisCollection)GetValue(AxesProperty);
            set => SetValue(AxesProperty, value);
        }
    }
    
}
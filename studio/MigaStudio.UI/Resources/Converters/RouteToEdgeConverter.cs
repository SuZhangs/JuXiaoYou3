using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using GraphShape.Algorithms.Layout;
using GraphShape.Controls.Converters;
using GraphShape.Controls.Extensions;
using Point = GraphShape.Point;
using Size = GraphShape.Size;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
        /// <summary>
    /// Converter of position and sizes of the source and target points,
    /// and the route information of an edge to a path.
    /// </summary>
    /// <remarks>The edge can bend, or it can be straight line.</remarks>
    public class RouteToEdgeConverter : IMultiValueConverter
    {
        private static readonly Typeface _typeface = new Typeface("Microsoft Yahei Light"); 
        private static readonly SolidColorBrush _foreground = new SolidColorBrush(Colors.Black);
        private static readonly EdgeRouteToPathConverter _conv = new EdgeRouteToPathConverter();

        private static readonly Dictionary<string, PathFigureCollection> _dict =
            new Dictionary<string, PathFigureCollection>();


        #region IMultiValueConverter

        /// <inheritdoc />
        /// <exception cref="T:System.ArgumentException">
        /// At least one of 9 arguments is missing.
        /// pos (1,2), size (3,4) of source; pos (5,6), size (7,8) of target; routeInformation (9)
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void ConvertPrivate(object[] values, out System.Windows.Point p1, out System.Windows.Point p2)
        {
            if (values is null)
               return;

            ExtractInputs(
                values,
                out Point sourcePos,
                out Point targetPos,
                out Size sourceSize,
                out Size targetSize,
                out System.Windows.Point[] routeInformation);

            bool hasRouteInfo = routeInformation != null && routeInformation.Length > 0;

            // Create the path
            p1 = LayoutUtils.GetClippingPoint(
                sourceSize,
                sourcePos,
                hasRouteInfo ? routeInformation[0].ToGraphShapePoint() : targetPos).ToPoint();

            p2 = LayoutUtils.GetClippingPoint(
                targetSize,
                targetPos,
                hasRouteInfo ? routeInformation[routeInformation.Length - 1].ToGraphShapePoint() : sourcePos).ToPoint();
        }
        private static void ExtractInputs(
            object[] values,
            out Point sourcePos,
            out Point targetPos,
            out Size sourceSize,
            out Size targetSize,
            out System.Windows.Point[] routeInformation)
        {
            // Get the position of the source
            sourcePos = new Point(
                values[0] != DependencyProperty.UnsetValue ? (double) values[0] : 0.0,
                values[1] != DependencyProperty.UnsetValue ? (double) values[1] : 0.0);

            // Get the size of the source
            sourceSize = new Size(
                values[2] != DependencyProperty.UnsetValue ? (double) values[2] : 0.0,
                values[3] != DependencyProperty.UnsetValue ? (double) values[3] : 0.0);

            // Get the position of the target
            targetPos = new Point(
                values[4] != DependencyProperty.UnsetValue ? (double) values[4] : 0.0,
                values[5] != DependencyProperty.UnsetValue ? (double) values[5] : 0.0);

            // Get the size of the target
            targetSize = new Size(
                values[6] != DependencyProperty.UnsetValue ? (double) values[6] : 0.0,
                values[7] != DependencyProperty.UnsetValue ? (double) values[7] : 0.0);

            // Get the route information
            routeInformation = values[8] != DependencyProperty.UnsetValue ? (System.Windows.Point[]) values[8] : null;
        }

        #endregion

        #region IMultiValueConverter

        /// <inheritdoc />
        /// <exception cref="T:System.ArgumentException">
        /// At least one of 9 arguments is missing.
        /// pos (1,2), size (3,4) of source; pos (5,6), size (7,8) of target; routeInformation (9)
        /// </exception>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var rel = values[9] as IDataCache;
            var hint = rel?.Name ?? "hint";
            var newValues = new object[9];
            Array.Copy(values, newValues,9);
            
            ConvertPrivate(newValues, out var p1 ,out var p2);

            var p3 = new System.Windows.Point((p1.X + p2.X) / 2d , (p1.Y + p2.Y) / 2d);

            var formattedText =
                    new FormattedText(
                        hint,
                        culture,
                        FlowDirection.LeftToRight,
                        _typeface,
                        14d,
                        _foreground,
                        96d);
            // )
             var f1 = formattedText.BuildGeometry(p3).GetOutlinedPathGeometry().Figures;

            return new PathFigureCollection(f1);
        }

        /// <inheritdoc />
        /// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Path to edge route conversion not supported.");
        }

        #endregion
    }

    public class RouteToEdge2Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var x1 = (double)values[0];
            var y1 = (double)values[1];

            var x2 = (double)values[2];
            var y2 = (double)values[3];
            var f1 = new PathFigureCollection(new[]
            {
                new PathFigure(new System.Windows.Point(x1,y1), new PathSegment[]
                {
                    new LineSegment(new System.Windows.Point(x2,y2), true)
                }, false)
            });
            return f1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Path to edge route conversion not supported.");
        }
    }
}
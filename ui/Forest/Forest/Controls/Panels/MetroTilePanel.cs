using System.Linq;
using Acorisoft.FutureGL.Forest.Utils;

namespace Acorisoft.FutureGL.Forest.Controls.Panels
{
    public class MetroTilePanel : Panel
    {
        #region Attach Properties

        public static readonly DependencyProperty TileWidthProperty = DependencyProperty.RegisterAttached(
            "TileWidthOverride", typeof(int), typeof(MetroTilePanel), new PropertyMetadata(Boxing.IntValues[1]));

        public static readonly DependencyProperty TileHeightProperty = DependencyProperty.RegisterAttached(
            "TileHeightOverride", typeof(int), typeof(MetroTilePanel), new PropertyMetadata(Boxing.IntValues[1]));

        public static void SetTileHeight(DependencyObject element, int value)
        {
            element.SetValue(TileHeightProperty, Math.Clamp(value, 1, 64));
        }

        public static int GetTileHeight(DependencyObject element)
        {
            return (int)element.GetValue(TileHeightProperty);
        }

        public static void SetTileWidth(DependencyObject element, int value)
        {
            element.SetValue(TileWidthProperty, Math.Clamp(value, 1, 64));
        }

        public static int GetTileWidth(DependencyObject element)
        {
            return (int)element.GetValue(TileWidthProperty);
        }

        #endregion

        public static readonly DependencyProperty UnitSizeProperty = DependencyProperty.Register(
            nameof(UnitSize),
            typeof(double),
            typeof(MetroTilePanel),
            new PropertyMetadata(32d));

        public static readonly DependencyProperty UnitPerRowProperty = DependencyProperty.Register(
            nameof(UnitPerRow),
            typeof(int),
            typeof(MetroTilePanel),
            new PropertyMetadata(Boxing.IntValues[5]));

        public static readonly DependencyProperty GapProperty = DependencyProperty.Register(
            nameof(Gap),
            typeof(double),
            typeof(MetroTilePanel),
            new PropertyMetadata(default(double)));

        protected override Size MeasureOverride(Size availableSize)
        {
            var unitSize = UnitSize;

            foreach (FrameworkElement element in Children)
            {
                if (element is null)
                {
                    continue;
                }

                if (element.Visibility == Visibility.Collapsed)
                {
                    continue;
                }

                var height = GetTileHeight(element) * unitSize;
                var width  = GetTileWidth(element) * unitSize;

                element.Height = height;
                element.Width  = width;
                element.Measure(new Size(height, width));
                availableSize.Width  += width;
                availableSize.Height =  Math.Max(availableSize.Height, height);
            }


            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var unitSize     = UnitSize;
            var elements     = TakeAvailableElements();
            var maxRowOfLine = UnitPerRow;
            var maxColumn    = Math.Floor((decimal)(finalSize.Width / unitSize)) -1;

            var column      = 0;
            var columnWidth = 0;
            var columnInRow = 0;
            var lastHeightUnit = 0;
            var row         = 0;
            var rowIndex    = 0;
            var gap         = Gap;

            foreach (var element in elements)
            {
                var heightUnit = GetTileHeight(element);
                var widthUnit  = GetTileWidth(element);


                //
                // 判断是否可以布局到这一行
                if (column + widthUnit > maxColumn)
                {
                    rowIndex    = rowIndex + maxRowOfLine + 1;
                    row         = rowIndex;
                    columnInRow = 0;
                    column      = 0;
                    columnWidth = 0;
                }

                //
                // 判断是否可以布局到这一列
                if (heightUnit + row > maxRowOfLine)
                {
                    row         =  rowIndex;
                    columnInRow =  0;
                    column      += columnWidth;
                    columnWidth = 0;
                }

                //
                // 拓展本列宽度
                if (widthUnit > columnWidth)
                {
                    columnWidth = widthUnit;
                }

                //
                // 这一列是否有行间隙,如果有布局在当前行
                // 如果没有，布局在下一行
                if (columnWidth - columnInRow > widthUnit)
                {
                    element.Arrange(new Rect(
                        x: columnInRow * unitSize,
                        y: row * unitSize,
                        width: widthUnit * unitSize,
                        height: heightUnit * unitSize));
                    columnInRow += widthUnit;
                }
                else
                {
                    columnInRow = 0;
                    element.Arrange(new Rect(
                        x: column * unitSize,
                        y: row * unitSize,
                        width: widthUnit * unitSize,
                        height: heightUnit * unitSize));
                    row += heightUnit;
                }
            }

            // 按行布局
            // foreach (var element in elements)
            // {
            // }

            return base.ArrangeOverride(finalSize);
        }

        private FrameworkElement[] TakeAvailableElements()
        {
            var list = new List<FrameworkElement>(32);
            list.AddRange(Children.Cast<FrameworkElement>()
                                  .Where(element => element is not null && element.Visibility != Visibility.Collapsed));
            return list.ToArray();
        }

        public double Gap
        {
            get => (double)GetValue(GapProperty);
            set => SetValue(GapProperty, value);
        }

        /// <summary>
        /// 每行的最大高度数量
        /// </summary>
        /// <remarks>
        /// <para>最大值为：16</para>
        /// <para>最小值为：1</para>
        /// </remarks>
        public int UnitPerRow
        {
            get => (int)GetValue(UnitPerRowProperty);
            set => SetValue(UnitPerRowProperty, Math.Clamp(value, 1, 16));
        }

        /// <summary>
        /// 每个单位的高度
        /// </summary>
        /// <remarks>
        /// <para>最大值为：128px</para>
        /// <para>最小值为：32px</para>
        /// </remarks>
        public double UnitSize
        {
            get => (double)GetValue(UnitSizeProperty);
            set => SetValue(UnitSizeProperty, Math.Clamp(value, 32, 256));
        }
    }
}
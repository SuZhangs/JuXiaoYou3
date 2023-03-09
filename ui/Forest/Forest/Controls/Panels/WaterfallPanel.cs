using System.Linq;

namespace Acorisoft.FutureGL.Forest.Controls.Panels
{
    internal struct PanelUvSize
    {
        private readonly Orientation _orientation;

        public Size ScreenSize => new(U, V);

        public double U { get; set; }

        public double V { get; set; }

        public double Width
        {
            get => _orientation == Orientation.Horizontal ? U : V;
            private set
            {
                if (_orientation == Orientation.Horizontal)
                {
                    U = value;
                }
                else
                {
                    V = value;
                }
            }
        }

        public double Height
        {
            get => _orientation == Orientation.Horizontal ? V : U;
            private set
            {
                if (_orientation == Orientation.Horizontal)
                {
                    V = value;
                }
                else
                {
                    U = value;
                }
            }
        }

        public PanelUvSize(Orientation orientation, double width, double height)
        {
            U            = V = 0d;
            _orientation = orientation;
            Width        = width;
            Height       = height;
        }

        public PanelUvSize(Orientation orientation, Size size)
        {
            U            = V = 0d;
            _orientation = orientation;
            Width        = size.Width;
            Height       = size.Height;
        }

        public PanelUvSize(Orientation orientation)
        {
            U            = V = 0d;
            _orientation = orientation;
        }
    }
    
    public class WaterfallPanel : Panel
    {
        public static readonly DependencyProperty GroupsProperty = DependencyProperty.Register(
        nameof(Groups), typeof(int), typeof(WaterfallPanel), new FrameworkPropertyMetadata(
            Boxing.IntValues[3], FrameworkPropertyMetadataOptions.AffectsMeasure), IsGroupsValid);

    public int Groups
    {
        get => (int) GetValue(GroupsProperty);
        set => SetValue(GroupsProperty, value);
    }

    private static bool IsGroupsValid(object value) => (int) value >= 1;

    public static readonly DependencyProperty AutoGroupProperty = DependencyProperty.Register(
        nameof(AutoGroup), typeof(bool), typeof(WaterfallPanel), new FrameworkPropertyMetadata(
            Boxing.False, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public bool AutoGroup
    {
        get => (bool) GetValue(AutoGroupProperty);
        set => SetValue(AutoGroupProperty, Boxing.Box(value));
    }

    public static readonly DependencyProperty DesiredLengthProperty = DependencyProperty.Register(
        nameof(DesiredLength), typeof(double), typeof(WaterfallPanel), new FrameworkPropertyMetadata(Boxing.DoubleValues[0],
            FrameworkPropertyMetadataOptions.AffectsMeasure), IsInRangeOfPosDoubleIncludeZero);

    public double DesiredLength
    {
        get => (double) GetValue(DesiredLengthProperty);
        set => SetValue(DesiredLengthProperty, value);
    }

    public static readonly DependencyProperty OrientationProperty =
        StackPanel.OrientationProperty.AddOwner(typeof(WaterfallPanel),
            new FrameworkPropertyMetadata(Orientation.Horizontal,
                FrameworkPropertyMetadataOptions.AffectsMeasure));

    public Orientation Orientation
    {
        get => (Orientation) GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    ///     是否在正浮点数范围内（包括0）
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsInRangeOfPosDoubleIncludeZero(object value)
    {
        var v = (double) value;
        return !(double.IsNaN(v) || double.IsInfinity(v)) && v >= 0;
    }
    
    public static bool IsVerySmall(double value) => Math.Abs(value) < 1E-06;

    private int CalculateGroupCount(Orientation orientation, PanelUvSize size)
    {
        if (!AutoGroup)
        {
            return Groups;
        }

        var itemLength = DesiredLength;

        if (IsVerySmall(itemLength))
        {
            return Groups;
        }

        return (int) (size.U / itemLength);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var orientation = Orientation;
        var uvConstraint = new PanelUvSize(orientation, finalSize);

        var groups = CalculateGroupCount(orientation, uvConstraint);
        if (groups < 1)
        {
            return finalSize;
        }

        var vArr = new double[groups].ToList();
        var itemU = uvConstraint.U / groups;

        var children = InternalChildren;
        for (int i = 0, count = children.Count; i < count; i++)
        {
            var child = children[i];
            if (child == null)
            {
                continue;
            }

            var minIndex = vArr.IndexOf(vArr.Min());
            var minV = vArr[minIndex];
            var childUvSize = new PanelUvSize(orientation, child.DesiredSize);
            var childSize = new PanelUvSize(orientation, itemU, childUvSize.V);
            var childRectSize = new PanelUvSize(orientation, minIndex * itemU, minV);

            child.Arrange(new Rect(new Point(childRectSize.U, childRectSize.V), childSize.ScreenSize));
            vArr[minIndex] = minV + childUvSize.V;
        }

        return finalSize;
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var orientation = Orientation;
        var uvConstraint = new PanelUvSize(orientation, constraint);

        var groups = CalculateGroupCount(orientation, uvConstraint);
        if (groups < 1)
        {
            return constraint;
        }

        var vArr = new double[groups].ToList();
        var itemU = uvConstraint.U / groups;
        if (double.IsNaN(itemU) || double.IsInfinity(itemU))
        {
            return constraint;
        }

        var children = InternalChildren;
        for (int i = 0, count = children.Count; i < count; i++)
        {
            var child = children[i];
            if (child == null)
            {
                continue;
            }

            child.Measure(constraint);

            var sz = new PanelUvSize(orientation, child.DesiredSize);
            var minIndex = vArr.IndexOf(vArr.Min());
            var minV = vArr[minIndex];

            vArr[minIndex] = minV + sz.V;
        }

        uvConstraint = new PanelUvSize(orientation, new Size(uvConstraint.ScreenSize.Width, vArr.Max()));

        return uvConstraint.ScreenSize;
    }
    }
    
    /// <summary>
    /// TilePanel
    /// 瀑布流布局
    /// </summary>
    public class TilePanel : Panel
    {
        #region 枚举
    
        private enum OccupyType
        {
            NONE,
            WIDTHHEIGHT,
            OVERFLOW
        }
    
        #endregion
    
        #region 属性
    
        /// <summary>
        /// 容器内元素的高度
        /// </summary>
        public int TileHeight
        {
            get { return (int)GetValue(TileHeightProperty); }
            set { SetValue(TileHeightProperty, value); }
        }
        /// <summary>
        /// 容器内元素的高度
        /// </summary>
        public static readonly DependencyProperty TileHeightProperty =
            DependencyProperty.Register(nameof(TileHeight), typeof(int), typeof(TilePanel), new FrameworkPropertyMetadata(100, FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        /// 容器内元素的宽度
        /// </summary>
        public int TileWidth
        {
            get { return (int)GetValue(TileWidthProperty); }
            set { SetValue(TileWidthProperty, value); }
        }
        /// <summary>
        /// 容器内元素的宽度
        /// </summary>
        public static readonly DependencyProperty TileWidthProperty =
            DependencyProperty.Register(nameof(TileWidth), typeof(int), typeof(TilePanel), new FrameworkPropertyMetadata(100, FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetWidthPix(DependencyObject obj)
        {
            return (int)obj.GetValue(WidthPixProperty);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetWidthPix(DependencyObject obj, int value)
        {
            if (value > 0)
            {
                obj.SetValue(WidthPixProperty, value);
            }
        }
        /// <summary>
        /// 元素的宽度比例，相对于TileWidth
        /// </summary>
        public static readonly DependencyProperty WidthPixProperty =
            DependencyProperty.RegisterAttached("WidthPix", typeof(int), typeof(TilePanel), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetHeightPix(DependencyObject obj)
        {
            return (int)obj.GetValue(HeightPixProperty);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetHeightPix(DependencyObject obj, int value)
        {
            if (value > 0)
            {
                obj.SetValue(HeightPixProperty, value);
            }
        }
        /// <summary>
        /// 元素的高度比例，相对于TileHeight
        /// </summary>
        public static readonly DependencyProperty HeightPixProperty =
            DependencyProperty.RegisterAttached("HeightPix", typeof(int), typeof(TilePanel), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        /// <summary>
        /// 排列方向
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        /// <summary>
        /// 排列方向
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(TilePanel), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        /// 格子数量
        /// </summary>
        public int TileCount
        {
            get { return (int)GetValue(TileCountProperty); }
            set { SetValue(TileCountProperty, value); }
        }
        /// <summary>
        /// 格子数量
        /// </summary>
        public static readonly DependencyProperty TileCountProperty =
            DependencyProperty.Register(nameof(TileCount), typeof(int), typeof(TilePanel), new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        /// Tile之间的间距
        /// </summary>
        public Thickness TileMargin
        {
            get { return (Thickness)GetValue(TileMarginProperty); }
            set { SetValue(TileMarginProperty, value); }
        }
        /// <summary>
        /// Tile之间的间距
        /// </summary>
        public static readonly DependencyProperty TileMarginProperty =
            DependencyProperty.Register(nameof(TileMargin), typeof(Thickness), typeof(TilePanel), new FrameworkPropertyMetadata(new Thickness(2), FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        /// 最小的高度比例
        /// </summary>
        private int MinHeightPix { get; set; }
        /// <summary>
        /// 最小的宽度比例
        /// </summary>
        private int MinWidthPix { get; set; }
    
        #endregion
    
        #region 方法
    
        private Dictionary<string, Point> Maps { get; set; }
        private OccupyType SetMaps(Point currentPosition, Size childPix)
        {
            var isOccupy = OccupyType.NONE;
    
            if (currentPosition.X + currentPosition.Y != 0)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    isOccupy = IsOccupyWidth(currentPosition, childPix);
                }
                else
                {
                    isOccupy = IsOccupyHeight(currentPosition, childPix);
                }
            }
    
            if (isOccupy == OccupyType.NONE)
            {
                for (var i = 0; i < childPix.Width; i++)
                {
                    for (var j = 0; j < childPix.Height; j++)
                    {
                        Maps[$"x_{currentPosition.X + i}y_{currentPosition.Y + j}"] = new Point(currentPosition.X + i, currentPosition.Y + j);
                    }
                }
            }
    
            return isOccupy;
        }
        
        private OccupyType IsOccupyWidth(Point currentPosition, Size childPix)
        {
            //计算当前行能否放下当前元素
            if (TileCount - currentPosition.X - childPix.Width < 0)
            {
                return OccupyType.OVERFLOW;
            }
    
            for (var i = 0; i < childPix.Width; i++)
            {
                if (Maps.ContainsKey($"x_{currentPosition.X + i}y_{currentPosition.Y}"))
                {
                    return OccupyType.WIDTHHEIGHT;
                }
            }
    
            return OccupyType.NONE;
        }
        private OccupyType IsOccupyHeight(Point currentPosition, Size childPix)
        {
            //计算当前行能否放下当前元素
            if (TileCount - currentPosition.Y - childPix.Height < 0)
            {
                return OccupyType.OVERFLOW;
            }
    
            for (var i = 0; i < childPix.Height; i++)
            {
                if (Maps.ContainsKey($"x_{currentPosition.X}y_{currentPosition.Y + i}"))
                {
                    return OccupyType.WIDTHHEIGHT;
                }
            }
    
            return OccupyType.NONE;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var              childPix          = new Size();
            var              childPosition     = new Point();
            Point?           lastChildPosition = null;
    
            Maps = new Dictionary<string, Point>();
            for (var i = 0; i < Children.Count; )
            {
                var child = Children[i] as FrameworkElement;
    
                if (child is null)
                {
                    continue;
                }
    
                childPix.Width = GetWidthPix(child);
                childPix.Height = GetHeightPix(child);
    
                if (Orientation == Orientation.Vertical)
                {
                    if (childPix.Height > TileCount)
                    {
                        childPix.Height = TileCount;
                    }
                }
                else
                {
                    if (childPix.Width > TileCount)
                    {
                        childPix.Width = TileCount;
                    }
                }
                var isOccupy = SetMaps(childPosition, childPix);
                
                //换列
                if (isOccupy == OccupyType.WIDTHHEIGHT)
                {
                    lastChildPosition ??= childPosition;
                    if (Orientation == Orientation.Horizontal)
                    {
                        childPosition.X += MinWidthPix;
                    }
                    else
                    {
                        childPosition.Y += MinHeightPix;
                    }
                }
                //换行
                else if (isOccupy == OccupyType.OVERFLOW)
                {
                    lastChildPosition ??= childPosition;
                    if (Orientation == Orientation.Horizontal)
                    {
                        childPosition.X = 0;
                        childPosition.Y += Maps[$"x_{childPosition.X}y_{childPosition.Y}"].Y;
                        //childPosition.Y++;//= this.MinHeightPix;
                    }
                    else
                    {
                        childPosition.Y = 0;
                        childPosition.X += Maps[$"x_{childPosition.X}y_{childPosition.Y}"].X;
                        //childPosition.X++;//= this.MinWidthPix;
                    }
                }
                else
                {
                    i++;
                    child.Arrange(new Rect(childPosition.X * TileWidth + Math.Floor(childPosition.X / MinWidthPix) * (TileMargin.Left + TileMargin.Right),
                                           childPosition.Y * TileHeight + Math.Floor(childPosition.Y / MinHeightPix) * (TileMargin.Top + TileMargin.Bottom),
                                           child.DesiredSize.Width, child.DesiredSize.Height));
                    if (lastChildPosition != null)
                    {
                        childPosition = (Point)lastChildPosition;
                        lastChildPosition = null;
                    }
                    else
                    {
                        if (Orientation == Orientation.Horizontal)
                        {
                            childPosition.X += childPix.Width;
                            if (childPosition.X == TileCount)
                            {
                                childPosition.X = 0;
                                childPosition.Y++;
                            }
                        }
                        else
                        {
                            childPosition.Y += childPix.Height;
                            if (childPosition.Y == TileCount)
                            {
                                childPosition.Y = 0;
                                childPosition.X++;
                            }
                        }
                    }
                }
            }
    
            return finalSize;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            int childWidthPix, childHeightPix, maxRowCount = 0;
    
            if (Children.Count == 0) return new Size();
            
            //遍历孩子元素
            foreach (FrameworkElement child in Children)
            {
                childWidthPix = GetWidthPix(child);
                childHeightPix = GetHeightPix(child);
    
                if (MinHeightPix == 0) MinHeightPix = childHeightPix;
                if (MinWidthPix == 0) MinWidthPix = childWidthPix;
    
                if (MinHeightPix > childHeightPix) MinHeightPix = childHeightPix;
                if (MinWidthPix > childWidthPix) MinWidthPix = childWidthPix;
            }
    
            foreach (FrameworkElement child in Children)
            {
                childWidthPix = GetWidthPix(child);
                childHeightPix = GetHeightPix(child);
    
                child.Margin = TileMargin;
                child.HorizontalAlignment = HorizontalAlignment.Left;
                child.VerticalAlignment = VerticalAlignment.Top;
    
                child.Width = TileWidth * childWidthPix + (child.Margin.Left + child.Margin.Right) * ((childWidthPix - MinWidthPix) / MinWidthPix);
                child.Height = TileHeight * childHeightPix + (child.Margin.Top + child.Margin.Bottom) * ((childHeightPix - MinHeightPix) / MinHeightPix);
    
                maxRowCount += childWidthPix * childHeightPix;
    
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }
    
            if (TileCount <= 0) throw new ArgumentOutOfRangeException();
            //if (this.MinWidthPix == 0) this.MinWidthPix = 1;
            //if (this.MinHeightPix == 0) this.MinHeightPix = 1;
            if (Orientation == Orientation.Horizontal)
            {
                Width = constraint.Width = TileCount * TileWidth + TileCount / MinWidthPix * (TileMargin.Left + TileMargin.Right);
                var heightPix = Math.Ceiling((double)maxRowCount / TileCount);
                if (!double.IsNaN(heightPix))
                    constraint.Height = heightPix * TileHeight + heightPix / MinHeightPix * (TileMargin.Top + TileMargin.Bottom);
            }
            else
            {
                Height = constraint.Height = TileCount * TileHeight + TileCount / MinHeightPix * (TileMargin.Top + TileMargin.Bottom);
                var widthPix = Math.Ceiling((double)maxRowCount / TileCount);
                if (!double.IsNaN(widthPix))
                    constraint.Width = widthPix * TileWidth + widthPix / MinWidthPix * (TileMargin.Left + TileMargin.Right);
            }
    
            return constraint;
        }
    
        #endregion
    }

}
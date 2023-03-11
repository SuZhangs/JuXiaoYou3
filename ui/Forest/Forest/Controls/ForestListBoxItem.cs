using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public partial class ForestListBoxItem : ListBoxItem
    {
        static ForestListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestListBoxItem), new FrameworkPropertyMetadata(typeof(ForestListBoxItem)));
        }
        
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(Geometry),
            typeof(ForestTabItemBase),
            new PropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty IsFilledProperty = DependencyProperty.Register(
            nameof(IsFilled),
            typeof(bool),
            typeof(ForestTabItemBase),
            new PropertyMetadata(Boxing.False));

        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
            nameof(Placement),
            typeof(Dock),
            typeof(ForestTabItemBase),
            new PropertyMetadata(Dock.Left));


        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            nameof(IconSize),
            typeof(double),
            typeof(ForestTabItemBase),
            new PropertyMetadata(17d));

        public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(
            nameof(Palette),
            typeof(HighlightColorPalette),
            typeof(ForestTabItemBase),
            new PropertyMetadata(default(HighlightColorPalette)));

        public ForestListBoxItem()
        {
            // TODO: add style
            Finder                           =  GetTemplateChild();
            StateMachine                     =  new VisualDFA();
            Loaded                           += OnLoadedIntern;
            Unloaded                         += OnUnloadedIntern;
            IsEnabledChanged                 += OnEnableChanged;
            StateMachine.StateChangedHandler =  HandleStateChanged;
            Initialize();
        }

        private void HandleStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            OnStateChanged(init, now);
        }


        /// <summary>
        /// 构建状态
        /// </summary>
        private void OnEnableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (bool)e.OldValue;
            var newValue = (bool)e.NewValue;

            if (!StateMachine.HasInitialized)
            {
                return;
            }

            if (oldValue && !newValue)
            {
                StateMachine.NextState(VisualState.Inactive);
            }

            if (!oldValue && newValue)
            {
                StateMachine.NextState(VisualState.Normal);
            }
        }

        private void Initialize()
        {
            BuildState();
            GetTemplateChildOverride(Finder);
        }

        protected virtual void BuildState()
        {
            StateMachine.AddState(VisualState.Normal, VisualState.Highlight1, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight2, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Normal, VisualStateTrigger.Next, false);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Normal, VisualState.Inactive, VisualStateTrigger.Disabled);
        }

        protected virtual void BuildAnimation()
        {
            //
        }

        public override void OnApplyTemplate()
        {
            Finder.Done(BuildAnimation).Find();
            StateMachine.NextState();
            base.OnApplyTemplate();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected ITemplatePartFinder GetTemplateChild() => new Finder(GetTemplateChild);

        /// <summary>
        /// 开始构建状态驱动的动画
        /// </summary>
        /// <returns>返回一个动画构建器。</returns>
        protected IStateDrivenAnimatorBuilder StateDrivenAnimation() => new StateDrivenAnimatorBuilder();

        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

        private void OnLoadedIntern(object sender, RoutedEventArgs e)
        {
            OnLoaded(sender, e);
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }

            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnUnselected(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight1 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Highlight1);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnMouseLeave(e);
        }

        public void SetText(string text)
        {
            Content = text;
        }

        public void SetToolTips(string text)
        {
            ToolTip = text;
        }

        /// <summary>
        /// 视觉状态机。
        /// </summary>
        protected VisualDFA StateMachine { get; }

        /// <summary>
        /// 模板查找器
        /// </summary>
        protected ITemplatePartFinder Finder { get; }

        /// <summary>
        /// 动画工具
        /// </summary>
        public Animator Animator { get; protected set; }

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public Dock Placement
        {
            get => (Dock)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
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

        public HighlightColorPalette Palette
        {
            get => (HighlightColorPalette)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }
    }
    
    
    partial class ForestListBoxItem
    {
        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";


        private bool             _disabled;
        private Storyboard       _storyboard;
        private Border           _bd;
        private ContentPresenter _content;
        private SolidColorBrush  _backgroundBrush;
        private SolidColorBrush  _foregroundBrush;
        private SolidColorBrush  _backgroundHighlight1Brush;
        private SolidColorBrush  _backgroundHighlight2Brush;
        private SolidColorBrush  _foregroundHighlightBrush;
        
        protected virtual void OnStateChanged(bool init, VisualState now)
        {
            var palette = Palette;
            var theme = ThemeSystem.Instance.Theme;

            // Stop Animation
            _storyboard?.Stop(_bd);

            if (!init)
            {
                HandleNormalState();
            }
            else
            {
                if (now == VisualState.Highlight1)
                {
                    HandleHighlight1State(theme.Duration.Medium);
                }
                else if (now == VisualState.Highlight2)
                {
                    HandleHighlight2State(theme.Duration.Medium);
                }
                else if (now == VisualState.Normal)
                {
                    HandleNormalState();
                }
                else if (now == VisualState.Inactive)
                {
                    HandleDisabledState(palette, theme);
                }
            }
        }

        protected virtual void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                   = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }


        private void HandleDisabledState(HighlightColorPalette palette,ForestThemeSystem theme)
        {
            //
            // 设置背景颜色

            _disabled        = true;
            _backgroundBrush = new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundInactive]);
            _foregroundBrush = new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInActive]);
            _bd.Background   = _backgroundBrush;

            // 设置文本颜色
            SetForeground(_foregroundBrush);
        }

        private void SetForeground(SolidColorBrush highlightForeground)
        {
            _content.SetValue(TextElement.ForegroundProperty, highlightForeground);
        }

        private void HandleNormalState()
        {
            var palette = Palette;
            var theme   = ThemeSystem.Instance.Theme;
            
            if (!IsEnabled)
            {
                HandleDisabledState(palette, theme);
                return;
            }
            
            if (IsSelected)
            {
                //
                // 设置背景颜色
                _backgroundHighlight2Brush ??= theme.GetHighlightColor(palette,3).ToSolidColorBrush();
                _foregroundHighlightBrush  ??= theme.Colors[(int)ForestTheme.ForegroundInHighlight].ToSolidColorBrush();
                _bd.Background             =   _backgroundHighlight2Brush;

                // 设置文本颜色
                SetForeground(_foregroundHighlightBrush);
            }
            else
            {
                //
                // 设置背景颜色
                if (_disabled || 
                    _backgroundBrush is null ||
                    _foregroundBrush is null)
                {
                    _backgroundBrush = theme.Colors[(int)ForestTheme.Background].ToSolidColorBrush();
                    _foregroundBrush = theme.Colors[(int)ForestTheme.Foreground].ToSolidColorBrush();
                    _disabled        = false;
                }

                _bd.Background   =   _backgroundBrush;

                // 设置文本颜色
                SetForeground(_foregroundBrush);
            }
        }

        private void HandleHighlight1State(Duration duration)
        {
            //
            // Opacity 动画
            var palette = Palette;
            var theme   = ThemeSystem.Instance.Theme;
            
            _backgroundHighlight1Brush ??= theme.GetHighlightColor(palette, 1).ToSolidColorBrush();
            _foregroundHighlightBrush  ??= theme.Colors[(int)ForestTheme.ForegroundInHighlight].ToSolidColorBrush();

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundBrush.Color,
                To       = _backgroundHighlight1Brush.Color,
            };

            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundHighlightBrush);
        }

        private void HandleHighlight2State(Duration duration)
        {
            //
            // Opacity 动画
            // var OpacityAnimation = new DoubleAnimation()
            // {
            //     Duration = duration,
            //     From     = 0.5,
            //     To       = 1,
            // };
            var palette = Palette;
            var theme   = ThemeSystem.Instance.Theme;
            _backgroundHighlight1Brush ??= theme.GetHighlightColor(palette, 1).ToSolidColorBrush();
            _backgroundHighlight2Brush ??= theme.GetHighlightColor(palette, 3).ToSolidColorBrush();
            _foregroundHighlightBrush  ??= theme.Colors[(int)ForestTheme.ForegroundInHighlight].ToSolidColorBrush();

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1Brush.Color,
                To       = _backgroundHighlight2Brush.Color,
            };

            // Storyboard.SetTarget(OpacityAnimation, _bd);
            Storyboard.SetTarget(backgroundAnimation, _bd);
            // Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { /*OpacityAnimation,*/ backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundHighlightBrush);
        }
    }
}
using System.Printing;
using System.Windows;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.Styles.Animations;

// ReSharper disable MemberCanBeMadeStatic.Global

namespace Acorisoft.FutureGL.Forest.Controls
{
    /// <summary>
    /// <see cref="ForestButtonBase"/> 类型表示一个支持VisualDFA的按钮。
    /// </summary>
    /// <remarks>
    /// <para>推荐的状态图：Normal -> Highlight1 -> Highlight2 -> Normal</para>
    /// </remarks>
    [Obsolete]
    public abstract class ForestButtonBase : Button, IHighlightColorPalette, ITextResourceAdapter, IForestControl
    {
        public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(
            nameof(Palette),
            typeof(HighlightColorPalette),
            typeof(ForestButtonBase),
            new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IForestControl)d).InvalidateState();
        }


        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(ForestButtonBase),
            new PropertyMetadata(default(CornerRadius)));

        protected ForestButtonBase()
        {
            Animator                         =  Animator.CreateDummy();
            Finder                           =  GetTemplateChild();
            StateMachine                     =  new VisualDFA();
            Loaded                           += OnLoadedIntern;
            Unloaded                         += OnUnloadedIntern;
            IsEnabledChanged                 += OnEnableChanged;
            StateMachine.StateChangedHandler =  HandleStateChanged;
            Initialize();
        }

        #region Initialize

        private void Initialize()
        {
            GetTemplateChildOverride(Finder);
            BuildState();
        }

        #endregion

        #region Button State Defnitions

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
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }

            if (!oldValue && newValue)
            {
                StateMachine.NextState(VisualStateTrigger.Back);
            }
        }


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (StateMachine.CurrentState is VisualState.Highlight2 && IsPressed)
            {
                StateMachine.NextState(IsMouseOver ? VisualStateTrigger.Back : VisualStateTrigger.Next);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight1)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            ReleaseMouseCapture();
            StateMachine.ResetState();
            base.OnMouseLeave(e);
        }

        public override void OnApplyTemplate()
        {
            Finder.Find();
            StateMachine.NextState();
            base.OnApplyTemplate();
        }

        #endregion

        #region Button States

        protected abstract void StopAnimation();
        protected abstract void SetForeground(Brush brush);
        
        

        private void HandleStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var palette = Palette;
            var theme   = ThemeSystem.Instance.Theme;

            // Stop Animation
            StopAnimation();

            if (!init)
            {
                if (IsEnabled)
                {
                    GoToNormalState(palette, theme);
                }
                else
                {
                    GoToDisableState(palette, theme);
                }
                return;
            }

            switch (now)
            {
                default:
                    GoToNormalState(palette, theme);
                    break;
                case VisualState.Highlight1:
                    GoToHighlight1State(theme.Duration.Medium, palette, theme);
                    break;
                case VisualState.Highlight2:
                    GoToHighlight2State(theme.Duration.Medium, palette, theme);
                    break;
                case VisualState.Inactive:
                    GoToDisableState(palette, theme);
                    break;
                    
            }
        }

        protected abstract void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToHighlight1State(Duration duration,HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToHighlight2State(Duration duration,HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToDisableState(HighlightColorPalette palette, ForestThemeSystem theme);


        #endregion

        #region Loaded / Unloaded

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

        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

        #endregion

        /// <summary>
        /// 构建状态
        /// </summary>
        private void BuildState()
        {
            StateMachine.AddState(VisualState.Normal, VisualState.Highlight1, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight2, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Normal, VisualStateTrigger.Next, false);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Normal, VisualState.Inactive, VisualStateTrigger.Disabled);
        }

        protected abstract void GetTemplateChildOverride(ITemplatePartFinder finder);

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

        #region IForestControl Members

        public void InvalidateState()
        {
            OnInvalidateState();
            InvalidateMeasure();
        }

        protected virtual void OnInvalidateState()
        {
        }

        #endregion

        #region ITextResourceAdapter Members

        void ITextResourceAdapter.SetText(string text)
        {
            Content = text;
        }

        void ITextResourceAdapter.SetToolTips(string text)
        {
            ToolTip = text;
        }

        #endregion


        /// <summary>
        /// 模板查找器
        /// </summary>
        protected ITemplatePartFinder Finder { get; }

        /// <summary>
        /// 视觉状态机。
        /// </summary>
        protected VisualDFA StateMachine { get; }

        /// <summary>
        /// 动画工具
        /// </summary>
        public Animator Animator { get; protected set; }

        public HighlightColorPalette Palette
        {
            get => (HighlightColorPalette)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }

    
    /// <summary>
    /// <see cref="ForestIconButtonBase"/> 类型表示一个支持VisualDFA的图标按钮。
    /// </summary>
    /// <remarks>
    /// <para>推荐的状态图：Normal -> Highlight1 -> Highlight2 -> Normal</para>
    /// </remarks>
    public abstract class ForestIconButtonBase : ForestButtonBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;

        static ForestIconButtonBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(Boxing.False));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(Boxing.False));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestIconButtonBase)d).HasIcon = e.NewValue is Geometry;
        }

        public bool HasIcon
        {
            get => (bool)GetValue(HasIconProperty);
            private set => SetValue(HasIconProperty, value);
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
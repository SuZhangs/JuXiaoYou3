using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.Styles.Animations;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class ForestTextBox : TextBox, ITextResourceAdapter
    {
        public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(
                                                                                                nameof(Palette),
                                                                                                typeof(HighlightColorPalette),
                                                                                                typeof(ForestTextBox),
                                                                                                new PropertyMetadata(default(HighlightColorPalette)));

        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_ContentHost";


        private Border       _bd;
        private ScrollViewer _content;
        private Storyboard   _storyboard;

        public ForestTextBox()
        {
            Finder                           =  GetTemplateChild();
            StateMachine                     =  new VisualDFA();
            Loaded                           += OnLoadedIntern;
            Unloaded                         += OnUnloadedIntern;
            IsEnabledChanged                 += OnEnableChanged;
            StateMachine.StateChangedHandler =  HandleStateChanged;
            Initialize();
        }

        private void Initialize()
        {
            GetTemplateChildOverride(Finder);
            BuildState();
            BuildAnimation();
        }

        private void HandleStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var palette = Palette;
            var theme = ThemeSystem.Instance.Theme;

            // 正常的背景颜色
            var background = theme.Colors[(int)ForestTheme.Background];
            var foreground = theme.Colors[(int)ForestTheme.Foreground];
            var highlightBackground = theme.GetHighlightColor(palette, 3);
            var activeBackground = theme.GetHighlightColor(palette, 1);
            var highlightForeground = theme.Colors[(int)ForestTheme.ForegroundInHighlight];
            var disabledForeground = theme.Colors[(int)ForestTheme.ForegroundInActive];

            // Stop Animation
            _storyboard?.Stop(_bd);

            if (!init)
            {
                HandleNormalState(
                    ref background,
                    ref disabledForeground,
                    ref foreground,
                    ref highlightBackground,
                    ref highlightForeground);
            }
            else
            {
                if (now == VisualState.Highlight1)
                {
                    HandleHighlight1State(
                        theme.Duration.Medium, 
                        ref background,
                        ref activeBackground,
                        ref highlightForeground);
                }
                else if (now == VisualState.Highlight2)
                {
                    HandleHighlight2State(
                        theme.Duration.Medium, 
                        ref activeBackground,
                        ref highlightBackground,
                        ref highlightForeground);
                }
                else if (now == VisualState.Normal)
                {
                    HandleNormalState(
                        ref background,
                        ref disabledForeground,
                        ref foreground,
                        ref highlightBackground,
                        ref highlightForeground);
                }
                else if (now == VisualState.Inactive)
                {
                    HandleDisabledState(ref background, ref disabledForeground);
                }
            }
        }

        private void HandleDisabledState(ref Color background,ref Color foreground)
        {
            //
            // 设置背景颜色
            _bd.Background = background.ToSolidColorBrush();
            _bd.Effect     = null;

            // 设置文本颜色
            SetForeground(ref foreground);
        }

        private void SetForeground(ref Color highlightForeground)
        {
            var foreground = highlightForeground.ToSolidColorBrush();
            _content.SetValue(TextElement.ForegroundProperty, foreground);
        }

        private void HandleNormalState(ref Color background, ref Color disabledForeground, ref Color foreground,
            ref Color highlightBackground, ref Color highlightForeground)
        {
            if (!IsEnabled)
            {
                HandleDisabledState(ref background, ref disabledForeground);
                return;
            }
            
            if (IsKeyboardFocused)
            {
                //
                // 设置背景颜色
                _bd.Background = highlightBackground.ToSolidColorBrush();

                // 创建阴影
                _bd.Effect = BuildHighlightShadowEffect(ref highlightBackground);

                // 设置文本颜色
                SetForeground(ref highlightForeground);
            }
            else
            {
                //
                // 设置背景颜色
                _bd.Background = background.ToSolidColorBrush();
                _bd.Effect     = null;

                // 设置文本颜色
                SetForeground(ref foreground);
            }
        }

        private void HandleHighlight1State(Duration duration, ref Color background,ref Color highlightBackground, ref Color highlightForeground)
        {
            //
            // Opacity 动画

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = background,
                To       = highlightBackground,
            };

            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(ref highlightForeground);
        }

        private void HandleHighlight2State(Duration duration, ref Color background, ref Color highlightBackground, ref Color highlightForeground)
        {
            //
            // Opacity 动画
            // var OpacityAnimation = new DoubleAnimation()
            // {
            //     Duration = duration,
            //     From     = 0.5,
            //     To       = 1,
            // };

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = background,
                To       = highlightBackground,
            };

            // Storyboard.SetTarget(OpacityAnimation, _bd);
            Storyboard.SetTarget(backgroundAnimation, _bd);
            // Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { /*OpacityAnimation,*/ backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 创建阴影
            _bd.Effect = BuildHighlightShadowEffect(ref highlightBackground);

            // 设置文本颜色
            SelectionBrush = ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.HighlightA2].ToSolidColorBrush();
            SetForeground(ref highlightForeground);
        }

        private static DropShadowEffect BuildHighlightShadowEffect(ref Color color)
        {
            // <DropShadowEffect x:Key = "Shadow.A3" Direction = "270" ShadowDepth = "4.5" BlurRadius = "14" Opacity = "0.42" Color = "{DynamicResource AccentA3}" />
            return new DropShadowEffect
            {
                Color       = color,
                ShadowDepth = 4.5d,
                BlurRadius  = 14,
                Opacity     = 0.42d,
                Direction   = 270
            };
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
        

        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

        /// <summary>
        /// 构建状态
        /// </summary>
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
            
        }


        protected virtual void GetTemplateChildOverride(ITemplatePartFinder finder)
        {

            finder.Find<Border>(PART_BdName, x => _bd                 = x)
                  .Find<ScrollViewer>(PART_ContentName, x => _content = x);
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

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnGotKeyboardFocus(e);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight2)
            {
                StateMachine.NextState(IsMouseOver ? VisualStateTrigger.Back : VisualStateTrigger.Next);
            }

            base.OnLostKeyboardFocus(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (!IsKeyboardFocused)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            ReleaseMouseCapture();
            if (StateMachine.CurrentState == VisualState.Highlight1)
            {
                StateMachine.ResetState();
            }
            base.OnMouseLeave(e);
        }


        public override void OnApplyTemplate()
        {
            Finder.Done(BuildAnimation).Find();
            StateMachine.NextState();
            base.OnApplyTemplate();
        }

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

        /// <summary>
        /// 动画工具
        /// </summary>
        public HighlightColorPalette Palette
        {
            get => (HighlightColorPalette)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        public void SetText(string text)
        {
        }

        public void SetToolTips(string text)
        {
            ToolTip = text;
        }
    }
}
using System.Windows;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Enums;
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
    public abstract class ForestButtonBase : Button, IHighlightColorPalette, ITextResourceAdapter
    {

        public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(
            nameof(Palette),
            typeof(HighlightColorPalette),
            typeof(ForestButtonBase),
            new PropertyMetadata(default(HighlightColorPalette)));
        
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

        private void Initialize()
        {
            GetTemplateChildOverride(Finder);
            BuildState();
            BuildAnimation();
        }

        private void HandleStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            if (!init)
            {
                Animator.NextState();

                if (!IsEnabled)
                {
                    Animator.NextState(VisualState.Inactive);
                    StateMachine.NextState(VisualState.Inactive, false);
                }
            }
            else
            {
                Animator.NextState(now);
            }
        }

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
            Finder.Done(BuildAnimation).Find();
            StateMachine.NextState();
            base.OnApplyTemplate();
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
    }
}
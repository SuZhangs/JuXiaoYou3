using System.Windows;
using System.Windows.Input;

using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.Styles.Animations;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class ForestTextBoxBase : TextBox, ITextResourceAdapter
    {
        public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(
            nameof(Palette),
            typeof(HighlightColorPalette),
            typeof(ForestTextBoxBase),
            new PropertyMetadata(default(HighlightColorPalette)));

        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_ContentHost";

        public ForestTextBoxBase()
        {
            Animator         =  Animator.CreateDummy();
            Finder           =  GetTemplateChild();
            StateMachine     =  new VisualDFA();
            Loaded           += OnLoadedIntern;
            Unloaded         += OnUnloadedIntern;
            IsEnabledChanged += OnEnableChanged;
            Initialize();
        }

        private void Initialize()
        {
            GetTemplateChildOverride(Finder);
            BuildState();
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
            finder.Find<Border>(PART_BdName, x => PART_Bd                 = x)
                  .Find<ScrollViewer>(PART_ContentName, x => PART_Content = x);
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
            if (StateMachine.CurrentState == VisualState.Normal)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }
            else
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

        protected Border PART_Bd { get; private set; }
        protected ScrollViewer PART_Content { get; private set; }

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
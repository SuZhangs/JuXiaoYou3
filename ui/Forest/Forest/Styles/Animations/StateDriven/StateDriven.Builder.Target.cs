using System.Windows;
using System.Windows.Media;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    internal class TargetBuilder : IStateDrivenTargetBuilder
    {
        public TargetBuilder(Action<StateDrivenAnimation> disposeExpr)
        {
            DisposeAction = disposeExpr;
            Arguments     = new List<StateDrivenAnimation.AnimationArgument>(8);
        }

        public IStateDrivenPropertyAnimationBuilder<Color?> Color(DependencyProperty property, Duration duration) => new ColorPropertyBuilder(AddExpr)
        {
            Duration = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<Color?> Color(Duration duration,params object[] property) => new ColorPropertyBuilder(AddExpr)
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<double?> Double(DependencyProperty property, Duration duration)=> new DoublePropertyBuilder(AddExpr)
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<double?> Double(Duration duration,params object[] property)=> new DoublePropertyBuilder(AddExpr)
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<object> Object(DependencyProperty property, Duration duration)=> new ObjectPropertyBuilder(AddExpr)
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<object> Object(Duration duration,params object[] property)=> new ObjectPropertyBuilder(AddExpr)
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(DependencyProperty property, Duration duration)=> new ThicknessPropertyBuilder(AddExpr)
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(Duration duration, params object[] property)=> new ThicknessPropertyBuilder(AddExpr)
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        private void AddExpr(StateDrivenAnimation.AnimationArgument argument)
        {
            Arguments.Add(argument);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public List<StateDrivenAnimation.AnimationArgument> Arguments { get; }

        /// <summary>
        /// 完成并添加
        /// </summary>
        public void Dispose() => DisposeAction?.Invoke(Finish());

        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public Action<StateDrivenAnimation> DisposeAction { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public FrameworkElement TargetElement { get; init; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StateDrivenAnimation Finish()
        {
            return new StateDrivenAnimation
            {
                TargetElement = TargetElement,
                Arguments     = Arguments,
            };
        }
    }

    internal class TargetAndDefaultBuilder : IStateDrivenTargetAndDefaultBuilder
    {
        public TargetAndDefaultBuilder(Action<FirstStateAnimation> disposeExpr) => DisposeAction = disposeExpr;
        
        public IStateDrivenTargetAndDefaultBuilder Set(DependencyProperty property, object value)
        {
            Animations.Add(new FirstStateAnimation.Setter
            {
                Value = value,
                Property = property
            });
            return this;
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <returns></returns>
        public FirstStateAnimation Finish()
        {
            return new FirstStateAnimation
            {
                TargetElement = TargetElement,
                Setters       = Animations
            };
        }

        /// <summary>
        /// 完成并添加
        /// </summary>
        public void Dispose() => DisposeAction?.Invoke(Finish());

        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public Action<FirstStateAnimation> DisposeAction { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public FrameworkElement TargetElement { get; init; }


        public IList<FirstStateAnimation.Setter> Animations { get; } = new List<FirstStateAnimation.Setter>(8);
    }
}
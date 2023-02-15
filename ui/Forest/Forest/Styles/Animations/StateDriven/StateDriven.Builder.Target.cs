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

        public IStateDrivenPropertyAnimationBuilder<Color?> Color(DependencyProperty property, Duration duration) => new PropertyBuilder<Color?>
        {
            Duration = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<Color?> Color(Duration duration,params object[] property) => new PropertyBuilder<Color?>
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<double?> Double(DependencyProperty property, Duration duration)=> new PropertyBuilder<double?>
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<double?> Double(Duration duration,params object[] property)=> new PropertyBuilder<double?>
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<object> Object(DependencyProperty property, Duration duration)=> new PropertyBuilder<object>
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<object> Object(Duration duration,params object[] property)=> new PropertyBuilder<object>
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(DependencyProperty property, Duration duration)=> new PropertyBuilder<Thickness?>
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(Duration duration, params object[] property)=> new PropertyBuilder<Thickness?>
        {
            Duration        = duration,
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };
        
        /// <summary>
        /// 
        /// </summary>
        public IList<StateDrivenAnimation.AnimationArgument> Arguments { get; }

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

        public StateDrivenAnimation Finish()
        {
            throw new NotImplementedException();
        }
    }

    internal class TargetAndDefaultBuilder : IStateDrivenTargetAndDefaultBuilder
    {
        public TargetAndDefaultBuilder(Action<FirstStateAnimation> disposeExpr) => DisposeAction = disposeExpr;
        
        public IStateDrivenTargetAndDefaultBuilder Set(DependencyProperty property, Duration duration, object value)
        {
            Animations.Add(new FirstStateAnimation.Setter
            {
                Value = value,
                Property = property
            });
            return this;
        }


        public IList<FirstStateAnimation.Setter> Animations { get; } = new List<FirstStateAnimation.Setter>(8);

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
    }
}
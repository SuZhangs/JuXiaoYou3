using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public abstract class PropertyBuilder
    {
        public IStateDrivenAnimationBuilder<Color?> Color(DependencyProperty property, Duration duration) => TargetContext.Color(property, duration);

        public IStateDrivenAnimationBuilder<Color?> Color(Duration duration, params object[] property)=> TargetContext.Color(duration, property);

        public IStateDrivenAnimationBuilder<double?> Double(DependencyProperty property, Duration duration)=> TargetContext.Double(property, duration);

        public IStateDrivenAnimationBuilder<double?> Double(Duration duration, params object[] property)=> TargetContext.Double(duration, property);

        public IStateDrivenAnimationBuilder<object> Object(DependencyProperty property, Duration duration)=> TargetContext.Object(property, duration);

        public IStateDrivenAnimationBuilder<object> Object(Duration duration, params object[] property)=> TargetContext.Object(duration, property);

        public IStateDrivenAnimationBuilder<Thickness?> Thickness(DependencyProperty property, Duration duration)=> TargetContext.Thickness(property, duration);

        public IStateDrivenAnimationBuilder<Thickness?> Thickness(Duration duration, params object[] property)=> TargetContext.Thickness(duration, property);


        public abstract PropertyAnimationRuner Finish();
        
        public abstract void Dispose();
        
        /// <summary>
        /// 目标构造器上下文。
        /// </summary>
        public IStateDrivenTargetBuilder TargetContext { get; init; }

        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public PropertyPath PropertyPath { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public Duration Duration { get; init; }
    }

    [SuppressMessage("Usage", "CA1816:Dispose 方法应调用 SuppressFinalize")]
    public abstract class PropertyBuilder<TValue> : PropertyBuilder
    {
        protected PropertyBuilder(Action<PropertyAnimationRuner> expr)
        {
            DisposeAction = expr;
        }
        
        protected abstract PropertyAnimationRuner CreateArgument();
        
        public IStateDrivenAnimationBuilder<TValue> Next(VisualState state, TValue value)
        {
            Mapper.TryAdd(state, value);
            return this;
        }


        public override PropertyAnimationRuner Finish()
        {
            return CreateArgument();
        }

        public sealed override void Dispose()
        {
            DisposeAction(Finish());
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<PropertyAnimationRuner> DisposeAction { get; }
        

        public Dictionary<VisualState, TValue> Mapper { get; protected init; }
    }
}
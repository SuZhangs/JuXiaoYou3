using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public abstract class PropertyBuilder : IStateDrivenPropertyAnimationBuilder
    {
        public IStateDrivenPropertyAnimationBuilder<Color?> NextColor(DependencyProperty property, Duration duration) => TargetContext.Color(property, duration);

        public IStateDrivenPropertyAnimationBuilder<Color?> NextColor(Duration duration, params object[] property)=> TargetContext.Color(duration, property);

        public IStateDrivenPropertyAnimationBuilder<double?> NextDouble(DependencyProperty property, Duration duration)=> TargetContext.Double(property, duration);

        public IStateDrivenPropertyAnimationBuilder<double?> NextDouble(Duration duration, params object[] property)=> TargetContext.Double(duration, property);

        public IStateDrivenPropertyAnimationBuilder<object> NextObject(DependencyProperty property, Duration duration)=> TargetContext.Object(property, duration);

        public IStateDrivenPropertyAnimationBuilder<object> NextObject(Duration duration, params object[] property)=> TargetContext.Object(duration, property);

        public IStateDrivenPropertyAnimationBuilder<Thickness?> NextThickness(DependencyProperty property, Duration duration)=> TargetContext.Thickness(property, duration);

        public IStateDrivenPropertyAnimationBuilder<Thickness?> NextThickness(Duration duration, params object[] property)=> TargetContext.Thickness(duration, property);


        public abstract StateDrivenAnimation.AnimationArgument Finish();
        
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
    public abstract class PropertyBuilder<TValue> : PropertyBuilder, IStateDrivenPropertyAnimationBuilder<TValue>
    {
        protected PropertyBuilder(Action<StateDrivenAnimation.AnimationArgument> expr)
        {
            DisposeAction = expr;
        }
        
        protected abstract StateDrivenAnimation.AnimationArgument CreateArgument();
        
        public IStateDrivenPropertyAnimationBuilder<TValue> Next(VisualState state, TValue value)
        {
            Mapper.TryAdd(state, value);
            return this;
        }


        public override StateDrivenAnimation.AnimationArgument Finish()
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
        public Action<StateDrivenAnimation.AnimationArgument> DisposeAction { get; }
        

        public Dictionary<VisualState, TValue> Mapper { get; protected init; }
    }
}
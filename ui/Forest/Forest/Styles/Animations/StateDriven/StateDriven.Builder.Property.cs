using System.Windows;
using System.Windows.Media;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public abstract class PropertyBuilder : IStateDrivenPropertyAnimationBuilder
    {
        public IStateDrivenPropertyAnimationBuilder<Color?> NextColor(DependencyProperty property) => TargetContext.Color(property);

        public IStateDrivenPropertyAnimationBuilder<Color?> NextColor(params object[] property)=> TargetContext.Color(property);

        public IStateDrivenPropertyAnimationBuilder<double?> NextDouble(DependencyProperty property)=> TargetContext.Double(property);

        public IStateDrivenPropertyAnimationBuilder<double?> NextDouble(params object[] property)=> TargetContext.Double(property);

        public IStateDrivenPropertyAnimationBuilder<object> NextObject(DependencyProperty property)=> TargetContext.Object(property);

        public IStateDrivenPropertyAnimationBuilder<object> NextObject(params object[] property)=> TargetContext.Object(property);

        public IStateDrivenPropertyAnimationBuilder<Thickness?> NextThickness(DependencyProperty property)=> TargetContext.Thickness(property);

        public IStateDrivenPropertyAnimationBuilder<Thickness?> NextThickness(params object[] property)=> TargetContext.Thickness(property);

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

        public abstract void Dispose();
    }

    public class PropertyBuilder<TValue> : PropertyBuilder, IStateDrivenPropertyAnimationBuilder<TValue>
    {
        public IStateDrivenPropertyAnimationBuilder<TValue> Next(VisualState state, TValue value)
        {
            return this;
        }

        public sealed override void Dispose()
        {
            
        }

        public Dictionary<VisualState, TValue> Mapper { get; } = new Dictionary<VisualState, TValue>();
    }
}
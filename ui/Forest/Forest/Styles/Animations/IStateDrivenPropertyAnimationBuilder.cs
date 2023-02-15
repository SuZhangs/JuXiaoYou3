using System.Windows;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// <see cref="IStateDrivenPropertyAnimationBuilder"/> 接口表示一个属性动画构造器。
    /// </summary>
    public interface IStateDrivenPropertyAnimationBuilder
    {
        /// <summary>
        /// 下一个属性
        /// </summary>
        /// <returns>返回当前上下文环境的目标构造器。</returns>
        IStateDrivenTargetBuilder NextProperty();
        
        /// <summary>
        /// 目标构造器上下文。
        /// </summary>
        IStateDrivenTargetBuilder TargetContext { get; init; }
        
        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        IStateDrivenAnimatorBuilder AnimatorContext { get;init; }
    }

    /// <summary>
    /// <see cref="IStateDrivenPropertyAnimationBuilder{T}"/> 接口表示一个属性动画构造器。
    /// </summary>
    public interface IStateDrivenPropertyAnimationBuilder<in T> : IStateDrivenPropertyAnimationBuilder
    {
        /// <summary>
        /// 下一个状态。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="value">指定的值。</param>
        /// <returns></returns>
        IStateDrivenPropertyAnimationBuilder<T> Next(VisualState state, T value);
    }
}
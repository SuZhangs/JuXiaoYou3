using System.Windows;
using System.Windows.Media;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// <see cref="IStateDrivenPropertyAnimationBuilder"/> 接口表示一个属性动画构造器。
    /// </summary>
    public interface IStateDrivenPropertyAnimationBuilder : IDisposable
    {
        /// <summary>
        /// 下一个属性
        /// </summary>
        /// <returns>返回当前上下文环境的目标构造器。</returns>
        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <param name="duration">动画时长</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Color?> NextColor(DependencyProperty property, Duration duration);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="duration">动画时长</param>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Color?> NextColor(Duration duration, params object[] property);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <param name="duration">动画时长</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<double?> NextDouble(DependencyProperty property, Duration duration);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="duration">动画时长</param>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<double?> NextDouble(Duration duration, params object[] property);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <param name="duration">动画时长</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<object> NextObject(DependencyProperty property, Duration duration);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="duration">动画时长</param>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<object> NextObject(Duration duration, params object[] property);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <param name="duration">动画时长</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Thickness?> NextThickness(DependencyProperty property, Duration duration);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="duration">动画时长</param>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Thickness?> NextThickness(Duration duration, params object[] property);

        /// <summary>
        /// 目标构造器上下文。
        /// </summary>
        IStateDrivenTargetBuilder TargetContext { get; init; }

        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        IStateDrivenAnimatorBuilder AnimatorContext { get; init; }
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
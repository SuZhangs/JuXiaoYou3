using System.Windows;
using System.Windows.Media;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    
    /// <summary>
    /// <see cref="IStateDrivenTargetBuilder"/> 接口表示一个目标构造器。
    /// </summary>
    public interface IStateDrivenTargetBuilder : IDisposable
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
        IStateDrivenPropertyAnimationBuilder<Color?> Color(DependencyProperty property, Duration duration);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="duration">动画时长</param>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Color?> Color(Duration duration, params object[] property);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="duration">动画时长</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Color?> Foreground(Duration duration);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <param name="duration">动画时长</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<double?> Double(DependencyProperty property, Duration duration);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="duration">动画时长</param>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<double?> Double(Duration duration, params object[] property);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <param name="duration">动画时长</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<object> Object(DependencyProperty property, Duration duration);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="duration">动画时长</param>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<object> Object(Duration duration, params object[] property);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <param name="duration">动画时长</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(DependencyProperty property, Duration duration);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="duration">动画时长</param>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(Duration duration, params object[] property);
        
        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get;init; }
    }
}
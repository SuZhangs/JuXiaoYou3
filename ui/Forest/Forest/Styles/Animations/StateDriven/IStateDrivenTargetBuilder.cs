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
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Color?> Color(DependencyProperty property);
        
        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Color?> Color(params object[] property);
        
        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<double?> Double(DependencyProperty property);
        
        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<double?> Double(params object[] property);
        
        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<object> Object(DependencyProperty property);
        
        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<object> Object(params object[] property);

        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(DependencyProperty property);
        
        /// <summary>
        /// 获取一个属性动画构造器。
        /// </summary>
        /// <param name="property">指定的属性</param>
        /// <returns>返回一个属性动画构造器。</returns>
        IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(params object[] property);
        
        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get;init; }
    }
}
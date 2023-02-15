﻿using System.Windows;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// <see cref="IStateDrivenAnimationBuilder"/> 接口表示一个动画构建器。
    /// </summary>
    public interface IStateDrivenAnimatorBuilder
    {
        /// <summary>
        /// 获取目标默认值构造器。
        /// </summary>
        /// <param name="element">目标元素。</param>
        /// <returns>返回目标默认值构造器。</returns>
        IStateDrivenTargetAndDefaultBuilder TargetAndDefault(UIElement element);
        
        /// <summary>
        /// 获取一个新的目标构造器。
        /// </summary>
        /// <returns>返回一个新的目标构造器。</returns>
        IStateDrivenTargetBuilder Target(UIElement element);
    }
}
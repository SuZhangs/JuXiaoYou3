﻿namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public interface IStateDrivenAnimator
    {
        /// <summary>
        /// 进入初始状态
        /// </summary>
        void NextState();
        
        /// <summary>
        /// 进入下一个状态
        /// </summary>
        /// <param name="state">下一个状态</param>
        void NextState(VisualState state);
    }
}
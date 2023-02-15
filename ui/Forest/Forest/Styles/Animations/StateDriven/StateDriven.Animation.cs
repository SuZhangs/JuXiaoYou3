using Acorisoft.FutureGL.Forest.Enums;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public abstract class StateDrivenAnimation
    {
        /// <summary>
        /// 进入下一个状态
        /// </summary>
        /// <param name="visualState">下一个状态</param>
        public abstract void NextState(VisualState visualState);
    }
}
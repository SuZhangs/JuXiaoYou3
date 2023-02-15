using System.Linq;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Enums;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// <see cref="StateDrivenAnimator"/> 基于状态的动画引擎。
    /// </summary>
    public class StateDrivenAnimator : Animator
    {
        public StateDrivenAnimator()
        {
            Animations = new List<StateDrivenAnimation>(16);
            FirstState = new List<FirstStateAnimation>(16);
        }

        /// <summary>
        /// 进入初始状态
        /// </summary>
        public sealed override void NextState()
        {
            foreach (var animation in Animations)
            {
                animation.NextState();
            }
            
            foreach (var animation in FirstState)
            {
                animation.NextState();
            }
        }

        /// <summary>
        /// 进入下一个状态
        /// </summary>
        /// <param name="visualState">下一个状态</param>
        public sealed override void NextState(VisualState visualState)
        {
            foreach (var animation in Animations)
            {
                animation.NextState(visualState);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IList<FirstStateAnimation> FirstState { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public IList<StateDrivenAnimation> Animations { get; }
    }
}
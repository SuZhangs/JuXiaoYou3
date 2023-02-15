using System.Linq;
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
        }

        /// <summary>
        /// 进入初始状态
        /// </summary>
        public sealed override void NextState()
        {
            var animation = Animations.FirstOrDefault();

            if (animation is not StateDrivenDefaultAnimation)
            {
                return;
            }

            animation.NextState();
        }

        /// <summary>
        /// 进入下一个状态
        /// </summary>
        /// <param name="visualState">下一个状态</param>
        public sealed override void NextState(VisualState visualState)
        {
            if (Animations.Count == 0)
            {
                return;
            }

            var hasFirstState = Animations[0] is StateDrivenDefaultAnimation;

            if (Animations.Count == 1 && hasFirstState)
            {
                return;
            }

            var iterator = hasFirstState ? Animations.Skip(1) : Animations;
            
            foreach (var animation in iterator)
            {
                animation.NextState(visualState);
            }
        }
        
        public IList<StateDrivenAnimation> Animations { get; }
    }
}
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public class _StateDrivenAnimation
    {
        private Storyboard _storyboard;

        /// <summary>
        /// 
        /// </summary>
        public void NextState()
        {
            _storyboard?.Stop(TargetElement);
            _storyboard = null;

            foreach (var argument in Arguments)
            {
                argument.NextState();
            }
        }

        /// <summary>
        /// 进入下一个状态
        /// </summary>
        /// <param name="nextState">下一个状态</param>
        public void NextState(VisualState nextState)
        {
            _storyboard?.Stop(TargetElement);
            _storyboard = new Storyboard();

            foreach (var argument in Arguments)
            {
                var animation = argument.NextState(nextState);

                if (animation is not null)
                {
                    Storyboard.SetTarget(animation, TargetElement);
                    _storyboard.Children.Add(animation);
                }
            }

            TargetElement.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
        }

        /// <summary>
        /// 目标元素。
        /// </summary>
        public FrameworkElement TargetElement { get; init; }

        /// <summary>
        /// 所有参数
        /// </summary>
        public IReadOnlyList<PropertyAnimationRuner> Arguments { get; init; }
    }
}
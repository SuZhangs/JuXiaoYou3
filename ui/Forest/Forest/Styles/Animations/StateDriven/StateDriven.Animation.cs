using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public abstract class StateDrivenAnimation
    {
        public abstract class AnimationArgument
        {
            /// <summary>
            /// 
            /// </summary>
            public abstract void NextState();
        
            /// <summary>
            /// 进入下一个状态
            /// </summary>
            /// <param name="nextState">下一个状态</param>
            public abstract AnimationTimeline NextState(VisualState nextState);
            
            /// <summary>
            /// 目标路径
            /// </summary>
            public PropertyPath PropertyPath { get; init; }
            
            /// <summary>
            /// 映射器
            /// </summary>
            public Dictionary<VisualState, Color?> Mapper { get; init; }
            
            /// <summary>
            /// 时长
            /// </summary>
            public Duration Duration { get; init; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void NextState()
        {
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
            var storyboard = new Storyboard();
            
            foreach (var argument in Arguments)
            {
                var animation = argument.NextState(nextState);
                
                if (animation is not null)
                {
                    storyboard.Children.Add(animation);
                }
            }
            
            TargetElement.BeginStoryboard(storyboard);
        }
        
        /// <summary>
        /// 目标路径
        /// </summary>
        public PropertyPath PropertyPath { get; init; }
        
        /// <summary>
        /// 目标元素。
        /// </summary>
        public FrameworkElement TargetElement { get; init; }
        
        /// <summary>
        /// 所有参数
        /// </summary>
        public IReadOnlyList<AnimationArgument> Arguments { get; init; }
    }

    public sealed class ColorAnimationArgument : StateDrivenAnimation.AnimationArgument
    {
        private VisualState _current;
        private VisualState _last;
        private Color?      _lastValue;
        private Color?      _currentValue;
        
        public override void NextState()
        {
            _current      = VisualState.Normal;
            _last         = VisualState.Normal;
            _lastValue    = Mapper[_current];
            _currentValue = Mapper[_current];
        }

        public override AnimationTimeline NextState(VisualState nextState)
        {
            if (!Mapper.TryGetValue(nextState, out var newValue))
            {
                return null;
            }

            if (nextState == _last)
            {
                //
                // back
                
            }
            else
            {
                //
                // forward
            }
            
            return null;
        }

        /// <summary>
        /// 映射器
        /// </summary>
        public Dictionary<VisualState, Color?> Mapper { get; } = new Dictionary<VisualState, Color?>();
    }
}
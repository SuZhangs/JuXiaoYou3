using System.Windows;
using System.Windows.Media;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    class AnimatorBuilder : IStateDrivenAnimatorBuilder
    {
        private int _hashCode = -1;
        
        /// <summary>
        /// 获取目标默认值构造器。
        /// </summary>
        /// <param name="element">目标元素。</param>
        /// <returns>返回目标默认值构造器。</returns>
        public IStateDrivenTargetAndDefaultBuilder TargetAndDefault(FrameworkElement element)
        {
            if (element is null)
            {
                return null;
            }

            if (_hashCode > -1)
            {
                // 同时只能操作一个。
                return null;
            }

            var hashCode = element.GetHashCode();

            if (_hashCode == hashCode)
            {
                return DefaultTargetContext;
            }

            //
            //
            _hashCode = hashCode;
            
            void DeleteExpr(FirstStateAnimation animation)
            {
                _hashCode            = -1;
                DefaultTargetContext = null;
                Animator.FirstState.Add(animation);
                DefaultTargetContext = null;
            }
            
            DefaultTargetContext = new TargetAndDefaultBuilder(DeleteExpr)
            {
                AnimatorContext = this,
                TargetElement = element
            };
            
            return DefaultTargetContext;
        }

        /// <summary>
        /// 获取一个新的目标构造器。
        /// </summary>
        /// <returns>返回一个新的目标构造器。</returns>
        public IStateDrivenTargetBuilder Target(FrameworkElement element)
        {
            if (element is null)
            {
                return null;
            }

            if (_hashCode > -1)
            {
                // 同时只能操作一个。
                return null;
            }

            var hashCode = element.GetHashCode();

            if (_hashCode == hashCode)
            {
                return TargetContext;
            }

            //
            //
            _hashCode = hashCode;
            
            void DeleteExpr(StateDrivenAnimation animation)
            {
                _hashCode     = -1;
                TargetContext = null;
                Animator.Animations.Add(animation);
            }
            
            TargetContext = new TargetBuilder(DeleteExpr)
            {
                AnimatorContext = this,
                TargetElement = element
            };
            
            return TargetContext;
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <returns></returns>
        public StateDrivenAnimator Finish()
        {
            if (TargetContext is TargetBuilder tb)
            {
                TargetContext = null;
                Animator.Animations.Add(tb.Finish());
            }

            if (DefaultTargetContext is TargetAndDefaultBuilder dtb)
            {
                DefaultTargetContext = null;
                Animator.FirstState.Add(dtb.Finish());
            }

            _hashCode = 1;
            return Animator;
        }

        /// <summary>
        /// 
        /// </summary>
        public IStateDrivenTargetBuilder TargetContext { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IStateDrivenTargetAndDefaultBuilder DefaultTargetContext { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public StateDrivenAnimator Animator { get; init; }

        public void Reset() => _hashCode = -1;
    }
}

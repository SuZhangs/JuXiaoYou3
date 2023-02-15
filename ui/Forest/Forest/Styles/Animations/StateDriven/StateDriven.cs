using System.Windows;
using System.Windows.Media;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    
    public static class StateDriven
    {
        /// <summary>
        /// 获取目标默认值构造器。
        /// </summary>
        /// <param name="builder">构造器。</param>
        /// <param name="element">目标元素。</param>
        /// <returns>返回目标默认值构造器。</returns>
        public static IStateDrivenTargetAndDefaultBuilder Continue(this IStateDrivenTargetAndDefaultBuilder builder, FrameworkElement element)
        {
            (builder.AnimatorContext as AnimatorBuilder)?.Reset();
            builder.Dispose();
            return builder.AnimatorContext.TargetAndDefault(element);
        }

        /// <summary>
        /// 获取目标构造器。
        /// </summary>
        /// <returns>返回目标构造器。</returns>
        public static IStateDrivenTargetBuilder Target(this IStateDrivenTargetAndDefaultBuilder builder, FrameworkElement element)
        {
            (builder.AnimatorContext as AnimatorBuilder)?.Reset();
            builder.Dispose();
            return builder.AnimatorContext.Target(element);
        }

        /// <summary>
        /// 获取一个新的目标构造器。
        /// </summary>
        /// <param name="builder">构造器。</param>
        /// <param name="element">目标元素。</param>
        /// <returns>返回一个新的目标构造器。</returns>
        public static IStateDrivenTargetBuilder NextElement(this IStateDrivenPropertyAnimationBuilder builder, FrameworkElement element)
        {
            (builder.AnimatorContext as AnimatorBuilder)?.Reset();
            builder.Dispose();
            var tc = builder.TargetContext;
            tc.Dispose();
            
            return builder.AnimatorContext.Target(element);
        }

        /// <summary>
        /// 完成构造。
        /// </summary>
        /// <param name="builder">构造器。</param>
        public static void Finish(this IStateDrivenPropertyAnimationBuilder builder)
        {
            if (builder is not PropertyBuilder pb)
            {
                return;
            }

            var ac = pb.AnimatorContext;
            var tc = pb.TargetContext;
            tc.Dispose();
            ac.Finish();
        }
    }
}
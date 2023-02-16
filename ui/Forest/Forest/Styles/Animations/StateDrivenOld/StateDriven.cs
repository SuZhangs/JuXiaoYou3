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
            builder.Finish();
            return builder.AnimatorContext.TargetAndDefault(element);
        }

        /// <summary>
        /// 获取目标构造器。
        /// </summary>
        /// <returns>返回目标构造器。</returns>
        public static IStateDrivenTargetBuilder Target(this IStateDrivenTargetAndDefaultBuilder builder, FrameworkElement element)
        {
            builder.Finish();
            return builder.AnimatorContext.Target(element);
        }

        /// <summary>
        /// 获取一个新的目标构造器。
        /// </summary>
        /// <param name="builder">构造器。</param>
        /// <param name="element">目标元素。</param>
        /// <returns>返回一个新的目标构造器。</returns>
        public static IStateDrivenTargetBuilder NextElement(this IStateDrivenAnimationBuilder builder, FrameworkElement element)
        {
            builder.Finish();
            return builder.Context.AnimatorContext.Target(element);
        }

        /// <summary>
        /// 完成构造。
        /// </summary>
        /// <param name="builder">构造器。</param>
        public static void Finish(this IStateDrivenAnimationBuilder builder)
        {
            builder.Build();
        }
    }
}
using System.Windows;
using System.Windows.Media;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    class AnimatorBuilder : IStateDrivenAnimatorBuilder
    {
        public IStateDrivenTargetAndDefaultBuilder TargetAndDefault(UIElement element)
        {
            throw new NotImplementedException();
        }

        public IStateDrivenTargetBuilder Target(UIElement element)
        {
            throw new NotImplementedException();
        }
    }

    public class TargetBuilder : IStateDrivenTargetBuilder
    {
        public IStateDrivenPropertyAnimationBuilder<Color?> Color(DependencyProperty property)
        {
            throw new NotImplementedException();
        }

        public IStateDrivenPropertyAnimationBuilder<Color?> Color(params object[] property)
        {
            throw new NotImplementedException();
        }

        public IStateDrivenPropertyAnimationBuilder<double?> Double(DependencyProperty property)
        {
            throw new NotImplementedException();
        }

        public IStateDrivenPropertyAnimationBuilder<double?> Double(params object[] property)
        {
            throw new NotImplementedException();
        }

        public IStateDrivenPropertyAnimationBuilder<object> Object(DependencyProperty property)
        {
            throw new NotImplementedException();
        }

        public IStateDrivenPropertyAnimationBuilder<object> Object(params object[] property)
        {
            throw new NotImplementedException();
        }

        public IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(DependencyProperty property)
        {
            throw new NotImplementedException();
        }

        public IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(params object[] property)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get; init; }
    }

    public class TargetAndDefaultBuilder : IStateDrivenTargetAndDefaultBuilder
    {
        public IStateDrivenTargetAndDefaultBuilder Set(DependencyProperty property, object value)
        {
            throw new NotImplementedException();
        }

        public IStateDrivenTargetAndDefaultBuilder Continue(UIElement element)
        {
            throw new NotImplementedException();
        }

        public IStateDrivenTargetBuilder Target(UIElement element)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get; init; }
    }

    public abstract class PropertyBuilder : IStateDrivenPropertyAnimationBuilder
    {
        public IStateDrivenTargetBuilder NextProperty()
        {
            throw new NotImplementedException();
        }

        public IStateDrivenTargetBuilder NextElement(UIElement element)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 目标构造器上下文。
        /// </summary>
        public IStateDrivenTargetBuilder TargetContext { get; init; }

        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get; init; }
    }

    public class ColorPropertyBuilder : PropertyBuilder
    {
    }

    public class DoublePropertyBuilder : PropertyBuilder
    {
    }

    public class ObjectPropertyBuilder : PropertyBuilder
    {
    }

    public class ThicknessPropertyBuilder : PropertyBuilder
    {
    }

}
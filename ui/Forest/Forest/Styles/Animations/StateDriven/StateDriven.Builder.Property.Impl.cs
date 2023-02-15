using System.Windows;
using System.Windows.Media;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public class ColorPropertyBuilder : PropertyBuilder<Color?>
    {
        public ColorPropertyBuilder(Action<StateDrivenAnimation> expr) : base(expr)
        {
        }

        protected override StateDrivenAnimation.AnimationArgument CreateArgument(VisualState state, Color? value)
        {
            throw new NotImplementedException();
        }
    }
    
    public class ThicknessPropertyBuilder : PropertyBuilder<Thickness?>
    {
        public ThicknessPropertyBuilder(Action<StateDrivenAnimation> expr) : base(expr)
        {
        }

        protected override StateDrivenAnimation.AnimationArgument CreateArgument(VisualState state, Thickness? value)
        {
            throw new NotImplementedException();
        }
    }
    
    public class DoublePropertyBuilder : PropertyBuilder<double?>
    {
        public DoublePropertyBuilder(Action<StateDrivenAnimation> expr) : base(expr)
        {
        }

        protected override StateDrivenAnimation.AnimationArgument CreateArgument(VisualState state, double? value)
        {
            throw new NotImplementedException();
        }
    }
    
    public class ObjectPropertyBuilder : PropertyBuilder<object>
    {
        public ObjectPropertyBuilder(Action<StateDrivenAnimation> expr) : base(expr)
        {
        }

        protected override StateDrivenAnimation.AnimationArgument CreateArgument(VisualState state, object value)
        {
            throw new NotImplementedException();
        }
    }
}
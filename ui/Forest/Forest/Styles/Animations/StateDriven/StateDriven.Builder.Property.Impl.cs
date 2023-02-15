using System.Windows;
using System.Windows.Media;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public class ColorPropertyBuilder : PropertyBuilder<Color?>
    {
        public ColorPropertyBuilder(Action<StateDrivenAnimation.AnimationArgument> expr) : base(expr)
        {
            Mapper = new Dictionary<VisualState, Color?>();
        }

        protected override StateDrivenAnimation.AnimationArgument CreateArgument()
        {
            return new ColorAnimationArgument
            {
                Duration     = Duration,
                PropertyPath = PropertyPath,
                Mapper       = Mapper
            };
        }
    }
    
    public class ThicknessPropertyBuilder : PropertyBuilder<Thickness?>
    {
        public ThicknessPropertyBuilder(Action<StateDrivenAnimation.AnimationArgument> expr) : base(expr)
        {
        }

        protected override StateDrivenAnimation.AnimationArgument CreateArgument()
        {
            return new ThicknessAnimationArgument
            {
                Duration     = Duration,
                PropertyPath = PropertyPath,
                Mapper       = Mapper
            };
        }
    }
    
    public class DoublePropertyBuilder : PropertyBuilder<double?>
    {
        public DoublePropertyBuilder(Action<StateDrivenAnimation.AnimationArgument> expr) : base(expr)
        {
        }

        protected override StateDrivenAnimation.AnimationArgument CreateArgument()
        {
            return new DoubleAnimationArgument()
            {
                Duration     = Duration,
                PropertyPath = PropertyPath,
                Mapper       = Mapper
            };
        }
    }
    
    public class ObjectPropertyBuilder : PropertyBuilder<object>
    {
        public ObjectPropertyBuilder(Action<StateDrivenAnimation.AnimationArgument> expr) : base(expr)
        {
        }

        protected override StateDrivenAnimation.AnimationArgument CreateArgument()
        {
            return new ObjectAnimationArgument()
            {
                Duration     = Duration,
                PropertyPath = PropertyPath,
                Mapper       = Mapper
            };
        }
    }
}
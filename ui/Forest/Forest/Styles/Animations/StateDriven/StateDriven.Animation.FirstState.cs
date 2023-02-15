using System.Windows;
using Acorisoft.FutureGL.Forest.Enums;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public class FirstStateAnimation
    {
        public class Setter
        {
            public DependencyProperty Property { get; init; }
            public object Value { get; init; }
        }
        
        public void NextState()
        {
            foreach (var setter in Setters)
            {
                TargetElement.SetValue(setter.Property, setter.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<Setter> Setters { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public FrameworkElement TargetElement { get; init; }
    }
}
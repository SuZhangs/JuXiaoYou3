using System.Windows;
using System.Windows.Media;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    internal class TargetBuilder : IStateDrivenTargetBuilder
    {
        public TargetBuilder(Action<StateDrivenAnimation> disposeExpr) => DisposeAction = disposeExpr;
        
        public IStateDrivenPropertyAnimationBuilder<Color?> Color(DependencyProperty property) => new PropertyBuilder<Color?>
        {
            TargetContext = this,
            AnimatorContext = AnimatorContext,
            PropertyPath = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<Color?> Color(params object[] property) => new PropertyBuilder<Color?>
        {
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<double?> Double(DependencyProperty property)=> new PropertyBuilder<double?>
        {
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<double?> Double(params object[] property)=> new PropertyBuilder<double?>
        {
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<object> Object(DependencyProperty property)=> new PropertyBuilder<object>
        {
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<object> Object(params object[] property)=> new PropertyBuilder<object>
        {
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(DependencyProperty property)=> new PropertyBuilder<Thickness?>
        {
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0)", property)
        };

        public IStateDrivenPropertyAnimationBuilder<Thickness?> Thickness(params object[] property)=> new PropertyBuilder<Thickness?>
        
        {
            TargetContext   = this,
            AnimatorContext = AnimatorContext,
            PropertyPath    = new PropertyPath("(0).(1)", property)
        };

        /// <summary>
        /// 完成并添加
        /// </summary>
        public void Dispose() => DisposeAction?.Invoke(Finish());

        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public Action<StateDrivenAnimation> DisposeAction { get; }

        public StateDrivenAnimation Finish()
        {
            throw new NotImplementedException();
        }
    }

    internal class TargetAndDefaultBuilder : IStateDrivenTargetAndDefaultBuilder
    {
        public TargetAndDefaultBuilder(Action<FirstStateAnimation> disposeExpr) => DisposeAction = disposeExpr;
        
        public IStateDrivenTargetAndDefaultBuilder Set(DependencyProperty property, object value)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 完成并添加
        /// </summary>
        public void Dispose() => DisposeAction?.Invoke(Finish());

        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public Action<FirstStateAnimation> DisposeAction { get; }

        public FirstStateAnimation Finish()
        {
            throw new NotImplementedException();
        }
    }
}
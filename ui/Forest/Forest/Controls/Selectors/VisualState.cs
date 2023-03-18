﻿namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    partial class ForestListBoxItemBase
    {
        /// <summary>
        /// 构建状态
        /// </summary>
        private void OnEnableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (bool)e.OldValue;
            var newValue = (bool)e.NewValue;

            if (!StateMachine.HasInitialized)
            {
                return;
            }

            if (oldValue && !newValue)
            {
                StateMachine.NextState(VisualState.Inactive);
            }

            if (!oldValue && newValue)
            {
                StateMachine.NextState(VisualState.Normal);
            }
        }
        
        
        protected abstract void GetTemplateChildOverride(ITemplatePartFinder finder);

        public sealed override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }
        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

        private void OnLoadedIntern(object sender, RoutedEventArgs e)
        {
            OnLoaded(sender, e);
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }

            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnUnselected(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Highlight1);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnMouseLeave(e);
        }

        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
}
namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    partial class ForestButtonBase
    {
        
        #region Button State Defnitions

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
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }

            if (!oldValue && newValue)
            {
                StateMachine.NextState(VisualState.Normal);
            }
        }


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (StateMachine.CurrentState is VisualState.Highlight2 && IsPressed)
            {
                StateMachine.NextState(IsMouseOver ? VisualStateTrigger.Back : VisualStateTrigger.Next);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight1)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            ReleaseMouseCapture();
            StateMachine.ResetState();
            base.OnMouseLeave(e);
        }

        public override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }

        #endregion
        
        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
    
    
    partial class ForestCheckBoxBase
    {
        #region Button State Defnitions

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
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }

            if (!oldValue && newValue)
            {
                StateMachine.NextState(VisualStateTrigger.Back);
            }
        }

        private void OnUncheckedIntern(object sender, RoutedEventArgs e)
        {
            if (IsEnabled)
            {
                StateMachine.NextState(IsMouseOver ? VisualState.Highlight1 : VisualState.Normal);
            }
            else
            {
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }
            
            OnUnchecked(sender, e);
        }

        private void OnCheckedIntern(object sender, RoutedEventArgs e)
        {
            if (IsEnabled)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }
            else
            {
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }
            
            OnChecked(sender, e);
        }
        
        protected virtual void OnUnchecked(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnChecked(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (IsChecked != true)
            {
                StateMachine.NextState(VisualState.Highlight1);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1)
            {
                
                ReleaseMouseCapture();
                StateMachine.ResetState();
            }
            base.OnMouseLeave(e);
        }

        public override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }

        #endregion
        
        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
    
    
    partial class ForestRadioButtonBase
    {
        #region Button State Defnitions

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
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }

            if (!oldValue && newValue)
            {
                StateMachine.NextState(VisualStateTrigger.Back);
            }
        }

        private void OnUncheckedIntern(object sender, RoutedEventArgs e)
        {
            if (IsEnabled)
            {
                StateMachine.NextState(IsMouseOver ? VisualState.Highlight1 : VisualState.Normal);
            }
            else
            {
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }
            
            OnUnchecked(sender, e);
        }

        private void OnCheckedIntern(object sender, RoutedEventArgs e)
        {
            if (IsEnabled)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }
            else
            {
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }
            
            OnChecked(sender, e);
        }
        
        protected virtual void OnUnchecked(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnChecked(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (IsChecked != true)
            {
                StateMachine.NextState(VisualState.Highlight1);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1)
            {
                
                ReleaseMouseCapture();
                StateMachine.ResetState();
            }
            base.OnMouseLeave(e);
        }

        public override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }

        #endregion
        
        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
    
    
    partial class ForestRepeatButtonBase
    {
        
        #region Button State Defnitions

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
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }

            if (!oldValue && newValue)
            {
                StateMachine.NextState(VisualStateTrigger.Back);
            }
        }


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (StateMachine.CurrentState is VisualState.Highlight2 && IsPressed)
            {
                StateMachine.NextState(IsMouseOver ? VisualStateTrigger.Back : VisualStateTrigger.Next);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight1)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            ReleaseMouseCapture();
            StateMachine.ResetState();
            base.OnMouseLeave(e);
        }

        public override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }

        #endregion
        
        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
    
    
    partial class ForestToggleButtonBase
    {
        #region Button State Defnitions

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
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }

            if (!oldValue && newValue)
            {
                StateMachine.NextState(VisualStateTrigger.Back);
            }
        }

        private void OnUncheckedIntern(object sender, RoutedEventArgs e)
        {
            if (IsEnabled)
            {
                StateMachine.NextState(IsMouseOver ? VisualState.Highlight1 : VisualState.Normal);
            }
            else
            {
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }
            
            OnUnchecked(sender, e);
        }

        private void OnCheckedIntern(object sender, RoutedEventArgs e)
        {
            if (IsEnabled)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }
            else
            {
                StateMachine.NextState(VisualStateTrigger.Disabled);
            }
            
            OnChecked(sender, e);
        }
        
        protected virtual void OnUnchecked(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnChecked(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (IsChecked != true)
            {
                StateMachine.NextState(VisualState.Highlight1);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1)
            {
                
                ReleaseMouseCapture();
                StateMachine.ResetState();
            }
            base.OnMouseLeave(e);
        }

        public override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }

        #endregion
        
        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
}
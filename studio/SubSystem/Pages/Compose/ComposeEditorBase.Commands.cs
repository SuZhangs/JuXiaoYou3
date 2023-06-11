﻿using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    public partial class ComposeEditorBase
    {
        private void UndoImpl()
        {
            if (Workspace is null)
            {
                return;
            }
            
            Workspace.Undo();
            UpdateUndoRedoCommandState();
        }
        
        private void RedoImpl()
        {
            if (Workspace is null)
            {
                return;
            }
            
            Workspace.Redo();
            UpdateUndoRedoCommandState();
        }

        private bool CanUndoImpl() => Workspace?.CanUndo() ?? false;
        private bool CanRedoImpl() => Workspace?.CanRedo() ?? false;
        
        private void UpdateUndoRedoCommandState()
        {
            RedoCommand.NotifyCanExecuteChanged();
            UndoCommand.NotifyCanExecuteChanged();
        }
        
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand SaveComposeCommand { get; }
        
        

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand NewComposeCommand { get; }
        
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand RedoCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand UndoCommand { get; }
    }
}
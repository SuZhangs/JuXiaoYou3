﻿namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public interface IWorkspaceCommandManager
    {
        bool CanUndo();
        bool CanRedo();

        void Undo();
        void Redo();
        void Heading(int level);
        void Bold();
        void Italic();
        void Table();
        void Image();
    }
}
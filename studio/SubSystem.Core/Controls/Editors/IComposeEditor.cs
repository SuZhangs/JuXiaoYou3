namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public interface IComposeEditor
    {
        void Initialize();
        
        ObservableCollection<IWorkspace> InternalWorkspaceCollection { get; }
    }
}
using System.Linq;
using Acorisoft.FutureGL.MigaStudio.Controls.Editors;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    partial class ComposeEditorBase
    {
        private readonly Dictionary<Type, IWorkspace>     WorkspaceMapping;
        private readonly ObservableCollection<IWorkspace> InternalWorkspaceCollection;
        private          IWorkspace                       _workspace;

        protected void CreateWorkspace(PartOfRtf rtf)
        {
            if (rtf is null)
            {
                return;
            }

            var workspace = EditorUtilities.CreateFromRtfPart(rtf);
            
            if (!WorkspaceMapping.TryAdd(typeof(PartOfRtf), workspace))
            {
                Xaml.Get<ILogger>()
                    .Warn("创建了重复的RTF模组");
                return;
            }
            
            InternalWorkspaceCollection.Add(workspace);
        }
        
        protected void CreateWorkspace(PartOfMarkdown md)
        {
            if (md is null)
            {
                return;
            }
            
            var workspace = EditorUtilities.CreateFromMarkdownPart(md);
            
            if (!WorkspaceMapping.TryAdd(typeof(PartOfRtf), workspace))
            {
                Xaml.Get<ILogger>()
                    .Warn("创建了重复的RTF模组");
                return;
            }
            
            InternalWorkspaceCollection.Add(workspace);
        }

        ObservableCollection<IWorkspace> IComposeEditor.InternalWorkspaceCollection => InternalWorkspaceCollection;

        void IComposeEditor.Initialize()
        {
            //
            // 初始化
            WorkspaceCollection.ForEach(x =>
            {
                x.Scheduler = Scheduler;
                x.Immutable();
            });
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Workspace"/> 属性。
        /// </summary>
        public IWorkspace Workspace
        {
            get => _workspace;
            set => SetValue(ref _workspace, value);
        }
        
        public ReadOnlyObservableCollection<IWorkspace> WorkspaceCollection { get; }
    }
}
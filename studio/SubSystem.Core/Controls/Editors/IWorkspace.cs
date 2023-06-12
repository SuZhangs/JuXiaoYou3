using System.Reactive.Concurrency;
using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public enum StateChangedEventSource
    {
        Caret,
        TextSource,
        Selection
    }

    public delegate void WorkspaceChangedEventHandler(StateChangedEventSource source, IWorkspace workspace);
    
    public interface IWorkspace : IDisposable, IUndoRedoManager
    {
        /// <summary>
        /// 获取当前的文本
        /// </summary>
        string Content { get; }
        
        /// <summary>
        /// 获取当前的调度器
        /// </summary>
        IScheduler Scheduler { get; set; }
        
        /// <summary>
        /// 变化的事件
        /// </summary>
        WorkspaceChangedEventHandler WorkspaceChanged { get; set; }
        
        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize();

        /// <summary>
        /// 
        /// </summary>
        void Active();

        /// <summary>
        /// 
        /// </summary>
        void Inactive();
    }
}
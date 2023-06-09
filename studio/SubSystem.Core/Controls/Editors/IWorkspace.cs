using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public interface IWorkspace
    {
        /// <summary>
        /// 获取当前工作区的可视性
        /// </summary>
        Visibility Visible { get; }
        
        /// <summary>
        /// 获取当前的文本
        /// </summary>
        string Content { get; }

        event EventHandler PositionChanged;
        event EventHandler TextChanged;
        event EventHandler SelectionChanged;
    }

    public abstract class Workspace : ObservableObject, IWorkspace
    {
        private Visibility _visible;

        /// <summary>
        /// 获取或设置 <see cref="Visible"/> 属性。
        /// </summary>
        public Visibility Visible
        {
            get => _visible;
            set => SetValue(ref _visible, value);
        }

        public abstract string Content { get; }
        public event EventHandler PositionChanged;
        public event EventHandler TextChanged;
        public event EventHandler SelectionChanged;
    }
}
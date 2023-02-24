using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    /// <summary>
    /// <see cref="IDialogViewModel"/> 对话框视图模型。
    /// </summary>
    public interface IDialogViewModel : IViewModel
    {
        /// <summary>
        /// 表示参数传递。
        /// </summary>
        /// <param name="arg">视图参数</param>
        void Start(Parameter arg);
        
        /// <summary>
        /// 取消命令。
        /// </summary>
        RelayCommand CancelCommand { get; }
        
        /// <summary>
        /// 标题。
        /// </summary>
        string Title { get; set; }
    }
    
}
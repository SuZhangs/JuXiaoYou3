using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    /// <summary>
    /// <see cref="IDialogService"/> 类型表示一个对话框服务
    /// </summary>
    public interface IDialogService : IAmbientService
    {
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <returns>返回一个可等待的任务</returns>
        Task<Operation<T>> Dialog<T, TViewModel>() where TViewModel : IDialogViewModel;
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Operation<T>> Dialog<T, TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel;
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Operation<T>> Dialog<T, TViewModel>(TViewModel viewModel, ViewParam parameter) where TViewModel : IDialogViewModel;
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Operation<T>> Dialog<T>(IDialogViewModel viewModel);
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Operation<T>> Dialog<T>(IDialogViewModel viewModel, ViewParam parameter);
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Operation<object>> Dialog(IDialogViewModel viewModel);
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Operation<object>> Dialog(IDialogViewModel viewModel, ViewParam parameter);
    }

    /// <summary>
    /// <see cref="IAmbientService"/> 类型表示一个环境服务
    /// </summary>
    public interface IAmbientService
    {

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Danger(string title, string content);

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Danger(string title, string content, string okButtonText, string cancelButtonText);
        
        /// <summary>
        /// 警告提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Warning(string title, string content);
        
        /// <summary>
        /// 警告提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Warning(string title, string content, string okButtonText, string cancelButtonText);
        
        /// <summary>
        /// 信息提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Info(string title, string content);
        
        /// <summary>
        /// 信息提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Info(string title, string content, string okButtonText, string cancelButtonText);
        
        /// <summary>
        /// 成功提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Success(string title, string content);
        
        
        /// <summary>
        /// 成功提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Success(string title, string content, string okButtonText, string cancelButtonText);
        
        /// <summary>
        /// 过时提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Obsolete(string title, string content);
        
        
        /// <summary>
        /// 过时提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Obsolete(string title, string content, string okButtonText, string cancelButtonText);
    }

    public interface IDialogServiceAmbient : IServiceAmbient<DialogHost>
    {
        
    }
}
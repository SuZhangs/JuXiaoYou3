using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Views;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class DialogService : IDialogService, IDialogServiceAmbient
    {
        private DialogHost _host;

        /// <summary>
        /// 设置服务响应者
        /// </summary>
        /// <param name="host"></param>
        public void SetServiceProvider(DialogHost host) => _host = host;

        #region IAmbientService

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        /// <returns></returns>
        public Task<bool> Danger(string title, string content)
        {
            return Danger(title, content, Language.ConfirmText, Language.CancelText);
        }

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Danger(string title, string content, string okButtonText, string cancelButtonText)
        {
            var dvm = new DangerViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
            };

            var result = await _host.ShowDialog(dvm, new ViewParam());
            return result.IsFinished;
        }

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        /// <returns></returns>
        public Task<bool> Warning(string title, string content)
        {
            return Warning(title, content, Language.ConfirmText, Language.CancelText);
        }

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Warning(string title, string content, string okButtonText, string cancelButtonText)
        {
            var dvm = new WarningViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
            };

            var result = await _host.ShowDialog(dvm, new ViewParam());
            return result.IsFinished;
        }
        
        /// <summary>
        /// 信息提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        public Task<bool> Info(string title, string content)
        {
            return Info(title, content, Language.ConfirmText, Language.CancelText);
        }

        /// <summary>
        /// 信息提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Info(string title, string content, string okButtonText, string cancelButtonText)
        {
            var dvm = new InfoViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
            };

            var result = await _host.ShowDialog(dvm, new ViewParam());
            return result.IsFinished;
        }
        

        /// <summary>
        /// 成功提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        /// <returns></returns>
        public Task<bool> Success(string title, string content)
        {
            return Success(title, content, Language.ConfirmText, Language.CancelText);
        }


        /// <summary>
        /// 成功提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Success(string title, string content, string okButtonText, string cancelButtonText)
        {
            var dvm = new SuccessViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
            };

            var result = await _host.ShowDialog(dvm, new ViewParam());
            return result.IsFinished;
        }
        
        /// <summary>
        /// 过时提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        /// <returns></returns>
        public Task<bool> Obsolete(string title, string content)
        {
            return Obsolete(title, content, Language.ConfirmText, Language.CancelText);
        }

        /// <summary>
        /// 过时提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Obsolete(string title, string content, string okButtonText, string cancelButtonText)
        {
            var dvm = new ObsoleteViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
            };

            var result = await _host.ShowDialog(dvm, new ViewParam());
            return result.IsFinished;
        }
        #endregion
        
        #region IDialogService

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Operation<T>> Dialog<T, TViewModel>() where TViewModel : IDialogViewModel
            => _host.ShowDialog<T>(Classes.CreateInstance<TViewModel>(), new ViewParam());

        public Task<Operation<T>> Dialog<T, TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel
            => _host.ShowDialog<T>(viewModel, new ViewParam());

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Operation<T>> Dialog<T, TViewModel>(TViewModel viewModel, ViewParam parameter) where TViewModel : IDialogViewModel
            => _host.ShowDialog<T>(viewModel, parameter);

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Operation<T>> Dialog<T>(IDialogViewModel viewModel)
            => _host.ShowDialog<T>(viewModel, new ViewParam());

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Operation<T>> Dialog<T>(IDialogViewModel viewModel, ViewParam parameter)
            => _host.ShowDialog<T>(viewModel, parameter);

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Operation<object>> Dialog(IDialogViewModel viewModel)
            => _host.ShowDialog(viewModel, new ViewParam());

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Operation<object>> Dialog(IDialogViewModel viewModel, ViewParam parameter)
            => _host.ShowDialog(viewModel, parameter);

        #endregion
    }
}
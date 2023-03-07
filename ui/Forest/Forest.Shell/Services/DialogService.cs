﻿using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Views;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class DialogService : IDialogService, IDialogServiceAmbient, IBusyService, IBusyServiceAmbient, INotifyService, INotifyServiceAmbient
    {
        class Session : IBusySession
        {
            public void Dispose()
            {
                if (Host is null)
                {
                    return;
                }

                Host.Dispatcher.Invoke(() => { Host.IsBusy = false; });

                Clean();
            }

            public void Update(string text)
            {
                if (Host is null)
                {
                    return;
                }

                Host.Dispatcher.Invoke(() =>
                {
                    //
                    // 更新
                    Host.BusyText = text;

                    if (!Host.IsBusy)
                    {
                        Host.IsBusy = true;
                    }
                });
            }

            public DialogHost Host { get; init; }
            public Action Clean { get; init; }
        }

        private const string Color = "#6F0240";

        private Session    _session;
        private DialogHost _host;

        /// <summary>
        /// 设置服务响应者
        /// </summary>
        /// <param name="host"></param>
        public void SetServiceProvider(DialogHost host) => _host = host;

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="notification">消息</param>
        public void Notify(IconNotification notification)
        {
            if (notification is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(notification.Color))
            {
                notification.Color = Color;
            }

            if (notification.Delay.TotalMilliseconds < 1000)
            {
                //
                //
                notification.Initialize();
            }

            _host.Messaging(notification);
        }

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="notification">消息</param>
        public void Notify(ImageNotification notification)
        {
            if (notification is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(notification.Color))
            {
                notification.Color = Color;
            }

            if (notification.Delay.TotalMilliseconds < 1000)
            {
                //
                //
                notification.Initialize();
            }

            _host.Messaging(notification);
        }

        /// <summary>
        /// 创建会话。
        /// </summary>
        /// <returns>返回会话</returns>
        public IBusySession CreateSession()
        {
            _session ??= new Session
            {
                Host  = _host,
                Clean = () => _session = null,
            };


            return _session;
        }

        #region IDialogAmbientService
        
        /// <summary>
        /// 通知对话框
        /// </summary>
        /// <param name="level">标题</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public Task Notify(CriticalLevel level, string title, string content)
        {
            return Notify(level, title, content, Language.ConfirmText);
        }

        /// <summary>
        /// 通知对话框
        /// </summary>
        /// <param name="level">标题</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="buttonText">确认按钮文本</param>
        /// <returns></returns>
        public async Task Notify(CriticalLevel level, string title, string content, string buttonText)
        {
            DialogViewModel dvm;

            if (level == CriticalLevel.Danger)
            {
                dvm = new DangerNotifyViewModel
                {
                    Title            = title,
                    Content          = content,
                    CancelButtonText = buttonText,
                };
            }
            else if (level == CriticalLevel.Warning)
            {
                dvm = new WarningNotifyViewModel
                {
                    Title            = title,
                    Content          = content,
                    CancelButtonText = buttonText,
                };
            }
            else if (level == CriticalLevel.Info)
            {
                dvm = new InfoViewModel
                {
                    Title            = title,
                    Content          = content,
                    CancelButtonText = buttonText,
                };
            }
            else if (level == CriticalLevel.Success)
            {
                dvm = new SuccessViewModel
                {
                    Title            = title,
                    Content          = content,
                    CancelButtonText = buttonText,
                };
            }
            else
            {
                dvm = new ObsoleteViewModel
                {
                    Title   = title,
                    Content = content,
                };
            }

            await _host.ShowDialog(dvm, new Parameter());
        }

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

            var result = await _host.ShowDialog(dvm, new Parameter());
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

            var result = await _host.ShowDialog(dvm, new Parameter());
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

            var result = await _host.ShowDialog(dvm, new Parameter());
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

            var result = await _host.ShowDialog(dvm, new Parameter());
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

            var result = await _host.ShowDialog(dvm, new Parameter());
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
        public Task<Op<T>> Dialog<T, TViewModel>() where TViewModel : IDialogViewModel
            => _host.ShowDialog<T>(Xaml.GetViewModel<TViewModel>() ?? Classes.CreateInstance<TViewModel>(), new Parameter());

        public Task<Op<T>> Dialog<T, TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel
            => _host.ShowDialog<T>(viewModel, new Parameter{ Args = new object[8]});
        
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<T>> Dialog<T, TViewModel>(Parameter parameter) where TViewModel : IDialogViewModel
        {
           return _host.ShowDialog<T>(Xaml.GetViewModel<TViewModel>() ?? Classes.CreateInstance<TViewModel>(), parameter);
        } 

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<T>> Dialog<T, TViewModel>(TViewModel viewModel, Parameter parameter) where TViewModel : IDialogViewModel
            => _host.ShowDialog<T>(viewModel, parameter);

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<T>> Dialog<T>(IDialogViewModel viewModel)
            => _host.ShowDialog<T>(viewModel, new Parameter{ Args = new object[8]});

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<T>> Dialog<T>(IDialogViewModel viewModel, Parameter parameter)
            => _host.ShowDialog<T>(viewModel, parameter);

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<object>> Dialog(IDialogViewModel viewModel)
            => _host.ShowDialog(viewModel, new Parameter{ Args = new object[8]});

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<object>> Dialog(IDialogViewModel viewModel, Parameter parameter)
            => _host.ShowDialog(viewModel, parameter);

        #endregion
    }
}
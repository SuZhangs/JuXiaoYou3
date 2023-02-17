﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Acorisoft.FutureGL.Forest.Exceptions;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;

namespace Acorisoft.FutureGL.Forest.Controls
{
    /// <summary>
    /// <see cref="DialogHost"/> 类型表示一个对话框宿主。
    /// </summary>
    public class DialogHost : ContentControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty ViewModelProperty;
        public static readonly DependencyProperty DialogProperty;
        public static readonly DependencyProperty MessageProperty;
        public static readonly DependencyProperty IsOpenedProperty;
        public static readonly DependencyProperty IsBusyProperty;
        public static readonly DependencyProperty BusyTextProperty;
        public static readonly RoutedEvent MessageOpeningEvent;
        public static readonly RoutedEvent MessageClosingEvent;

        #endregion

        static DialogHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogHost),
                new FrameworkPropertyMetadata(typeof(DialogHost)));
            
            IsBusyProperty = DependencyProperty.Register(nameof(IsBusy),
                typeof(bool),
                typeof(DialogHost),
                new PropertyMetadata(Boxing.False));

            MessageProperty = DependencyProperty.Register(nameof(Message),
                typeof(WindowMessage),
                typeof(DialogHost),
                new PropertyMetadata(null));

            BusyTextProperty = DependencyProperty.Register(nameof(BusyText),
                typeof(string),
                typeof(DialogHost),
                new PropertyMetadata("正在加载..."));
            
            IsOpenedProperty = DependencyProperty.Register(nameof(IsOpened),
                typeof(bool),
                typeof(DialogHost),
                new PropertyMetadata(Boxing.False));


            ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
                typeof(IViewModel),
                typeof(DialogHost),
                new PropertyMetadata(default(IViewModel),
                    OnViewModelChanged));

            DialogProperty = DependencyProperty.Register(nameof(Dialog),
                typeof(object),
                typeof(DialogHost),
                new PropertyMetadata(default(object)));
            
            MessageOpeningEvent = EventManager.RegisterRoutedEvent(nameof(MessageOpening), RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(DialogHost));
            
            MessageClosingEvent = EventManager.RegisterRoutedEvent(nameof(MessageClosing), RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(DialogHost)); 
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = (DialogHost)d;

            if (e.NewValue is IViewModel newValue)
            {
                var view = Xaml.Connect(newValue);

                if (view is not FrameworkElement fe)
                {
                    if( host._stack.CanCompletedOrCancel())
                        host._stack.CompletedOrCancel();
                    return;
                }
                else 
                {
                    fe.Loaded += OnDialogContentLoaded;
                }

                //
                // 设置视图
                host.Dialog   = view;
                host.IsOpened = true;
            }
            else
            {
                host.ClearValue(DialogProperty);
                host.ClearValue(ViewModelProperty);
            }
        }

        private static void OnDialogContentLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement fe)
            {
                return;
            }
            fe.Loaded -= OnDialogContentLoaded;

            if (fe.DataContext is IViewModel vm)
            {
                vm.Start();
            }
        }

        private int Milliseconds = 10;

        private readonly DialogStack _stack;
        private readonly ConcurrentQueue<WindowMessage> _queue;
        private readonly DispatcherTimer _timer;
        private int _messageCounting;
        private int _messageDelay;
        private int _animationCounting;
        private bool _hasMessagePending;

        public DialogHost()
        {
            _stack = new DialogStack();
            _queue = new ConcurrentQueue<WindowMessage>();
            _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(Milliseconds), DispatcherPriority.Normal, OnDispatchMessage, Dispatcher);
            this.Unloaded += OnUnloaded;
            
            Xaml.Get<IDialogServiceAmbient>().SetServiceProvider(this);
            Xaml.Get<IBusyServiceAmbient>().SetServiceProvider(this);
            Xaml.Get<INotifyServiceAmbient>().SetServiceProvider(this);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _queue.Clear();
        }

        private void HandlingMessagePending()
        {
            _messageCounting += Milliseconds;
                
            //
            // 不满足时。
            if (_messageCounting - Milliseconds < _messageDelay)
            {
                return;
            }
                
            //
            // 满足时
            RaiseEvent(new RoutedEventArgs
            {
                RoutedEvent = MessageClosingEvent,
            });
                
            _messageCounting   = 0;
            _animationCounting = 300 / Milliseconds;
            _hasMessagePending = false;
        }
        
        private void OnDispatchMessage(object sender, EventArgs e)
        {
            if (_hasMessagePending)
            {
                HandlingMessagePending();
                return;
            }
            
            //
            // 已经空了就退出
            if (_queue.IsEmpty)
            {
                Message = null;
                _timer.Stop();
                return;
            }

            //
            // 动画计时
            if (_animationCounting > 0)
            {
                _animationCounting--;
                return;
            }


            //
            // 尝试弹出队列
            if (!_queue.TryDequeue(out var message))
            {
                return;
            }

            Message            = message;
            Milliseconds       = 50;
            _hasMessagePending = true;
            _messageCounting   = 0;
            _messageDelay      = (int)message.Delay.TotalMilliseconds;
            _timer.Interval    = TimeSpan.FromMilliseconds(Milliseconds);
            RaiseEvent(new RoutedEventArgs
            {
                RoutedEvent = MessageOpeningEvent,
            });
        }

        public async Task<Operation<T>> ShowDialog<T>(IDialogViewModel dialog, ViewParam param)
        {
            var result = await ShowDialog(dialog, param);

            if (result is null)
            {
                return Operation<T>.Failed("参数返回空");
            }

            return result.IsFinished ?
                       Operation<T>.Success((T)result.Value) :
                       Operation<T>.Failed(result.Reason);
        }

        public async Task<Operation<object>> ShowDialog(IDialogViewModel dialog, ViewParam param)
        {
            if (dialog is null)
            {
                return Operation<object>.Failed("视图模型为空");
            }

            try
            {
                //
                // 弹出
                var previous = _stack.WaitForOpen(dialog);

                if (dialog is not __IDialogViewModel dialog2)
                {
                    return Operation<object>.Failed("这不是一个可等待的对话框");
                }

                //
                // 暂停
                if (dialog != previous)
                {
                    previous.Suspend();
                }


                //
                // 设置参数。
                param                ??= new ViewParam();
                param.ViewModelSource       =   previous;
                param.CloseHandler   =   CloseDialog;
                dialog2.CloseHandler =   CloseDialog;

                //
                //
                dialog.Start(param);


                //
                //
                Dispatcher.Invoke(() => ViewModel = dialog);

                //
                // 等待
                return await dialog2.WaitHandle.Task;
            }
            catch (DuplicateDataException)
            {
                return Operation<object>.Failed("对话框重复打开！");
            }
        }

        public void Messaging(WindowMessage message)
        {
            if (message is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(message.Color))
            {
                message.Color = "#6FA240";
            }
            
            _queue.Enqueue(message);
            _timer.Start();
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <remarks>注意：该操作不会设置返回值。也不会取消</remarks>
        public void CloseDialog()
        {
            if (_stack.Current is null)
            {
                return;
            }

            //
            //
            var finished = _stack.CompletedOrCancel();

            //
            //
            finished.Stop();

            //
            // 恢复上一个
            if (!_stack.CanCompletedOrCancel())
            {
                IsOpened  = false;
                ViewModel = null;
                return;
            }

            //
            //
            if (finished is not __IDialogViewModel dialog2)
            {
                IsOpened = false;
                return;
            }

            //
            // 设置取消
            dialog2.WaitHandle.TrySetCanceled();

            //
            // 恢复上下文
            var current = _stack.Current;

            //
            //
            current.Resume();

            //
            // 弹出
            IsOpened  = false;
            ViewModel = current;
        }

        /// <summary>
        /// 视图模型
        /// </summary>
        public IViewModel ViewModel
        {
            get => (IViewModel)GetValue(ViewModelProperty);
            internal set => SetValue(ViewModelProperty, value);
        }

        /// <summary>
        /// 实际对话框内容
        /// </summary>
        public object Dialog
        {
            get => GetValue(DialogProperty);
            private set => SetValue(DialogProperty, value);
        }

        public bool IsOpened
        {
            get => (bool)GetValue(IsOpenedProperty);
            set => SetValue(IsOpenedProperty, Boxing.Box(value));
        }
        
        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, Boxing.Box(value));
        }

        public string BusyText
        {
            get => (string)GetValue(BusyTextProperty);
            set => SetValue(BusyTextProperty, value);
        }
        
        public WindowMessage Message
        {
            get => (WindowMessage)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        
        public event RoutedEventHandler MessageOpening
        {
            add => AddHandler(MessageOpeningEvent, value);
            remove => RemoveHandler(MessageOpeningEvent, value);
        }
        
        public event RoutedEventHandler MessageClosing
        {
            add => AddHandler(MessageClosingEvent, value);
            remove => RemoveHandler(MessageClosingEvent, value);
        }
        
        public class DialogStack
        {
            private readonly Stack<IDialogViewModel> _waitingStack = new Stack<IDialogViewModel>(16);
            private readonly HashSet<int> _duplication = new HashSet<int>();

            private IDialogViewModel _current;
            private IDialogViewModel _previous;

            /// <summary>
            /// 等待会打开对话框
            /// </summary>
            /// <param name="dialog">要打开的对话框</param>
            /// <returns></returns>
            /// <exception cref="ArgumentNullException">参数为空</exception>
            /// <exception cref="DuplicateDataException">对话框重复加载</exception>
            public IDialogViewModel WaitForOpen(IDialogViewModel dialog)
            {
                if (dialog is null)
                {
                    throw new ArgumentNullException(nameof(dialog));
                }

                if (!_duplication.Add(dialog.GetHashCode()))
                {
                    throw new DuplicateDataException(nameof(dialog));
                }

                if (_current is null)
                {
                    _current = dialog;
                    return _current;
                }

                _previous = _current;
                _current  = dialog;
                _waitingStack.Push(_previous);

                return _previous;
            }

            /// <summary>
            /// 完成或者取消
            /// </summary>
            /// <returns>返回完成的值。</returns>
            public IDialogViewModel CompletedOrCancel()
            {
                //
                // 注意：
                // _previous 和 _waitingStack的栈顶是等价的
                var finished = _current;

                if (_waitingStack.Count > 0)
                {
                    //
                    _previous = _waitingStack.Pop();
                    _current  = _previous;
                }
                else
                {
                    _previous = null;
                    _current  = null;
                }

                _duplication.Remove(finished.GetHashCode());

                return finished;
            }

            /// <summary>
            /// 是否可以完成或者取消
            /// </summary>
            /// <returns>返回一个值</returns>
            public bool CanCompletedOrCancel() => _waitingStack.Count > 0 || _current is not null;

            /// <summary>
            /// 当前的值
            /// </summary>
            public IDialogViewModel Current => _current;

            /// <summary>
            /// 之前的值
            /// </summary>
            public IDialogViewModel Previous => _previous;
        }
    }
}
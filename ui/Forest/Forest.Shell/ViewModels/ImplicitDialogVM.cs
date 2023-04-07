using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public abstract class ImplicitDialogVM : DialogViewModel, IInputViewModel
    {
        protected ImplicitDialogVM()
        {
            CompletedCommand = new RelayCommand(Complete);
        }

        protected void Complete()
        {
            if (CloseAction is null)
            {
                return;
            }

            if (IsCompleted())
            {
                //
                //
                Finish();

                //
                //
                if (Wait.TrySetResult(Op<object>.Success(Result)))
                {
                    //
                    // 清理现场
                    CloseAction();
                    CloseAction = null;
                }

                return;
            }


            Xaml.Get<IBuiltinDialogService>()
                .Notify(CriticalLevel.Warning, Language.NotifyText, Failed());
        }

        protected abstract bool IsCompleted();
        protected abstract void Finish();
        protected abstract string Failed();

        public sealed override void Cancel()
        {
            if (CloseAction is null)
            {
                return;
            }

            if (Wait.TrySetResult(Op<object>.Failed(Failed())))
            {
                //
                // 清理现场
                CloseAction();
                CloseAction = null;
            }
        }

        /// <summary>
        /// 结果
        /// </summary>
        public object Result { get; protected set; }

        /// <summary>
        /// 取消命令。
        /// </summary>
        public RelayCommand CompletedCommand { get; }
    }

    public abstract class BooleanDialogVM : ImplicitDialogVM
    {
        private string      _completeButtonText;
        private string      _cancelButtonText;
        #region Lifetime

        public sealed override void Start()
        {
            StartOverride();
            base.Start();
        }

        public sealed override void Resume()
        {
            var web = Xaml.Get<IWindowEventBroadcast>();
            WindowEventBroadcastDisposable = web.Keys.Subscribe(OnKeyboardInput);
            base.Resume();
        }

        public sealed override void Suspend()
        {
            WindowEventBroadcastDisposable?.Dispose();
            base.Suspend();
        }
        
        internal virtual void StartOverride()
        {
        }

        #endregion

        #region IObserver<WindowKeyEventArgs>

        
        protected override void OnKeyboardInput(WindowKeyEventArgs e)
        {
            if (!e.IsDown && IsFireCancelFromKeyEvent(e))
            {
                Cancel();
            }
        }
        
        protected virtual bool IsFireCancelFromKeyEvent(WindowKeyEventArgs value) => value.Args.Key == Key.Escape;

        #endregion

        #region InputViewModel Overrides

        protected override bool IsCompleted() => true;

        protected sealed override void Finish()
        {
            Result = Boxing.True;
        }

        protected sealed override string Failed()
        {
            Result = Boxing.False;
            return "用户取消操作";
        }

        #endregion

        /// <summary>
        /// 获取或设置 <see cref="CancelButtonText"/> 属性。
        /// </summary>
        public string CancelButtonText
        {
            get => _cancelButtonText;
            set => SetValue(ref _cancelButtonText, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="CompleteButtonText"/> 属性。
        /// </summary>
        public string CompleteButtonText
        {
            get => _completeButtonText;
            set => SetValue(ref _completeButtonText, value);
        }
    }

    public abstract class ExplicitDialogVM : DialogViewModel, IInputViewModel
    {
        protected ExplicitDialogVM()
        {
            CompletedCommand = new RelayCommand(Complete, IsCompleted);
        }

        protected void Complete()
        {
            if (CloseAction is null)
            {
                return;
            }

            if (IsCompleted())
            {
                //
                //
                Finish();

                //
                //
                if (Wait.TrySetResult(Op<object>.Success(Result)))
                {
                    //
                    // 清理现场
                    CloseAction();
                    CloseAction = null;
                }
            }
        }

        protected abstract bool IsCompleted();
        protected abstract void Finish();
        protected abstract string Failed();

        public sealed override void Cancel()
        {
            if (CloseAction is null)
            {
                return;
            }

            if (Wait.TrySetResult(Op<object>.Failed(Failed())))
            {
                //
                // 清理现场
                CloseAction();
                CloseAction = null;
            }
        }

        /// <summary>
        /// 结果
        /// </summary>
        public object Result { get; protected set; }

        /// <summary>
        /// 取消命令。
        /// </summary>
        public RelayCommand CompletedCommand { get; }
    }
}
using System.Threading.Tasks;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public abstract class InputViewModel : DialogViewModel, IInputViewModel
    {
        protected InputViewModel() : base()
        {
            CompletedCommand = new RelayCommand(Complete, IsCompleted);
        }

        protected void Complete()
        {
            if (CloseAction is null)
            {
                throw new ArgumentNullException(nameof(CloseAction));
            }

            //
            //
            Finish();

            //
            //
            if (Wait.TrySetResult(Result<object>.Success(Result)))
            {
                //
                // 清理现场
                CloseAction();
                CloseAction = null;
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

            if (Wait.TrySetResult(Result<object>.Failed(Failed())))
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

    public abstract class OperationViewModel : InputViewModel, IObserver<WindowKeyEventArgs>
    {
        private string _completeButtonText;
        private string _cancelButtonText;
        private IDisposable _disposable;

        #region Lifetime

        public sealed override Task Start()
        {
            var web = Xaml.Get<IWindowEventBroadcast>();
            _disposable = web.Keys.Subscribe(this);
            StartOverride();
            return base.Start();
        }

        public sealed override void Resume()
        {
            var web = Xaml.Get<IWindowEventBroadcast>();
            _disposable = web.Keys.Subscribe(this);
            base.Resume();
        }

        public sealed override void Suspend()
        {
            _disposable?.Dispose();
            base.Suspend();
        }

        public sealed override void Stop()
        {
            _disposable?.Dispose();
            base.Stop();
        }
        
        internal virtual void StartOverride(){}

        #endregion

        #region IObserver<WindowKeyEventArgs>

        
        void IObserver<WindowKeyEventArgs>.OnCompleted() { }
        void IObserver<WindowKeyEventArgs>.OnError(Exception error) { }

        void IObserver<WindowKeyEventArgs>.OnNext(WindowKeyEventArgs value)
        {
            if (!value.IsDown && IsFireCancelFromKeyEvent(value))
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
}
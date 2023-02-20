namespace Acorisoft.FutureGL.Forest.ViewModels
{
    // TODO: add logic for FieldViewModel
    public abstract class FieldViewModel : InputViewModel
    {
        private string     _errorMessage;
        private Visibility _visibility;

        public void NotifyPropertyUpdate()
        {
            //
            CompletedCommand.NotifyCanExecuteChanged();
            
            //
            if (IsCompleted())
            {
                Visibility = Visibility.Hidden;
                return;
            }

            _errorMessage = ReportFailedMessage();
            Visibility    = Visibility.Visible;
            RaiseUpdated(nameof(IsError));
            RaiseUpdated(nameof(ErrorMessage));
        }

        protected sealed override string Failed()
        {
            return "用户取消了操作";
        }

        /// <summary>
        /// 报告错误信息。
        /// </summary>
        /// <returns>返回错误信息。</returns>
        protected abstract string ReportFailedMessage();

        /// <summary>
        /// 获取或设置 <see cref="Visibility"/> 属性。
        /// </summary>
        public Visibility Visibility
        {
            get => _visibility;
            set => SetValue(ref _visibility, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsError"/> 属性。
        /// </summary>
        public bool IsError => IsCompleted();

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage => _errorMessage;
    }
}
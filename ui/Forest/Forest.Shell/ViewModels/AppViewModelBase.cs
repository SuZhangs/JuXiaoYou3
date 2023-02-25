using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public class AppViewModelBase : ViewModelBase, IAppViewModel
    {
        private WindowState     _windowState;

        /// <summary>
        /// 获取或设置 <see cref="WindowState"/> 属性。
        /// </summary>
        public WindowState WindowState
        {
            get => _windowState;
            set => SetValue(ref _windowState, value);
        }
    }
}
using System.Collections.ObjectModel;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class TabBased : AppViewModelBase
    {
        private IViewController _currentController;
        

        /// <summary>
        /// 表示当前的主要视图控制器。
        /// </summary>
        /// <remarks>
        /// 该属性必须为 <see cref="ITabViewController"/> 或者 <see cref="ISingleViewController"/> 类型的实例。
        /// </remarks>
        public ITabViewController Controller { get; protected init; }

        /// <summary>
        /// 表示当前的视图控制器。
        /// </summary>
        /// <remarks>
        /// 该属性必须为 <see cref="Controller"/> 属性 或者 <see cref="ISplashController"/> 类型的实例。
        /// </remarks>
        public IViewController CurrentController
        {
            get => _currentController;
            set => SetValue(ref _currentController, value);
        }
    }
}
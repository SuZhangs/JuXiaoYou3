using System;
using System.Collections.ObjectModel;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using Acorisoft.FutureGL.MigaStudio.Pages.Relationships;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class TabShell : TabController
    {
        private WindowState _windowState;

        public TabShell()
        {
            Xaml.Get<IWindowEventBroadcast>()
                .PropertyTunnel
                .WindowState = x => WindowState = x;
        }

        protected override void StartOverride()
        {
            RequireStartupTabViewModel();
        }

        protected override void RequireStartupTabViewModel()
        {
            New<HomeViewModel>();
        }

        public sealed override string Id => AppViewModel.IdOfTabShellController;

        /// <summary>
        /// 用于绑定的<see cref="WindowState"/> 属性。
        /// </summary>
        public WindowState WindowState
        {
            get => _windowState;
            set => SetValue(ref _windowState, value);
        }
    }
}
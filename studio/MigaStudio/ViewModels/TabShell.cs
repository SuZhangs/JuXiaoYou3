using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using CommunityToolkit.Mvvm.Input;

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
            New<CharacterConstraintViewModel>();
            New<ItemConstraintViewModel>();
        }

        protected override void RequireStartupTabViewModel()
        {
            New<HomeViewModel>();
        }

        public sealed override string Id => "::Main";

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
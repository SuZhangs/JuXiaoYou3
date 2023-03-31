using System;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Models;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Pages.Services;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;
using Acorisoft.FutureGL.MigaStudio.ViewModels;
using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class HomeViewModel : TabViewModel
    {
        public HomeViewModel()
        {
            Title             = "首页";
            GotoPageCommand   = Command<Type>(GotoPageImpl);
            GotoDialogCommand = AsyncCommand<Type>(GotoDialogImpl);
        }

        private void GotoPageImpl(Type type)
        {
            Controller.New(type);
        }
        
        private async Task GotoDialogImpl(Type type)
        {
            await Xaml.Get<IDialogService>()
                      .Dialog(Xaml.GetViewModel<IDialogViewModel>(type));
        }

        public sealed override bool Uniqueness => true;
        public sealed override bool Removable => false;

        public RelayCommand<Type> GotoPageCommand { get; }
        public AsyncRelayCommand<Type> GotoDialogCommand { get; }
    }
}
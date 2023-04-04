using System;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using CommunityToolkit.Mvvm.Input;

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
            await DialogService()
                      .Dialog(Xaml.GetViewModel<IDialogViewModel>(type));
        }

        public sealed override bool Uniqueness => true;
        public sealed override bool Removable => false;

        public RelayCommand<Type> GotoPageCommand { get; }
        public AsyncRelayCommand<Type> GotoDialogCommand { get; }
    }
}
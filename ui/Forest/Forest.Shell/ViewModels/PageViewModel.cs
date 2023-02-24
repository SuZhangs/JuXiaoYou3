using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public abstract class PageViewModel : ViewModelBase
    {
        protected PageViewModel()
        {
            Xaml.Get<IWindowEventBroadcast>()
                .Keys
                .Subscribe(OnKeyboardInput)
                .DisposeWith(Collector);
        }

        protected virtual void OnKeyboardInput(WindowKeyEventArgs e)
        {
        }
    }
}
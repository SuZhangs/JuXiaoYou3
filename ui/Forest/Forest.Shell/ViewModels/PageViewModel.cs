using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public abstract class PageViewModel : ViewModelBase, IViewModelLanguageService
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
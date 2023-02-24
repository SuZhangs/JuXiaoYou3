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

        #region Commands

        protected AsyncRelayCommand AsyncCommand(string name, Func<Task> execute) => Associate(name, new AsyncRelayCommand(execute));

        protected AsyncRelayCommand AsyncCommand(string name, Func<Task> execute, Func<bool> canExecute) => Associate(name, new AsyncRelayCommand(execute, canExecute));

        protected AsyncRelayCommand<T> AsyncCommand<T>(string name, Func<T, Task> execute) => Associate(name, new AsyncRelayCommand<T>(execute));

        protected AsyncRelayCommand<T> AsyncCommand<T>(string name, Func<T, Task> execute, Predicate<T> canExecute) => Associate(name, new AsyncRelayCommand<T>(execute, canExecute));

        protected RelayCommand Command(string name, Action execute) => Associate(name, new RelayCommand(execute));

        protected RelayCommand Command(string name, Action execute, Func<bool> canExecute) => Associate(name, new RelayCommand(execute, canExecute));

        #endregion

        protected virtual void OnKeyboardInput(WindowKeyEventArgs e)
        {
        }
    }
}
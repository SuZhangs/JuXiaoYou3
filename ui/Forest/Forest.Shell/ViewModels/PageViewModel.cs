using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public abstract class PageViewModel : ViewModelBase, IViewModelLanguageService
    {
        private readonly Dictionary<string, FrameworkElement> _elements;
    }
}
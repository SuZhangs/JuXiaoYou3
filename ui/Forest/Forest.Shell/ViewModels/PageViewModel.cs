using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Inputs;
using Acorisoft.FutureGL.Forest.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public abstract class PageViewModel : ViewModelBase
    {



        public override string ToString()
        {
            return $"{GetHashCode()}";
        }
    }
}
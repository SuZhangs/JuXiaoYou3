using System.Reactive;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.AppModels
{
    public interface ITabViewController : IViewController, IRootViewModel
    {
        IObservable<Unit> ItemsChanged { get; }
    }
}
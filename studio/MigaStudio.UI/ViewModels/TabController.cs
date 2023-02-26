using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class TabController : ViewModelBase, ITabViewController
    {
        private readonly Subject<Unit> _itemsChanged;
        
        protected TabController()
        {
            _itemsChanged = new Subject<Unit>();
            Onboards      = new ObservableCollection<ITabViewModel>();
            Outboards     = new ObservableCollection<ITabViewModel>();
        }

        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ITabViewModel> Onboards { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ITabViewModel> Outboards { get; }

        /// <summary>
        /// 
        /// </summary>
        public IObservable<Unit> ItemsChanged => _itemsChanged;
    }
}
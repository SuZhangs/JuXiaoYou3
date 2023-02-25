using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class TabController : ViewModelBase, ITabViewController
    {
        protected TabController()
        {
            Onboards  = new ObservableCollection<ITabViewModel>();
            Outboards = new ObservableCollection<ITabViewController>();
        }

        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ITabViewModel> Onboards { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ITabViewController> Outboards { get; }
    }
}
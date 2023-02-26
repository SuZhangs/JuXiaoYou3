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
        protected TabController()
        {
            Onboards      = new ObservableCollection<ITabViewModel>();
            Outboards     = new ObservableCollection<ITabViewModel>();
        }

        public int MaximumTabItemCount => (int)((SystemParameters.WorkArea.Width - 300) / 128);
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ITabViewModel> Onboards { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ITabViewModel> Outboards { get; }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class TabController : ViewModelBase, ITabViewController
    {
        private string _id;
        
        protected TabController()
        {
            Onboards  = new ObservableCollection<ITabViewModel>();
            Outboards = new ObservableCollection<ITabViewController>();
        }

        /// <summary>
        /// 当前的ID
        /// </summary>
        public string Id
        {
            get => _id;
            set => SetValue(ref _id, value);
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class GalleryViewModel<TItem> : TabViewModel where TItem : class
    {
        protected const int MaxItemCount     = 30;
        protected const int PageCountLimited = 512;


        private int    _pageIndex;
        private TItem _selectedItem;
        
        protected GalleryViewModel()
        {
            DataSource      = new List<TItem>(128);
            Collection      = new ObservableCollection<TItem>();
            NextPageCommand = Command(NextPageImpl, CanNextPage);
            LastPageCommand = Command(LastPageImpl, CanLastPage);
        }
        
        #region Last / Next

        private bool CanNextPage() => _pageIndex + 1 < TotalPageCount;

        private bool CanLastPage() => _pageIndex > 1;

        protected void JumpPage(int index)
        {
            if (index < 0 || index > TotalPageCount)
            {
                return;
            }

            PageIndex = index;
            PageRequest(PageIndex);
            NextPageCommand.NotifyCanExecuteChanged();
            LastPageCommand.NotifyCanExecuteChanged();
        }

        private void NextPageImpl()
        {
            PageIndex++;
            PageRequest(PageIndex);
            NextPageCommand.NotifyCanExecuteChanged();
            LastPageCommand.NotifyCanExecuteChanged();
        }

        private void LastPageImpl()
        {
            PageIndex--;
            PageRequest(PageIndex);
            NextPageCommand.NotifyCanExecuteChanged();
            LastPageCommand.NotifyCanExecuteChanged();
        }

        #endregion

        /// <summary>
        /// 需要同步数据源
        /// </summary>
        /// <returns>true为需要，false为不需要</returns>
        protected abstract bool NeedDataSourceSynchronize();
        
        /// <summary>
        /// 请求同步数据源
        /// </summary>
        /// <param name="dataSource">数据源</param>
        protected abstract void OnRequestDataSourceSynchronize(IList<TItem> dataSource);

        /// <summary>
        /// 计算当前的页面总数
        /// </summary>
        protected abstract void OnRequestComputePageCount();

        public sealed override void OnStart()
        {
            OnRequestDataSourceSynchronize(DataSource);
            OnRequestComputePageCount();
            JumpPage(PageIndex);
            
            base.OnStart();
        }

        public sealed override void Resume()
        {
            if (NeedDataSourceSynchronize())
            {
                DataSource.Clear();
                OnRequestDataSourceSynchronize(DataSource);
                OnRequestComputePageCount();
                
                //
                // 重新计算
                if (PageIndex > TotalPageCount)
                {
                    PageIndex = 1;
                }
                
                //
                //
                JumpPage(PageIndex);
            }
            
            base.Resume();
        }

        /// <summary>
        /// 页面跳转请求
        /// </summary>
        /// <param name="index">页面索引，从1-1024.</param>
        protected void PageRequest(int index)
        {
            if (DataSource.Count == 0)
            {
                return;
            }
            var skipElementCounts = (index - 1) * 30;
            var minPageItemCount  = Math.Clamp(DataSource.Count - skipElementCounts, 0, MaxItemCount);

            Collection.AddRange(DataSource.Skip(skipElementCounts)
                                          .Take(minPageItemCount), true);
            NextPageCommand.NotifyCanExecuteChanged();
            LastPageCommand.NotifyCanExecuteChanged();
        }
        
        /// <summary>
        /// 全部内容的集合
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public List<TItem> DataSource { get; }
        
        /// <summary>
        /// 支持绑定的集合
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<TItem> Collection { get; }


        /// <summary>
        /// 所有页面数量
        /// </summary>
        public int TotalPageCount { get; protected set; }


        /// <summary>
        /// 选择的内容
        /// </summary>
        public TItem SelectedItem
        {
            get => _selectedItem;
            set => SetValue(ref _selectedItem, value);
        }
        
        /// <summary>
        /// 当前索引
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set => SetValue(ref _pageIndex, value);
        }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand NextPageCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand LastPageCommand { get; }
    }
}
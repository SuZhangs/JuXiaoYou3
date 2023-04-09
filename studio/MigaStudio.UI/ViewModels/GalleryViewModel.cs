using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class GalleryViewModel<TEntity> : TabViewModel where TEntity : class
    {
        protected const int MaxItemCount     = 30;
        protected const int PageCountLimited = 512;


        private int            _pageIndex;
        private TEntity        _selectedItem;
        private OrderByMethods _orderOption;
        private bool           _isFiltering;
        private string         _filterString;


        protected GalleryViewModel()
        {
            DataSource              = new List<TEntity>(128);
            Collection              = new ObservableCollection<TEntity>();
            NextPageCommand         = Command(NextPageImpl, CanNextPage);
            LastPageCommand         = Command(LastPageImpl, CanLastPage);
            SearchPageCommand       = Command(SearchPageImpl);
            SetOrderByMethodCommand = Command<OrderByMethods>(x => OrderOption = x);
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

        #region SearchPage

        private void SearchPageImpl()
        {
            if (string.IsNullOrEmpty(FilterString))
            {
                IsFiltering = false;
            }
            
            IsFiltering = true;
            PageRequest(PageIndex);
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
        /// <param name="dataSource">需要同步的数据源</param>
        protected abstract void OnRequestDataSourceSynchronize(IList<TEntity> dataSource);

        /// <summary>
        /// 计算当前的页面总数
        /// </summary>
        /// <param name="dataSource">要计算的数据源</param>
        protected abstract void OnRequestComputePageCount(IList<TEntity> dataSource);

        #region OnStart / OnResume

        public sealed override void OnStart()
        {
            OnRequestDataSourceSynchronize(DataSource);
            OnRequestComputePageCount(DataSource);
            JumpPage(PageIndex);

            base.OnStart();
        }

        public override void Resume()
        {
            if (NeedDataSourceSynchronize())
            {
                DataSource.Clear();
                OnRequestDataSourceSynchronize(DataSource);
                OnRequestComputePageCount(DataSource);

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

        #endregion

        protected abstract IList<TEntity> BuildFilteringExpression(IList<TEntity> dataSource, string keyword, OrderByMethods sorting);
        protected abstract IList<TEntity> BuildDefaultExpression(IEnumerable<TEntity> dataSource, OrderByMethods sorting);

        /// <summary>
        /// 页面跳转请求
        /// </summary>
        /// <param name="index">页面索引，从1-1024.</param>
        protected void PageRequest(int index)
        {
            IList<TEntity> unsortedDataSource;
            
            if (IsFiltering)
            {
                unsortedDataSource = BuildFilteringExpression(DataSource, FilterString, OrderOption);
                OnRequestComputePageCount(unsortedDataSource);
                index = PageIndex;

            }
            else
            {
                unsortedDataSource = BuildDefaultExpression(DataSource, OrderOption);
            }
            

            if (unsortedDataSource.Count == 0)
            {
                return;
            }
            
            var skipElementCounts = (index - 1) * 30;
            var minPageItemCount  = Math.Clamp(unsortedDataSource.Count - skipElementCounts, 0, MaxItemCount);

            Collection.AddRange(DataSource.Skip(skipElementCounts)
                                          .Take(minPageItemCount), true);
            NextPageCommand.NotifyCanExecuteChanged();
            LastPageCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// 全部内容的集合
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public List<TEntity> DataSource { get; }

        /// <summary>
        /// 支持绑定的集合
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<TEntity> Collection { get; }


        /// <summary>
        /// 所有页面数量
        /// </summary>
        public int TotalPageCount { get; protected set; }

        /// <summary>
        /// 获取或设置 <see cref="FilterString"/> 属性。
        /// </summary>
        public string FilterString
        {
            get => _filterString;
            set => SetValue(ref _filterString, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsFiltering"/> 属性。
        /// </summary>
        public bool IsFiltering
        {
            get => _isFiltering;
            set => SetValue(ref _isFiltering, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="OrderOption"/> 属性。
        /// </summary>
        public OrderByMethods OrderOption
        {
            get => _orderOption;
            set => SetValue(ref _orderOption, value);
        }

        /// <summary>
        /// 选择的内容
        /// </summary>
        public TEntity SelectedItem
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
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand SearchPageCommand { get; }
        public RelayCommand<OrderByMethods> SetOrderByMethodCommand { get; }
    }
}
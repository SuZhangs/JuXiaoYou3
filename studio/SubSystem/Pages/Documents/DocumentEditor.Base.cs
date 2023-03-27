using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaUtils.Collections;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public abstract class DocumentEditorVMBase : TabViewModel
    {
        #region Fields

        #region SubViews

        private HeaderedSubView  _selectedSubView;
        private FrameworkElement _subView;

        #endregion

        #endregion

        protected DocumentEditorVMBase()
        {
            InternalSubViews = new ObservableCollection<HeaderedSubView>();
            SubViews         = new ReadOnlyCollection<HeaderedSubView>(InternalSubViews);
            Initialize();
        }

        private void Initialize()
        {
            CreateSubViews(InternalSubViews);
        }

        protected static void AddSubView<TView>(ICollection<HeaderedSubView> collection, string id, bool caching = true) where TView : FrameworkElement
        {
            collection.Add(new HeaderedSubView
            {
                Caching = caching,
                Name    = Language.GetText(id),
                Type    = typeof(TView)
            });
        }

        #region Override Methods

        #region SubViews

        /// <summary>
        /// 创建子页面
        /// </summary>
        /// <param name="collection">集合</param>
        protected abstract void CreateSubViews(ICollection<HeaderedSubView> collection);

        #endregion

        #endregion

        #region Properties

        #region SubViews

        /// <summary>
        /// 获取或设置 <see cref="SubView"/> 属性。
        /// </summary>
        public FrameworkElement SubView
        {
            get => _subView;
            private set => SetValue(ref _subView, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedSubView"/> 属性。
        /// </summary>
        public HeaderedSubView SelectedSubView
        {
            get => _selectedSubView;
            set
            {
                SetValue(ref _selectedSubView, value);

                if (value is null)
                {
                    return;
                }

                _selectedSubView.Create(this);
                SubView = _selectedSubView.SubView;
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        protected ObservableCollection<HeaderedSubView> InternalSubViews { get; }

        /// <summary>
        /// 子页面
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public ReadOnlyCollection<HeaderedSubView> SubViews { get; }

        #endregion

        #endregion


        public DocumentType Type { get; private set; }
    }
}
using System.Collections.ObjectModel;
using System.Windows;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        private SubViewBase _selectedSubView;
        
        protected static void AddSubView<TView>(ICollection<HeaderedSubView> collection, string id) where TView : FrameworkElement
        {
            collection.Add(new HeaderedSubView
            {
                Name = Language.GetText(id),
                Type = typeof(TView)
            });
        }

        /// <summary>
        /// 创建子页面
        /// </summary>
        /// <param name="collection">集合</param>
        protected abstract void CreateSubViews(ICollection<SubViewBase> collection);
        
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
        public SubViewBase SelectedSubView
        {
            get => _selectedSubView;
            set
            {
                OnSubViewChanged(_selectedSubView, value);
                SetValue(ref _selectedSubView, value);

            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        protected ObservableCollection<SubViewBase> InternalSubViews { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ReadOnlyCollection<SubViewBase> SubViews { get; }
    }
}
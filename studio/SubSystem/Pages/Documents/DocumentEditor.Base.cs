using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.ViewModels.CustomDataParts;
using Acorisoft.FutureGL.MigaUtils.Collections;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public abstract partial class DocumentEditorVMBase : TabViewModel
    {
        //
        // SubViews
        //
        private HeaderedSubView  _selectedSubView;
        private FrameworkElement _subView;

        protected DocumentEditorVMBase()
        {
            InternalSubViews = new ObservableCollection<HeaderedSubView>();
            SubViews         = new ReadOnlyCollection<HeaderedSubView>(InternalSubViews);
            CustomDataParts  = new ObservableCollection<ICustomDataPart>();
            Initialize();
        }

        private void Initialize()
        {
            CreateSubViews(InternalSubViews);
            SelectedSubView        = InternalSubViews.FirstOrDefault();
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

        protected override void OnStart(NavigationParameter parameter)
        {
            // TODO:
            base.OnStart(parameter);
        }

        public override void OnStart()
        {
            SelectedCustomDataPart = CustomDataParts.FirstOrDefault();
            base.OnStart();
        }

        /// <summary>
        /// 创建子页面
        /// </summary>
        /// <param name="collection">集合</param>
        protected abstract void CreateSubViews(ICollection<HeaderedSubView> collection);

        #endregion

        #region Properties

        //---------------------------------------------
        //
        // SubViews
        //
        //---------------------------------------------
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

                if (_selectedSubView is null)
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

        //---------------------------------------------
        //
        // CustomDataParts
        //
        //---------------------------------------------
        #region CustomDataParts

        private ICustomDataPart   _selectedCustomDataPart;
        private ICustomDataPartUI _customDataPart;

        /// <summary>
        /// 获取或设置 <see cref="CustomDataPart"/> 属性。
        /// </summary>
        public ICustomDataPartUI CustomDataPart
        {
            get => _customDataPart;
            private set => SetValue(ref _customDataPart, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedCustomDataPart"/> 属性。
        /// </summary>
        public ICustomDataPart SelectedCustomDataPart
        {
            get => _selectedCustomDataPart;
            set
            {
                SetValue(ref _selectedCustomDataPart, value);

                if (_selectedCustomDataPart is null)
                {
                    return;
                }

                CustomDataPart = CustomDataPartUIFactory.GetUI(_selectedCustomDataPart, this);
            }
        }

        /// <summary>
        /// 自定义部件
        /// </summary>
        /// <remarks>自定义部件会出现在【设定】-【基础信息】当中，用户可以添加删除部件、调整部件顺序。</remarks>
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ICustomDataPart> CustomDataParts { get; }

        #endregion
        
        
        //---------------------------------------------
        //
        // DataParts
        //
        //---------------------------------------------

        #region DataParts

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<DataPart> FixedDataParts { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<PartOfModule> Modules { get; }

        #endregion

        #endregion


        public DocumentType Type { get; private set; }
    }
    
    partial class DocumentEditorVMBase
    {
        //
        // DocumentManager Part
        protected void OpenDocument(Document document)
        {
            //
            // Clear
            SelectedCustomDataPart = null;
            Modules.Clear();
            FixedDataParts.Clear();

            foreach (var part in document.Parts)
            {
                if (part is PartOfModule module)
                {
                    Modules.Add(module);
                }
                else if (part is ICustomDataPart custom)
                {
                    CustomDataParts.Add(custom);
                }
                else
                {
                    FixedDataParts.Add(part);
                }
            }
        }
    } 
}
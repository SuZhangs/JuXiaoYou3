using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    partial class DocumentEditorVMBase
    {
        //---------------------------------------------
        //
        // SubViews
        //
        //---------------------------------------------

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

        //---------------------------------------------
        //
        // CustomDataParts
        //
        //---------------------------------------------

        #region DetailParts


        #endregion


        //---------------------------------------------
        //
        // DataParts
        //
        //---------------------------------------------

        #region DataParts

        /// <summary>
        /// 获取或设置 <see cref="SelectedModulePart"/> 属性。
        /// </summary>
        public PartOfModule SelectedModulePart
        {
            get => _selectedModulePart;
            set
            {
                SetValue(ref _selectedModulePart, value);

                if (_selectedModulePart is null)
                {
                    return;
                }

                var selector = _selectedModulePart.Blocks
                                                  .Select(x => ModuleBlockFactory.GetDataUI(x, OnModuleBlockValueChanged));
                ContentBlocks.AddRange(selector, true);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="DetailPart"/> 属性。
        /// </summary>
        public IPartOfDetailUI DetailPart
        {
            get => _detailPartOfDetail;
            private set => SetValue(ref _detailPartOfDetail, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedDetailPart"/> 属性。
        /// </summary>
        public IPartOfDetail SelectedDetailPart
        {
            get => _selectedDetailPart;
            set
            {
                SetValue(ref _selectedDetailPart, value);

                if (_selectedDetailPart is null)
                {
                    return;
                }

                DetailPart = CustomDataPartUIFactory.GetUI(_selectedDetailPart, this);
            }
        }

        /// <summary>
        /// 自定义部件
        /// </summary>
        /// <remarks>自定义部件会出现在【设定】-【基础信息】当中，用户可以添加删除部件、调整部件顺序。</remarks>
        [NullCheck(UniTestLifetime.Constructor)] public ObservableCollection<IPartOfDetail> DetailParts { get; }
        [NullCheck(UniTestLifetime.Constructor)] public ObservableCollection<DataPart> InvisibleDataParts { get; }
        [NullCheck(UniTestLifetime.Constructor)] public ObservableCollection<PartOfModule> ModuleParts { get; }
        [NullCheck(UniTestLifetime.Constructor)] public ObservableCollection<ModuleBlockDataUI> ContentBlocks { get; }
        [NullCheck(UniTestLifetime.Constructor)] public ObservableCollection<PreviewBlock> PreviewBlocks { get; }

        #endregion


        [NullCheck(UniTestLifetime.Constructor)] public IDatabaseManager DatabaseManager { get; }
        [NullCheck(UniTestLifetime.Constructor)] public TemplateEngine TemplateEngine { get; }
        [NullCheck(UniTestLifetime.Constructor)] public ImageEngine ImageEngine { get; }
        [NullCheck(UniTestLifetime.Constructor)] public DocumentEngine DocumentEngine { get; }
    }
}
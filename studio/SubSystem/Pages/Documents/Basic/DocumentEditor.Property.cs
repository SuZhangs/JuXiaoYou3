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

        [NullCheck(UniTestLifetime.Constructor)] protected ObservableCollection<HeaderedSubView> InternalSubViews { get; }

        /// <summary>
        /// 子页面
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)] public ReadOnlyCollection<HeaderedSubView> SubViews { get; }

        #endregion

        //---------------------------------------------
        //
        // CustomDataParts
        //
        //---------------------------------------------
        
        #region CustomDataParts

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
        [NullCheck(UniTestLifetime.Constructor)] public ObservableCollection<ICustomDataPart> CustomDataParts { get; }

        #endregion
        
        
        //---------------------------------------------
        //
        // DataParts
        //
        //---------------------------------------------

        #region DataParts
        

        /// <summary>
        /// 获取或设置 <see cref="Module"/> 属性。
        /// </summary>
        public PartOfModule Module
        {
            get => _module;
            set
            {
                SetValue(ref _module, value);

                if (_module is null)
                {
                    return;
                }

                var selector = _module.Blocks
                                      .Select(x => ModuleBlockFactory.GetDataUI(x, OnModuleChanged));
                Blocks.AddRange(selector, true);
            }
        }

        [NullCheck(UniTestLifetime.Constructor)] public ObservableCollection<DataPart> InvisibleDataParts { get; }
        
        [NullCheck(UniTestLifetime.Constructor)] public ObservableCollection<PartOfModule> ModuleDataParts { get; }

        /// <summary>
        /// 
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)] public ObservableCollection<ModuleBlockDataUI> Blocks { get; }
        
        [NullCheck(UniTestLifetime.Constructor)] public ObservableCollection<PreviewBlock> PreviewBlocks { get; }
        
        #endregion
        
        

        

        #endregion

        [NullCheck(UniTestLifetime.Constructor)] public IDatabaseManager DatabaseManager { get; }
        
        [NullCheck(UniTestLifetime.Constructor)] public TemplateEngine TemplateEngine { get; }
        
        [NullCheck(UniTestLifetime.Constructor)] public ImageEngine ImageEngine { get; }
        
        public DocumentType Type { get; private set; }
    }
}
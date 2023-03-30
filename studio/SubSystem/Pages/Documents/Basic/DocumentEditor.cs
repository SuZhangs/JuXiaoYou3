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
    public abstract partial class DocumentEditorVMBase : TabViewModel
    {
        //
        // Fields
        //
        //                                                  ------------------
        private HeaderedSubView   _selectedSubView;        // SubViews
        private FrameworkElement  _subView;                // ------------------
        private ICustomDataPart   _selectedCustomDataPart; // CustomDataPart
        private ICustomDataPartUI _customDataPart;         // ------------------
        private Document          _document;               // Document
        private DocumentCache     _cache;                  //------------------
        private PartOfModule      _module;                 // Module
        

        protected DocumentEditorVMBase()
        {
            Blocks             = new ObservableCollection<ModuleBlockDataUI>();
            InternalSubViews   = new ObservableCollection<HeaderedSubView>();
            SubViews           = new ReadOnlyCollection<HeaderedSubView>(InternalSubViews);
            CustomDataParts    = new ObservableCollection<ICustomDataPart>();
            InvisibleDataParts = new ObservableCollection<DataPart>();
            ModuleDataParts    = new ObservableCollection<PartOfModule>();
            PreviewBlocks      = new ObservableCollection<PreviewBlock>();
            
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

        #region OnStart

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
    }
}
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
using Acorisoft.FutureGL.MigaStudio.ViewModels.CustomDataParts;
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
        // Documents
        //
        //---------------------------------------------

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

        #region DataParts

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
    
    partial class DocumentEditorVMBase
    {
        [Obsolete]
        private async Task AddModulePartImpl()
        {
            //
            // 只能添加未添加的模组
            var availableModules = TemplateEngine.TemplateCacheDB
                                                 .FindAll()
                                                 .Where(x => !_DataPartTrackerOfId.ContainsKey(x.Id));
            
            //
            // 返回用户选择的模组
            var moduleCaches = await Xaml.Get<IDialogService>()
                                         .Dialog<IEnumerable<ModuleTemplateCache>>(new ModuleSelectorViewModel(), new Parameter
                                         {
                                             Args = new object[]
                                             {
                                                 availableModules
                                             }
                                         });

            if (!moduleCaches.IsFinished)
            {
                return;
            }

            var module = moduleCaches.Value
                                     .Select(x => TemplateEngine.CreateModule(x));

            AddModules(module);
        }

        [Obsolete]
        private void AddModules(IEnumerable<PartOfModule> modules)
        {
            if (modules is null)
            {
                return;
            }

            var result = 0;
            
            foreach (var module in modules)
            {
                if (AddModule(module))
                {
                    _document.Parts.Add(module);
                    result++;
                }
            }

            if (result == 0)
            {
                Warning("没有变化");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async Task ChangeAvatarImpl()
        {
            
        }
        
        //---------------------------------------------
        //
        // Commands
        //
        //---------------------------------------------
        
        
        
        [NullCheck(UniTestLifetime.Constructor)]public AsyncRelayCommand ChangeAvatarCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand SaveDocumentCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand OpenDocumentCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand NewDocumentCommand { get; }
        
        
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand AddCustomDataPartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<ICustomDataPart> ShiftUpCustomDataPartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<ICustomDataPart> ShiftDownCustomDataPartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<ICustomDataPart> RemoveCustomDataPartCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand AddModulePartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand UpgradeModulePartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<PartOfModule> ShiftUpModulePartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<PartOfModule> ShiftDownModulePartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<PartOfModule> RemoveModulePartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<PartOfModule> RemoveAllModulePartCommand { get; }
    }
    
    partial class DocumentEditorVMBase
    {
        private readonly Dictionary<string, int>   _DataPartTrackerOfId;
        private readonly Dictionary<int, DataPart> _DataPartTrackerOfIndex;
        private readonly Dictionary<string, int>   _MetadataTrackerOfString;
        private readonly Dictionary<int, Metadata> _MetadataTrackerOfIndex;

        [Obsolete]
        private bool AddModule(PartOfModule module)
        {
            if (module is null)
            {
                return false;
            }

            var currentIndex = _document.Parts.Count;

            if (_DataPartTrackerOfId.TryAdd(module.Id, currentIndex))
            {
                _DataPartTrackerOfIndex.Add(currentIndex, module);
                _document.Parts.Add(module);
                ModuleDataParts.Add(module);
                return true;
            }

            return false;
        }

        private PartOfModule GetModuleById(string id)
        {
            var index = _DataPartTrackerOfId.TryGetValue(id, out var indexTemp) ? indexTemp : -1;
            var result = index > -1 ? _DataPartTrackerOfIndex[index] : null;
            return result as PartOfModule;
        }
        
        private PartOfModule GetModuleByIndex(int index)
        {
            var result = index > -1 ? _DataPartTrackerOfIndex[index] : null;
            return result as PartOfModule;
        }
        
        //
        // DocumentManager Part
        protected void OpenDocument(Document document)
        {
            //
            // Clear
            SelectedCustomDataPart = null;
            ModuleDataParts.Clear();
            InvisibleDataParts.Clear();

            foreach (var part in document.Parts)
            {
                if (part is PartOfModule module)
                {
                    ModuleDataParts.Add(module);
                }
                else if (part is ICustomDataPart custom)
                {
                    CustomDataParts.Add(custom);
                }
                else
                {
                    InvisibleDataParts.Add(part);
                }
            }
        }
    } 
}
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Gallery
{
    public class DocumentGalleryViewModel : TabViewModel
    {
        private const int MaxCountPerPage     = 40;
        private const int MaxCountPerPageMask = 39;
        private const int MaxPageCount        = 250;
        private const int MinPageIndex        = 1;


        private readonly ReadOnlyObservableCollection<IDataCache> _collection;

        public DocumentGalleryViewModel()
        {
            Xaml.Get<IWindowEventBroadcast>()
                .Keys
                .Subscribe(OnKeyPress)
                .DisposeWith(Collector);

            var dbMgr = Xaml.Get<IDatabaseManager>();
            DatabaseManager = dbMgr;
            ImageEngine     = dbMgr.GetEngine<ImageEngine>();
            DocumentEngine  = dbMgr.GetEngine<DocumentEngine>();
            TemplateEngine  = dbMgr.GetEngine<TemplateEngine>();
            DocumentSource  = new SourceList<IDataCache>();

            NewDocumentCommand         = AsyncCommand(NewDocumentImpl);
            SelectDocumentCommand      = Command<DocumentCache>(SelectDocumentImpl);
            ChangeAvatarCommand        = AsyncCommand<DocumentCache>(ChangeDocumentAvatarImpl, HasDocument);
            GotoTemplateGalleryCommand = AsyncCommand(GotoTemplateGalleryImpl);
            NextPageCommand            = Command(NextPageImpl, CanNextPage);
            LastPageCommand            = Command(LastPageImpl, CanLastPage);


            DocumentSource.Connect()
                          .ObserveOn(Scheduler)
                          .Bind(out _collection)
                          .Subscribe()
                          .DisposeWith(Collector);

            //
            // 初始化
            PageIndex = MinPageIndex;
        }

        #region Command

        #region Last / Next

        private bool CanNextPage() => _pageIndex + 1 < _totalPageCount;

        private bool CanLastPage() => _pageIndex > 1;

        private void NextPageImpl()
        {
            PageIndex++;
            LoadPage();
        }

        private void LastPageImpl()
        {
            PageIndex--;
            LoadPage();
        }

        #endregion


        #region Document

        private static bool HasDocument(IDataCache cache) => cache is not null;

        private async Task NewDocumentImpl()
        {
            var wizard = await SubSystem.NewDocumentWizard();

            if (!wizard.IsFinished)
            {
                return;
            }

            var cache  = wizard.Value;
            var result = DocumentEngine.AddDocument(cache);

            if (!result.IsFinished)
            {
                await Xaml.Get<IBuiltinDialogService>()
                          .Notify(CriticalLevel.Warning,
                              StringFromCode.Notify,
                              StringFromCode.GetEngineResult(result.Reason));
            }
            else
            {
                await Xaml.Get<IBuiltinDialogService>().Notify(
                    CriticalLevel.Success,
                    StringFromCode.Notify,
                    StringFromCode.OperationOfAddIsSuccess);

                Update();
                Refresh();
            }
        }


        private void SelectDocumentImpl(DocumentCache index)
        {
            if (index is null)
            {
                return;
            }

            Selected = index;

            // TODO: 进一步操作
            IsDocumentPropertyPaneOpen = Selected is not null;
        }


        private async Task ChangeDocumentAvatarImpl(DocumentCache index)
        {
            if (index is null)
            {
                return;
            }

            var r = await ImageUtilities.Avatar();

            if (!r.IsFinished)
            {
                return;
            }

            var    buffer = r.Buffer;
            var    raw    = await Pool.MD5.ComputeHashAsync(buffer);
            var    md5    = Convert.ToBase64String(raw);
            string avatar;

            if (ImageEngine.HasFile(md5))
            {
                var fr = ImageEngine.Records.FindById(md5);
                avatar = fr.Uri;
            }
            else
            {
                avatar = $"avatar_{ID.Get()}.png";
                buffer.Seek(0, SeekOrigin.Begin);
                ImageEngine.SetAvatar(buffer, avatar);

                var record = new FileRecord
                {
                    Id   = md5,
                    Uri  = avatar,
                    Type = ResourceType.Image
                };

                ImageEngine.AddFile(record);
            }

            index.Avatar = avatar;
            DocumentEngine.UpdateDocument(index);
        }

        #endregion

        #region GotoPage

        private async Task GotoTemplateGalleryImpl()
        {
            await Xaml.Get<IDialogService>()
                      .Dialog(Xaml.GetViewModel<TemplateGalleryViewModel>());
            
            // TODO: 更新模组
            if (!TemplateEngine.Activated)
            {
                TemplateEngine.Activate();
            }
            
            
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// 加载图片，确保图片的索引位于（1，250），也就是最大支持7500个设定
        /// </summary>
        /// <remarks>
        /// 注意：在加载页面之前，必须使用Update方法更新总页面数。
        /// </remarks>
        private void LoadPage()
        {
            // 值范围：[1,250]
            var index = Math.Clamp(
                _pageIndex, 
                MinPageIndex, 
                Math.Min(_totalPageCount, MaxPageCount));
            
            // Take(MaxCountPerPage)可能会出错
            // 需要Take(Math.Min(,1,MaxCountPerPage)
            var takeElementCount =  _totalElementCount > MaxCountPerPage ? MaxCountPerPage : _totalElementCount;
            
            var iterator = DocumentEngine.DocumentCacheDB
                                         .FindAll()
                                         .Skip((index - 1) * MaxCountPerPage)
                                         .Take(takeElementCount);

            // 不进行空检查，因为这个错误在发生的时候会导致整个应用无法正常运行
            DocumentSource.Clear();
            DocumentSource.AddRange(iterator);
        }

        private void Update()
        {
            _totalElementCount = DocumentEngine.DocumentCacheDB.Count();
            _totalPageCount    = (_totalElementCount + MaxCountPerPageMask) / MaxCountPerPage;
        }



        #endregion

        #region Inputs

        
        private async void OnKeyPress(WindowKeyEventArgs arg)
        {
            var keyArg = arg.Args;
            if (!arg.IsDown)
            {
                if (keyArg.Key == Key.N &&
                    keyArg.KeyboardDevice.Modifiers == ModifierKeys.Control)
                {
                    await NewDocumentImpl();
                }
            }
        }

        #endregion

        #region OnStart / Refresh / Resume

        public void Refresh()
        {
            if (!DocumentEngine.Activated)
            {
                // 在第一次加载文档引擎的时候，各项依赖属性都未刷新
                // 需要调用Update方法，更新属性
                DocumentEngine.Activate();
                Update();
            }
            else if (Version == DocumentEngine.Version)
            {
                //
                // 如果版本号相同就不需要更新，避免频繁的创建对象
                return;
            }

            //
            // 每次调用该方法，都需要同步引擎的版本和当前的版本。
            Version = DocumentEngine.Version;

            //
            // 加载内容
            LoadPage();
        }

        public override void OnStart()
        {
            Refresh();
            base.OnStart();
        }

        public override void Resume()
        {
            Refresh();
            base.Resume();
        }

        #endregion

        #region Properties

        #region Bindable Properties

        private int        _pageIndex;
        private int        _totalPageCount;
        private int        _totalElementCount;
        private IDataCache _selected;
        private bool       _isDocumentPropertyPaneOpen;

        /// <summary>
        /// 获取或设置 <see cref="IsDocumentPropertyPaneOpen"/> 属性。
        /// </summary>
        public bool IsDocumentPropertyPaneOpen
        {
            get => _isDocumentPropertyPaneOpen;
            set => SetValue(ref _isDocumentPropertyPaneOpen, value);
        }


        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public IDataCache Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="TotalPage"/> 属性。
        /// </summary>
        public int TotalPage
        {
            get => _totalPageCount;
            set
            {
                SetValue(ref _totalPageCount, value);
                LastPageCommand.NotifyCanExecuteChanged();
                NextPageCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="PageIndex"/> 属性。
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                SetValue(ref _pageIndex, value);
                LastPageCommand.NotifyCanExecuteChanged();
                NextPageCommand.NotifyCanExecuteChanged();
            }
        }

        #endregion

        /// <summary>
        /// 当前的引擎版本，用于判断是否重新加载内容。
        /// </summary>
        /// <remarks>重新加载内容，这个过程虽然对于后端是没有什么压力的，但是对于前端以及大量的IO操作则是致命性的。</remarks>
        public int Version { get; private set; }

        public SourceList<IDataCache> DocumentSource { get; }
        public ReadOnlyObservableCollection<IDataCache> Collection => _collection;
        public DocumentEngine DocumentEngine { get; }
        public TemplateEngine TemplateEngine { get; }
        public ImageEngine ImageEngine { get; }

        public IDatabaseManager DatabaseManager { get; }

        #region Commands

        public AsyncRelayCommand NewDocumentCommand { get; }
        public RelayCommand NextPageCommand { get; }
        public RelayCommand LastPageCommand { get; }

        public RelayCommand<DocumentCache> SelectDocumentCommand { get; }
        public AsyncRelayCommand<DocumentCache> ChangeAvatarCommand { get; }
        public AsyncRelayCommand GotoTemplateGalleryCommand { get; }
        
        #endregion

        #endregion
    }
}
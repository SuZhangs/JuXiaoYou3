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
        private readonly ReadOnlyObservableCollection<IDataCache> _collection;

        public DocumentGalleryViewModel()
        {
            Xaml.Get<IWindowEventBroadcast>()
                .Keys
                .Subscribe(OnKeyPress)
                .DisposeWith(Collector);

            Title           = StringFromCode.GetText($"name.{nameof(DocumentGallery)}");
            DatabaseManager = Xaml.Get<IDatabaseManager>();
            ImageEngine     = DatabaseManager.GetEngine<ImageEngine>();
            DocumentEngine  = DatabaseManager.GetEngine<DocumentEngine>();
            DocumentSource  = new SourceList<IDataCache>();

            NewDocumentCommand          = AsyncCommand(NewDocumentImpl);
            SelectDocumentCommand       = Command<DocumentCache>(SelectDocumentImpl);
            ChangeDocumentAvatarCommand = AsyncCommand<DocumentCache>(ChangeDocumentAvatarImpl, HasDocument);
            NextPageCommand             = Command(NextPageImpl, CanNextPage);
            LastPageCommand             = Command(LastPageImpl, CanLastPage);


            DocumentSource.Connect()
                          .ObserveOn(Scheduler)
                          .Bind(out _collection)
                          .Subscribe()
                          .DisposeWith(Collector);

            //
            // 初始化
            PageIndex = 1;
        }

        #region Command

        #region Last / Next

        

        private bool CanNextPage() => _pageIndex + 1 < _totalPage;

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


        #endregion

        #region Private Methods

        private void LoadPage()
        {
            var index    = Math.Clamp(_pageIndex, 1, _totalPage);
            var iterator = DocumentEngine.DocumentCacheDB.FindAll().Skip((index - 1) * 50).Take(50);

            DocumentSource.Clear();
            DocumentSource.AddRange(iterator);
        }

        private void Update()
        {
            var totalCount = DocumentEngine.DocumentCacheDB.Count();
            _totalPage = (totalCount + 49) / 50;
        }


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
                DocumentEngine.Activate();
                Update();
            }
            else if (Version == DocumentEngine.Version)
            {
                return;
            }

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

        private int                         _pageIndex;
        private int                         _totalPage;
        private IDataCache                  _selected;
        private bool                      _isDocumentPropertyPaneOpen;

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
            get => _totalPage;
            set
            {
                SetValue(ref _totalPage, value);
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
        public ImageEngine ImageEngine { get; }

        public IDatabaseManager DatabaseManager { get; }

        #region Commands

        public AsyncRelayCommand NewDocumentCommand { get; }
        public RelayCommand NextPageCommand { get; }
        public RelayCommand LastPageCommand { get; }
        
        public RelayCommand<DocumentCache> SelectDocumentCommand { get; }
        public AsyncRelayCommand<DocumentCache> ChangeDocumentAvatarCommand { get; }

        #endregion

        #endregion
    }
}
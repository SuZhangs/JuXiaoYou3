using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
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
            DocumentEngine  = DatabaseManager.GetEngine<DocumentEngine>();
            DocumentSource  = new SourceList<IDataCache>();

            NewDocumentCommand = new AsyncRelayCommand(NewDocumentImpl);
            NextPageCommand    = new RelayCommand(NextPageImpl, CanNextPage);
            LastPageCommand    = new RelayCommand(LastPageImpl, CanLastPage);


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

        private bool CanNextPage() => _pageIndex + 1 < _totalPage;
        
        private bool CanLastPage() => _pageIndex > 0;

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

        private int _pageIndex;
        private int _totalPage;

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

        public IDatabaseManager DatabaseManager { get; }

        #region Commands

        

        public AsyncRelayCommand NewDocumentCommand { get; }
        public RelayCommand NextPageCommand { get; }
        public RelayCommand LastPageCommand { get; }

        #endregion
        
        #endregion
    }
}
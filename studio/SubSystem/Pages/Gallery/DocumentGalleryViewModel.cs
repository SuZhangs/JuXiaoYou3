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

            DatabaseManager = Xaml.Get<IDatabaseManager>();
            DocumentEngine  = DatabaseManager.GetEngine<DocumentEngine>();
            DocumentSource  = new SourceList<IDataCache>();

            NewDocumentCommand = new AsyncRelayCommand(NewDocumentImpl);


            DocumentSource.Connect()
                          .ObserveOn(Scheduler)
                          .Bind(out _collection)
                          .Subscribe()
                          .DisposeWith(Collector);
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
                Refresh();
            }
        }

        public void Refresh()
        {
            if (!DocumentEngine.Activated)
            {
                DocumentEngine.Activate();
            }
            else if (Version == DocumentEngine.Version)
            {
                return;
            }

            Version = DocumentEngine.Version;

            DocumentSource.Clear();
            DocumentSource.AddRange(
                DocumentEngine.DocumentCacheDB
                              .FindAll()
                              .Take(50));
        }

        public override void OnStart()
        {
            Version = DocumentEngine.Version;
            Refresh();
            base.OnStart();
        }

        public override void Resume()
        {
            Refresh();
            base.Resume();
        }

        /// <summary>
        /// 当前的引擎版本，用于判断是否重新加载内容。
        /// </summary>
        /// <remarks>重新加载内容，这个过程虽然对于后端是没有什么压力的，但是对于前端以及大量的IO操作则是致命性的。</remarks>
        public int Version { get; private set; }

        public SourceList<IDataCache> DocumentSource { get; }
        public ReadOnlyObservableCollection<IDataCache> Collection => _collection;
        public DocumentEngine DocumentEngine { get; }

        public IDatabaseManager DatabaseManager { get; }

        public AsyncRelayCommand NewDocumentCommand { get; }
    }
}
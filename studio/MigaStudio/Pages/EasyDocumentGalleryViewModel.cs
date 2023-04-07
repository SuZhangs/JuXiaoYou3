using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class EasyDocumentGalleryViewModel : GalleryViewModel<DocumentCache>
    {
        private int _version;
        
        public EasyDocumentGalleryViewModel()
        {
            var dbMgr = Xaml.Get<IDatabaseManager>();
            DatabaseManager = dbMgr;
            DocumentEngine  = dbMgr.GetEngine<DocumentEngine>();
            DocumentSource  = new List<DocumentCache>(128);
            Collection      = new ObservableCollection<DocumentCache>();
            _version        = DocumentEngine.Version;

            NewDocumentCommand    = AsyncCommand(NewDocumentImpl);
            RemoveDocumentCommand = AsyncCommand<DocumentCache>(RemoveDocumentImpl);
            OpenDocumentCommand   = AsyncCommand<DocumentCache>(OpenDocumentImpl);
            EditDocumentCommand   = AsyncCommand<DocumentCache>(EditDocumentImpl);
        }

        protected sealed override void OnPageRequest(int index)
        {
            PageRequest(DocumentSource, Collection, index);
        }

        private void OnDocumentSourceChanged()
        {
            var count        = DocumentSource.Count;
            var maxPageCount = (count + 29) / 30;

            if (maxPageCount == 1)
            {
                PageIndex = 1;
            }
            else
            {
                PageIndex = Math.Clamp(PageIndex, 1, maxPageCount);
            }
            
            JumpPage(PageIndex);
        }

        protected override void OnStart(Parameter parameter)
        {
            Type = (DocumentType)parameter.Args[0];
            DocumentSource.AddRange(DocumentEngine.GetDocuments(Type));
            OnDocumentSourceChanged();
            base.OnStart(parameter);
        }

        public override void Resume()
        {
            if (_version != DocumentEngine.Version)
            {
                _version = DocumentEngine.Version;
                DocumentSource.Clear();
                DocumentSource.AddRange(DocumentEngine.GetDocuments(Type));
                OnDocumentSourceChanged();
            }
            
            base.Resume();
        }

        private async Task NewDocumentImpl()
        {
            await DocumentUtilities.AddDocument(DocumentEngine, Type, x =>
            {
                
            });
        }

        private async Task EditDocumentImpl(DocumentCache cache)
        {
            await DocumentUtilities.AddDocument(DocumentEngine, Type, x => { });
        }

        private async Task OpenDocumentImpl(DocumentCache cache)
        {
            await DocumentUtilities.AddDocument(DocumentEngine, Type, x => { });
        }

        private async Task RemoveDocumentImpl(DocumentCache cache)
        {
            if (cache is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            DocumentUtilities.RemoveDocument(DocumentEngine, cache, x => { Successful(SubSystemString.OperationOfRemoveIsSuccessful); });
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public List<DocumentCache> DocumentSource { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<DocumentCache> Collection { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public DocumentEngine DocumentEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public TemplateEngine TemplateEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ImageEngine ImageEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand NewDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<DocumentCache> EditDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<DocumentCache> OpenDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<DocumentCache> RemoveDocumentCommand { get; }

        public DocumentType Type { get; private set; }
    }
}
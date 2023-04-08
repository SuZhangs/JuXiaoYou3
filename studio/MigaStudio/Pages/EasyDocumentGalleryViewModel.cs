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
    public class EasyDocumentGalleryViewModelProxy : BindingProxy<EasyDocumentGalleryViewModel>{}
    
    public class EasyDocumentGalleryViewModel : GalleryViewModel<DocumentCache>
    {
        private int _version;
        
        public EasyDocumentGalleryViewModel()
        {
            var dbMgr = Xaml.Get<IDatabaseManager>();
            DatabaseManager = dbMgr;
            DocumentEngine  = dbMgr.GetEngine<DocumentEngine>();
            _version        = DocumentEngine.Version;

            NewDocumentCommand    = AsyncCommand(NewDocumentImpl);
            RemoveDocumentCommand = AsyncCommand<DocumentCache>(RemoveDocumentImpl);
            OpenDocumentCommand   = AsyncCommand<DocumentCache>(OpenDocumentImpl);
            EditDocumentCommand   = AsyncCommand<DocumentCache>(EditDocumentImpl);
        }

        protected override void OnRequestComputePageCount()
        {
            var count = DataSource.Count;
            TotalPageCount = ((count == 0 ? 1 : count) + 29) / 30;

            if (TotalPageCount == 1)
            {
                PageIndex = 1;
            }
            else
            {
                PageIndex = Math.Clamp(
                    PageIndex,
                    1,
                    Math.Min(TotalPageCount, PageCountLimited));
            }
        }

        protected sealed override void OnRequestDataSourceSynchronize(IList<DocumentCache> dataSource)
        {
            dataSource.AddRange(DocumentEngine.GetDocuments(Type));
        }

        protected sealed override void OnStart(Parameter parameter)
        {
            Type = (DocumentType)parameter.Args[0];
            base.OnStart(parameter);
        }

        protected sealed override bool NeedDataSourceSynchronize()
        {
            if (_version != DocumentEngine.Version)
            {
                _version = DocumentEngine.Version;
                return true;
            }

            return false;
        }

        private async Task NewDocumentImpl()
        {
            await DocumentUtilities.AddDocument(DocumentEngine, Type, x =>
            {
                DataSource.Add(x);
                Collection.Add(x);
                
            });
        }

        private async Task EditDocumentImpl(DocumentCache cache)
        {
            await DocumentUtilities.AddDocument(DocumentEngine, Type, x =>
            {
                
            });
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

            DocumentUtilities.RemoveDocument(DocumentEngine, cache, x =>
            {
                DataSource.Remove(x);
                Collection.Remove(x);
                Successful(SubSystemString.OperationOfRemoveIsSuccessful);
            });
        }

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
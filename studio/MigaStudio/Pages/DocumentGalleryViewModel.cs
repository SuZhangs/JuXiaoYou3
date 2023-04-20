using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class EasyDocumentGalleryViewModelProxy : BindingProxy<DocumentGalleryViewModel>{}
    
    public class DocumentGalleryViewModel : GalleryViewModel<DocumentCache>
    {
        private int  _version;
        private bool _isPropertyPaneOpen;
        
        public DocumentGalleryViewModel()
        {
            var dbMgr = Xaml.Get<IDatabaseManager>();
            DatabaseManager = dbMgr;
            DocumentEngine  = dbMgr.GetEngine<DocumentEngine>();
            ImageEngine     = dbMgr.GetEngine<ImageEngine>();
            KeywordEngine   = dbMgr.GetEngine<KeywordEngine>();
            _version        = DocumentEngine.Version;

            SelectedDocumentCommand     = CommandUtilities.CreateSelectedCommand<DocumentCache>(x =>
            {
                SelectedItem       = x;
                IsPropertyPaneOpen = true;
            });
            NewDocumentCommand          = AsyncCommand(NewDocumentImpl);
            LockOrUnlockDocumentCommand = Command<DocumentCache>(LockOrUnlockDocumentImpl);
            ChangeDocumentCommand       = AsyncCommand<DocumentCache>(ChangeDocumentImpl);
            RemoveDocumentCommand       = AsyncCommand<DocumentCache>(RemoveDocumentImpl);
            OpenDocumentCommand         = Command<DocumentCache>(OpenDocumentImpl);
            AddKeywordCommand           = AsyncCommand(AddKeywordImpl);
            RemoveKeywordCommand        = AsyncCommand<string>(RemoveKeywordImpl, x => !string.IsNullOrEmpty(x));
        }

        protected override void OnRequestComputePageCount(IList<DocumentCache> dataSource)
        {
            var count = dataSource.Count;
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

        protected override IList<DocumentCache> BuildDefaultExpression(IEnumerable<DocumentCache> dataSource, OrderByMethods sorting)
        {
            if (sorting == OrderByMethods.AscendingByName)
            {
                return dataSource.OrderBy(x => x.Name)
                                 .ToList();
            }
            
            if (sorting == OrderByMethods.DescendingByName)
            {
                return dataSource.OrderByDescending(x => x.Name)
                                 .ToList();
            }

            if (sorting == OrderByMethods.AscendingByTimeOfCreated)
            {
                return dataSource.OrderBy(x => x.TimeOfCreated)
                                 .ToList();
            }

            if (sorting == OrderByMethods.DescendingByTimeOfCreated)
            {
                
                return dataSource.OrderByDescending(x => x.TimeOfCreated)
                                 .ToList();
            }

            if (sorting == OrderByMethods.AscendingByTimeOfModified)
            {
                
                return dataSource.OrderBy(x => x.TimeOfModified)
                                 .ToList();
            }

            return dataSource.OrderByDescending(x => x.TimeOfModified)
                             .ToList();
        }


        protected override IList<DocumentCache> BuildFilteringExpression(IList<DocumentCache> dataSource, string keyword, OrderByMethods sorting)
        {
            var filteredSource = dataSource.Where(x => x.Name.Contains(keyword));
            return BuildDefaultExpression(filteredSource, sorting);
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

        public override void Resume()
        {
            base.Resume();

            if (SelectedItem is not null)
            {
                SelectedItem = DocumentUtilities.UpdateDocument(
                    DocumentEngine,
                    SelectedItem.Id, 
                    DataSource,
                    Collection);
            }
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
        
        private async Task ChangeDocumentImpl(DocumentCache cache)
        {
            await DocumentUtilities.ChangeDocument(DocumentEngine, ImageEngine, cache, _ =>
            {
                Successful(SubSystemString.OperationOfSaveIsSuccessful);
            });
        }
        
        private void LockOrUnlockDocumentImpl(DocumentCache cache)
        {
            DocumentUtilities.LockOrUnlock(DocumentEngine, cache);
        }

        private void OpenDocumentImpl(DocumentCache cache)
        {
            DocumentUtilities.OpenDocument(Controller, cache);
        }

        private async Task RemoveDocumentImpl(DocumentCache cache)
        {
            if (cache is null)
            {
                return;
            }

            if (cache.IsDeleted)
            {
                return;
            }

            if (cache.IsLocked)
            {
                await Warning(SubSystemString.CannotDelete);
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
        private async Task AddKeywordImpl()
        {
            await DocumentUtilities.AddKeyword(SelectedItem.Keywords,
                KeywordEngine,
                SetDirtyState,
                Warning);
        }

        private async Task RemoveKeywordImpl(string item)
        {
            await DocumentUtilities.RemoveKeyword(
                item, 
                SelectedItem.Keywords, 
                KeywordEngine, 
                SetDirtyState, 
                DangerousOperation);
        }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> RemoveKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public DocumentEngine DocumentEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ImageEngine ImageEngine { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public KeywordEngine KeywordEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand NewDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<DocumentCache> SelectedDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<DocumentCache> ChangeDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<DocumentCache> LockOrUnlockDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<DocumentCache> OpenDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<DocumentCache> RemoveDocumentCommand { get; }

        public DocumentType Type { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="IsPropertyPaneOpen"/> 属性。
        /// </summary>
        public bool IsPropertyPaneOpen
        {
            get => _isPropertyPaneOpen;
            set => SetValue(ref _isPropertyPaneOpen, value);
        }
    }
}
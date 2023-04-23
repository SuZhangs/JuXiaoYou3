﻿using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs;
using Acorisoft.FutureGL.MigaStudio.Pages.Compose;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class ComposeGalleryViewModelProxy : BindingProxy<ComposeGalleryViewModel>
    {
        
    }
    
    public class ComposeGalleryViewModel  : GalleryViewModel<ComposeCache>
    {
        private int _version;
        
        public ComposeGalleryViewModel()
        {
            var dbMgr = Xaml.Get<IDatabaseManager>();
            DatabaseManager = dbMgr;
            ComposeEngine   = dbMgr.GetEngine<ComposeEngine>();
            ImageEngine     = dbMgr.GetEngine<ImageEngine>();
            KeywordEngine   = dbMgr.GetEngine<KeywordEngine>();
            _version        = ComposeEngine.Version;
            
            AddComposeCommand       = AsyncCommand(AddImpl);
            RemoveComposeCommand    = AsyncCommand<ComposeCache>(RemoveImpl);
            ShiftUpComposeCommand   = Command<ComposeCache>(ShiftUpImpl);
            ShiftDownComposeCommand = Command<ComposeCache>(ShiftDownImpl);
            OpenComposeCommand      = Command<ComposeCache>(OpenImpl);
            AddKeywordCommand       = AsyncCommand(AddKeywordImpl);
            RemoveKeywordCommand    = AsyncCommand<string>(RemoveKeywordImpl, x => !string.IsNullOrEmpty(x));
        }

        protected sealed override bool NeedDataSourceSynchronize()
        {
            if (_version != ComposeEngine.Version)
            {
                _version = ComposeEngine.Version;
                return true;
            }

            return false;
        }
        
        protected override void OnRequestComputePageCount(IList<ComposeCache> dataSource)
        {
            
            var count = dataSource.Count;
            TotalPageCount = ((count == 0 ? 1 : count) + MaxItemCountMask) / MaxItemCount;

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

        protected override IList<ComposeCache> BuildDefaultExpression(IEnumerable<ComposeCache> dataSource, OrderByMethods sorting)
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


        protected override IList<ComposeCache> BuildFilteringExpression(IList<ComposeCache> dataSource, string keyword, OrderByMethods sorting)
        {
            var filteredSource = dataSource.Where(x => x.Name.Contains(keyword));
            return BuildDefaultExpression(filteredSource, sorting);
        }

        protected sealed override void OnRequestDataSourceSynchronize(IList<ComposeCache> dataSource)
        {
            dataSource.AddRange(ComposeEngine.GetComposes());
        }

        private async Task AddImpl()
        {
            var title = await StringViewModel.String(SubSystemString.EditNameTitle);

            if (!title.IsFinished)
            {
                return;
            }

            var now = DateTime.Now;
            var item = new ComposeCache
            {
                Id             = ID.Get(),
                Name          = title.Value,
                TimeOfCreated  = now,
                TimeOfModified = now,
            };
            
            Collection.Add(item);
        }

        private async Task RemoveImpl(ComposeCache index)
        {
            if (index is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Collection.Remove(index);
        }

        private void OpenImpl(ComposeCache index)
        {
            if (index is null)
            {
                return;
            }

            Controller.OpenCompose<ComposeEditorViewModel>(index);
        }

        private void ShiftDownImpl(ComposeCache index)
        {
            if (index is null)
            {
                return;
            }

            Collection.ShiftDown(index);
        }

        private void ShiftUpImpl(ComposeCache index)
        {
            if (index is null)
            {
                return;
            }

            Collection.ShiftUp(index);
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public ComposeEngine ComposeEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ImageEngine ImageEngine { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public KeywordEngine KeywordEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

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
        public AsyncRelayCommand AddComposeCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ComposeCache> ShiftUpComposeCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ComposeCache> ShiftDownComposeCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ComposeCache> OpenComposeCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ComposeCache> RemoveComposeCommand { get; }
    }
}
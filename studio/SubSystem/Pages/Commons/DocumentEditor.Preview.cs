﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaStudio.Models.Previews;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        private async Task AddPreviewBlockImpl()
        {
            var r = await NewPreviewBlockViewModel.New();

            if (!r.IsFinished)
            {
                return;
            }

            var b = r.Value;

            if (!await ContinueNamingImpl(b))
            {
                return;
            }

            if (!await ContinueEditImpl(b))
            {
                return;
            }

            //
            //
            PreviewPart.Blocks.Add(b);
            PreviewBlocks.Add(PreviewBlockUI.GetUI(b));
            SavePreviewPart();
            RefreshPreviewBlockImp();
        }

        private async Task<bool> ContinueNamingImpl(PreviewBlock b)
        {
            if (b is null)
            {
                return false;
            }

            var r1 = await StringViewModel.String(SubSystemString.EditNameTitle);

            if (!r1.IsFinished)
            {
                return false;
            }

            b.Name = r1.Value;
            return true;
        }

        private async Task<bool> ContinueEditImpl(PreviewBlock b)
        {
            if (b is GroupingPreviewBlock hb)
            {
                var r1 = await EditPreviewBlockViewModel.Edit(hb, Document.Parts);
                return r1.IsFinished;
            }

            if (b is RarityPreviewBlock rarity)
            {
                var r1 = await SubSystem.OptionSelection<ModuleBlock>(
                    SubSystemString.SelectTitle,
                    null,
                    Document.Parts
                            .Where(x => x is PartOfModule)
                            .OfType<PartOfModule>()
                            .Select(x => x.Blocks)
                            .SelectMany(x => x)
                            .Where(x => !string.IsNullOrEmpty(x.Metadata) && x is not GroupBlock)
                            .Where(x => x is NumberBlock or SingleLineBlock or RateBlock));

                rarity.Name          = r1.Value.Name;
                rarity.ValueSourceID = r1.Value.Metadata;
                return r1.IsFinished;
            }

            if (b is StringPreviewBlock sp)
            {
                var r1 = await EditStringPreviewBlockViewModel.Edit(sp, Document.Parts);
                return r1.IsFinished;
            }

            if (b is ChartPreviewBlock rp)
            {
                var r1 = await EditChartPreviewBlockViewModel.Edit(rp, Document.Parts);
                return r1.IsFinished;
            }

            return false;
        }

        private void SavePreviewPart()
        {
            
            
            var db = Xaml.Get<IDatabaseManager>()
                         .Database
                         .CurrentValue;
            var mmp = db.Get<ModuleManifestProperty>();
            mmp.SetPreviewManifest(Type, PreviewPart);
            db.Set(mmp);
        }

        private async Task EditPreviewBlockImpl(PreviewBlockUI block)
        {            
            if (block is null)
            {
                return;
            }
            var r = await StringViewModel.String(SubSystemString.EditNameTitle);

            if (!r.IsFinished)
            {
                return;
            }

            block.Name = r.Value;
            SavePreviewPart();
        }
        
        private void ShiftUpPreviewBlockImpl(PreviewBlockUI block)
        {
            if (block is null)
            {
                return;
            }
            PreviewBlocks.ShiftUp(block);
            PreviewPart.Blocks.ShiftUp(block.BaseSource);
            reSortPreviewBlocksImpl();
        }
        
        private void ShiftDownPreviewBlockImpl(PreviewBlockUI block)
        {            
            if (block is null)
            {
                return;
            }
            PreviewBlocks.ShiftDown(block);
            PreviewPart.Blocks.ShiftDown(block.BaseSource);
            reSortPreviewBlocksImpl();
        }
        
        private void reSortPreviewBlocksImpl()
        {
            for (var i = 0; i < PreviewBlocks.Count; i++)
            {
                PreviewBlocks[i].BaseSource
                                .Index = i;
                PreviewPart.Blocks[i].Index = i;
            }
            SavePreviewPart();
        }
        
        private async Task ExportPreviewBlockAsPictureImpl()
        {
        }

        private async Task ExportPreviewBlockAsPdfImpl()
        {
        }

        private async Task ExportPreviewBlockAsMarkdownImpl()
        {
        }

        private async Task RemovePreviewBlockImpl(PreviewBlockUI block)
        {
            if (block is null)
            {
                return;
            }
            
            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            PreviewBlocks.Remove(block);
            PreviewPart.Blocks.Remove(block.BaseSource);
            SavePreviewPart();
        }

        private void RefreshPreviewBlockImp()
        {
            //
            // 更新
            PreviewBlocks.ForEach(x => x.Update(GetMetadataById, GetBlockById));
        }

        /// <summary>
        /// 自定义部件
        /// </summary>
        /// <remarks>自定义部件会出现在【设定】-【基础信息】当中，用户可以添加删除部件、调整部件顺序。</remarks>
        [NullCheck(UniTestLifetime.Constructor)]
        public PartOfPreview PreviewPart { get; private set; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<PreviewBlockUI> PreviewBlocks { get; init; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddPreviewBlockCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand RefreshPreviewBlockCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ExportPreviewBlockAsPdfCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ExportPreviewBlockAsPictureCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ExportPreviewBlockAsMarkdownCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<PreviewBlockUI> EditPreviewBlockCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<PreviewBlockUI> ShiftUpPreviewBlockCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<PreviewBlockUI> ShiftDownPreviewBlockCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<PreviewBlockUI> RemovePreviewBlockCommand { get; }
    }
}
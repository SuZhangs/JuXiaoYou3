using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;
using Acorisoft.FutureGL.MigaStudio.Models.Presentations;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        private async Task AddPresentationImpl()
        {
            var r = await NewPresentationViewModel.New();

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
            PresentationPart.Blocks.Add(b);
            Presentations.Add(PresentationUI.GetUI(b));
            SavePresentationPart();
            RefreshPresentationImp();
        }

        private async Task<bool> ContinueNamingImpl(Presentation b)
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

        private async Task<bool> ContinueEditImpl(Presentation b)
        {
            if (b is GroupingPresentation hb)
            {
                var r1 = await EditPresentationViewModel.Edit(hb, Document.Parts);
                return r1.IsFinished;
            }

            if (b is RarityPresentation rarity)
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

            if (b is StringPresentation sp)
            {
                var r1 = await EditStringPresentationViewModel.Edit(sp, Document.Parts);
                return r1.IsFinished;
            }

            if (b is ChartPresentation rp)
            {
                var r1 = await EditChartPresentationViewModel.Edit(rp, Document.Parts);
                return r1.IsFinished;
            }

            return false;
        }

        private void SavePresentationPart()
        {
            
            
            var db = Xaml.Get<IDatabaseManager>()
                         .Database
                         .CurrentValue;
            var mmp = db.Get<ModuleManifestProperty>();
            mmp.SetPresentationManifest(Type, PresentationPart);
            db.Set(mmp);
        }

        private async Task EditPresentationImpl(PresentationUI block)
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
            SavePresentationPart();
        }
        
        private void ShiftUpPresentationImpl(PresentationUI block)
        {
            if (block is null)
            {
                return;
            }
            Presentations.ShiftUp(block);
            PresentationPart.Blocks.ShiftUp(block.BaseSource);
            reSortPresentationsImpl();
        }
        
        private void ShiftDownPresentationImpl(PresentationUI block)
        {            
            if (block is null)
            {
                return;
            }
            Presentations.ShiftDown(block);
            PresentationPart.Blocks.ShiftDown(block.BaseSource);
            reSortPresentationsImpl();
        }
        
        private void reSortPresentationsImpl()
        {
            for (var i = 0; i < Presentations.Count; i++)
            {
                Presentations[i].BaseSource
                                .Index = i;
                PresentationPart.Blocks[i].Index = i;
            }
            SavePresentationPart();
        }
        
        private async Task ExportPresentationAsPictureImpl()
        {
        }

        private async Task ExportPresentationAsPdfImpl()
        {
        }

        private async Task ExportPresentationAsMarkdownImpl()
        {
        }

        private async Task RemovePresentationImpl(PresentationUI block)
        {
            if (block is null)
            {
                return;
            }
            
            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Presentations.Remove(block);
            PresentationPart.Blocks.Remove(block.BaseSource);
            SavePresentationPart();
        }

        private void RefreshPresentationImp()
        {
            //
            // 更新
            Presentations.ForEach(x => x.Update(GetMetadataById, GetBlockById));
        }

        /// <summary>
        /// 自定义部件
        /// </summary>
        /// <remarks>自定义部件会出现在【设定】-【基础信息】当中，用户可以添加删除部件、调整部件顺序。</remarks>
        [NullCheck(UniTestLifetime.Constructor)]
        public PartOfPresentation PresentationPart { get; private set; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<PresentationUI> Presentations { get; init; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddPresentationCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand RefreshPresentationCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ExportPresentationAsPdfCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ExportPresentationAsPictureCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ExportPresentationAsMarkdownCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<PresentationUI> EditPresentationCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<PresentationUI> ShiftUpPresentationCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<PresentationUI> ShiftDownPresentationCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<PresentationUI> RemovePresentationCommand { get; }
    }
}
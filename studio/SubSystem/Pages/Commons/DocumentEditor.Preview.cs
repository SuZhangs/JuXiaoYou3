using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;
using Acorisoft.FutureGL.MigaStudio.Models.Presentations;
using Acorisoft.FutureGL.MigaStudio.Utilities;
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
            RefreshPresentation();
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
                var r1 = await SubSystem.Selection<ModuleBlock>(
                    SubSystemString.SelectTitle,
                    null,
                    Document.Parts
                            .Where(x => x is PartOfModule)
                            .OfType<PartOfModule>()
                            .Select(x => x.Blocks)
                            .SelectMany(x => x)
                            .Where(x => !string.IsNullOrEmpty(x.Metadata) && x is not GroupBlock)
                            .Where(x => x is NumberBlock or SingleLineBlock or RateBlock));

                var r = r1.Value;
                rarity.Name          = r.Name;
                rarity.IsMetadata    = !string.IsNullOrEmpty(r.Metadata);
                rarity.ValueSourceID = rarity.IsMetadata ? r.Metadata : r.Id;
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
            if (IsOverridePresentationPart)
            {
                SetDirtyState();
            }
            else
            {
                var db = Xaml.Get<IDatabaseManager>()
                             .Database
                             .CurrentValue;
                var mmp = db.Get<PresetProperty>();
                mmp.SetPresentationPreset(Type, PresentationPart);
                db.Set(mmp);
            }
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

        protected void ResetPresentation()
        {
            Presentations.AddRange(PresentationPart.Blocks.Select(PresentationUI.GetUI), true);
            RefreshPresentation();
        }
        
        private async Task OverridePresentationImpl()
        {
            if (!await DangerousOperation(SubSystemString.AreYouSureOverrideIt))
            {
                return;
            }
            //
            // 打开 PresentationPart
            var db = Xaml.Get<IDatabaseManager>()
                         .Database
                         .CurrentValue;

            var thisTypePresentation = db.Get<PresetProperty>()
                                         .GetPresentationPreset(Type, x => db.Set(x));
            
            IsOverridePresentationPart = true;
            
            //
            // 复制一份
            PresentationPart = new PartOfPresentation
            {
                Id     = ID.Get(),
                Blocks = new ObservableCollection<Presentation>(thisTypePresentation.Blocks)
            };
            
            //
            //
            SavePresentationPart();
            ResetPresentation();
        }
        
        private async Task SynchronizePresentationImpl()
        {
            if (!IsOverridePresentationPart)
            {
                await Obsoleted(SubSystemString.NoChange);
                return;
            }
            
            if (!await DangerousOperation(SubSystemString.AreYouSureSynchronizeIt))
            {
                return;
            }
            
            //
            // 打开 PresentationPart
            var db = Xaml.Get<IDatabaseManager>()
                         .Database
                         .CurrentValue;

            var thisTypePresentation = db.Get<PresetProperty>()
                                         .GetPresentationPreset(Type, x => db.Set(x));
            
            IsOverridePresentationPart = true;
            
            //
            // 复制一份
            PresentationPart.Blocks.AddMany(thisTypePresentation.Blocks, true);
            
            //
            //
            SavePresentationPart();
            ResetPresentation();
        }

        private async Task unOverridePresentationImpl()
        {
            if (!IsOverridePresentationPart)
            {
                await Obsoleted(SubSystemString.NoChange);
                return;
            }
            
            if (!await DangerousOperation(SubSystemString.AreYouSureUnOverrideIt))
            {
                return;
            }
            
            //
            // 打开 PresentationPart
            var db = Xaml.Get<IDatabaseManager>()
                         .Database
                         .CurrentValue;
            
            IsOverridePresentationPart = false;
            
            //
            // 复制一份
            PresentationPart = db.Get<PresetProperty>()
                                 .GetPresentationPreset(Type, x => db.Set(x));
            SavePresentationPart();
            ResetPresentation();
        }
        
        private async Task ExportPresentationAsPictureImpl()
        {
            var savedlg = FileIO.Save(SubSystemString.ImageFilter, "*.png");

            if (savedlg.ShowDialog() != true)
            {
                return;
            }
        }

        private async Task ExportPresentationAsPdfImpl()
        {
            var savedlg = FileIO.Save(SubSystemString.PdfFilter, "*.pdf");

            if (savedlg.ShowDialog() != true)
            {
                return;
            }
        }

        private async Task ExportPresentationAsMarkdownImpl()
        {
            var savedlg = FileIO.Save(SubSystemString.MarkdownFilter, "*.md");

            if (savedlg.ShowDialog() != true)
            {
                return;
            }
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

        protected void RefreshPresentation()
        {
            //
            // 更新
            Presentations.ForEach(x => x.Update(GetMetadataById, GetBlockById));
        }

        public bool IsOverridePresentationPart { get; private set; }
        
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
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand OverridePresentationCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand unOverridePresentationCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SynchronizePresentationCommand { get; }
    }
}
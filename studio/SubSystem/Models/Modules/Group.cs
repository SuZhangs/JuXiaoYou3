using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaStudio.Models.Modules.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    public class GroupBlockDataUI : ModuleBlockDataUI
    {
        public GroupBlockDataUI(GroupBlock block) : this(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        
        public GroupBlockDataUI(GroupBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            TargetBlock = block;
            Items       = new List<ModuleBlockDataUI>();
            if (block.Items is not null)
            {
                Items.AddRange(block.Items.Select(ModuleBlockFactory.GetDataUI));
            }
        }

        public override bool CompareTemplate(ModuleBlock block)
        {
            return TargetBlock.CompareTemplate(block);
        }

        public override bool CompareValue(ModuleBlock block)
        {
            return TargetBlock.CompareValue(block);
        }

        /// <summary>
        /// 
        /// </summary>
        protected GroupBlock TargetBlock { get; }

        public List<ModuleBlockDataUI> Items { get; init; }
    }

    public class GroupBlockEditUI : ModuleBlockEditUI, IGroupBlockEditUI
    {
        public GroupBlockEditUI(IGroupBlock block) : base(block)
        {
            Items         = new ObservableCollection<ModuleBlockEditUI>();
            AddCommand    = new AsyncRelayCommand(AddImpl);
            RemoveCommand = new AsyncRelayCommand<ModuleBlockEditUI>(RemoveImpl);
            EditCommand   = new AsyncRelayCommand<ModuleBlockEditUI>(EditImpl);
            UpCommand     = new RelayCommand<ModuleBlockEditUI>(UpImpl);
            DownCommand   = new RelayCommand<ModuleBlockEditUI>(DownImpl);

            if (block.Items is not null)
            {
                Items.AddRange(block.Items.Select(ModuleBlockFactory.GetEditUI));
            }
        }

        public override ModuleBlock CreateInstance()
        {
            return new GroupBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
                Items    = new List<ModuleBlock>(Items.Select(x => x.CreateInstance()))
            };
        }


        private async Task AddImpl()
        {
            var r = await NewElementViewModel.New();

            if (!r.IsFinished)
            {
                return;
            }


            var element = r.Value;

            await EditBlockViewModel.Edit(element);

            Items.Add(element);
        }

        private async Task EditImpl(ModuleBlockEditUI element)
        {
            if (element is null)
            {
                return;
            }

            var r = await EditBlockViewModel.Edit(element);

            if (!r.IsFinished)
            {
                return;
            }

            await Xaml.Get<IDialogService>()
                      .Notify(
                          CriticalLevel.Success,
                          SubSystemString.Notify,
                          SubSystemString.OperationOfSaveIsSuccessful);
        }

        private async Task RemoveImpl(ModuleBlockEditUI element)
        {
            if (element is null)
            {
                return;
            }

            var r = await Xaml.Get<IDialogService>()
                              .Danger(
                                  SubSystemString.Notify,
                                  SubSystemString.AreYouSureCreateNew);

            if (!r)
            {
                return;
            }

            if (Items.Remove(element))
            {
                await Xaml.Get<IDialogService>()
                          .Notify(
                              CriticalLevel.Success,
                              SubSystemString.Notify,
                              SubSystemString.OperationOfRemoveIsSuccessful);
            }
        }

        private void UpImpl(ModuleBlockEditUI element)
        {
            if (element is null)
            {
                return;
            }

            var index = Items.IndexOf(element);

            if (index < 1)
            {
                return;
            }

            Items.Move(index, index - 1);
        }

        private void DownImpl(ModuleBlockEditUI element)
        {
            if (element is null)
            {
                return;
            }

            var index = Items.IndexOf(element);

            if (index >= Items.Count - 1)
            {
                return;
            }

            Items.Move(index, index + 1);
        }

        public AsyncRelayCommand AddCommand { get; }
        public AsyncRelayCommand<ModuleBlockEditUI> EditCommand { get; }
        public AsyncRelayCommand<ModuleBlockEditUI> RemoveCommand { get; }
        public RelayCommand<ModuleBlockEditUI> UpCommand { get; }
        public RelayCommand<ModuleBlockEditUI> DownCommand { get; }

        public ObservableCollection<ModuleBlockEditUI> Items { get; init; }
    }
}
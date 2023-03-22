using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaStudio.Modules.ViewModels;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    public class GroupBlockDataUI : ModuleBlockDataUI
    {

        public GroupBlockDataUI(GroupBlock block) : base(block)
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
            Items       = new ObservableCollection<ModuleBlock>();
            if (block.Items is not null)
            {
                Items.AddRange(block.Items);
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
                Items    = new List<ModuleBlock>(Items)
            };
        }

        public ObservableCollection<ModuleBlock> Items { get; init; }
    }
}
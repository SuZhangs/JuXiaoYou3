using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    public class GroupBlockDataUI : ModuleBlockDataUI, IGroupBlockDataUI
    {

        public GroupBlockDataUI(GroupBlock block) : base(block)
        {
            TargetBlock = block;
            Items       = new List<ModuleBlock>();
            if (block.Items is not null)
            {
                Items.AddRange(block.Items);
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

        public List<ModuleBlock> Items { get; init; }
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
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    public class SwitchBlockDataUI : ModuleBlockDataUI<OptionBlock, bool>, IOptionBlockDataUI
    {
        public SwitchBlockDataUI(OptionBlock block) : base(block)
        {
        }
    }
    
    public class StarBlockDataUI : ModuleBlockDataUI<StarBlock, bool>, IOptionBlockDataUI
    {
        public StarBlockDataUI(StarBlock block) : base(block)
        {
        }
    }
    
    public class HeartBlockDataUI : ModuleBlockDataUI<HeartBlock, bool>, IOptionBlockDataUI
    {
        public HeartBlockDataUI(HeartBlock block) : base(block)
        {
        }
        
        public class SwitchBlockDataUI : ModuleBlockDataUI<SwitchBlock, bool>, IOptionBlockDataUI
        {
            public SwitchBlockDataUI(SwitchBlock block) : base(block)
            {
            }
        }
    }

    public class HeartBlockEditUI : ModuleBlockEditUI<HeartBlock, bool>, IOptionBlockEditUI
    {
        protected HeartBlockEditUI(HeartBlock block) : base(block)
        {
        }

        protected override HeartBlock CreateInstance()
        {
            return new HeartBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
                Fallback = Fallback
            };
        }
    }
    
    public class SwitchBlockEditUI : ModuleBlockEditUI<SwitchBlock, bool>, IOptionBlockEditUI
    {
        protected SwitchBlockEditUI(SwitchBlock block) : base(block)
        {
        }

        protected override SwitchBlock CreateInstance()
        {
            return new SwitchBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
                Fallback = Fallback
            };
        }
    }
    
    public class StarBlockEditUI : ModuleBlockEditUI<StarBlock, bool>, IOptionBlockEditUI
    {
        protected StarBlockEditUI(StarBlock block) : base(block)
        {
        }

        protected override StarBlock CreateInstance()
        {
            return new StarBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
                Fallback = Fallback
            };
        }
    }
}
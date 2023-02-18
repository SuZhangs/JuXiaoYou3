using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public class ColorBlockDataUI : ModuleBlockDataUI<ColorBlock, string>, IColorBlockDataUI
    {
        public ColorBlockDataUI(ColorBlock block) : base(block)
        {
        }
    }
    
    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public class ColorBlockEditUI : ModuleBlockEditUI<ColorBlock, string>, IColorBlockEditUI
    {
        public ColorBlockEditUI(ColorBlock block) : base(block)
        {
        }

        protected override ColorBlock CreateInstanceOverride()
        {
            return new ColorBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
                Fallback = Fallback,
            };
        }
    }
}
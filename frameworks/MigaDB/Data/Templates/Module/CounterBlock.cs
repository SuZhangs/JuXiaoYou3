namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface ICounterBlock : IModuleBlock, IModuleBlock<bool>
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface ICounterBlockDataUI : ICounterBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface ICounterBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<bool>
    {
    }
    
    public abstract class CounterBlock : ModuleBlock
    {
        
    }
}
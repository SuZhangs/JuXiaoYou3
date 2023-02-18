namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface IChartBlock : IModuleBlock, IModuleBlock<bool>
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface IChartBlockDataUI : IChartBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface IChartBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<bool>
    {
    }
    public class ChartBlock
    {
        
    }
}
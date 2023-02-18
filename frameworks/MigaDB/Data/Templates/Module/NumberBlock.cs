namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    /// <summary>
    /// 表示数字内容块。
    /// </summary>
    public interface INumberBlock : ICounterBlock
    {
        /// <summary>
        /// 后缀
        /// </summary>
        string Suffix { get; }
    }

    /// <summary>
    /// 表示数字内容块。
    /// </summary>
    public interface INumberBlockDataUI : INumberBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示数字内容块。
    /// </summary>
    public interface INumberBlockEditUI : ICounterBlockEditUI
    {
        /// <summary>
        /// 后缀
        /// </summary>
        string Suffix { get; set; }
    }

    /// <summary>
    /// 表示数字内容块。
    /// </summary>
    public class NumberBlock : ModuleBlock, INumberBlock
    {
        /// <summary>
        /// 清除当前值。
        /// </summary>
        public override void ClearValue() => Value = -1;
        
        /// <summary>
        /// 当前值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public int Fallback { get; init; }

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; init; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum { get; init; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; init; }
    }
}
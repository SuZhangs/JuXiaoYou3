namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    /// <summary>
    /// 表示多行内容块。
    /// </summary>
    public interface IMultiLineBlock : IModuleBlock, IModuleBlock<string>
    {
        /// <summary>
        /// 开启表达式
        /// </summary>
        bool EnableExpression { get; }
        
        /// <summary>
        /// 字数限制，如果值为-1，则表示没有限制。
        /// </summary>
        int CharacterLimited { get; }
    }

    /// <summary>
    /// 表示多行内容块。
    /// </summary>
    public interface IMultiLineBlockDataUI : IMultiLineBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示多行内容块。
    /// </summary>
    public interface IMultiLineBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<string>
    {
        /// <summary>
        /// 开启表达式
        /// </summary>
        bool EnableExpression { get; set; }

        /// <summary>
        /// 字数限制，如果值为-1，则表示没有限制。
        /// </summary>
        int CharacterLimited { get; set; }
    }


    /// <summary>
    /// 表示多行内容块。
    /// </summary>
    public class MultiLineBlock : ModuleBlock, IMultiLineBlock
    {
        
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (MultiLineBlock)block;
            return Fallback == bb.Fallback &&
                   EnableExpression == bb.EnableExpression &&
                   CharacterLimited == bb.CharacterLimited;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (MultiLineBlock)block;
            return bb.Value == Value;
        }
        /// <summary>
        /// 清除当前值。
        /// </summary>
        public override void ClearValue() => Value = string.Empty;
        
        /// <summary>
        /// 开启表达式
        /// </summary>
        public bool EnableExpression { get; init; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// 默认值
        /// </summary>
        public string Fallback { get; init; }
        
        
        /// <summary>
        /// 字数限制，如果值为-1，则表示没有限制。
        /// </summary>
        public int CharacterLimited { get; init; }
    }
}
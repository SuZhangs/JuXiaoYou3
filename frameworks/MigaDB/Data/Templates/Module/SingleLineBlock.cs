﻿namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示单行内容块。
    /// </summary>
    public interface ISingleLineBlock : IModuleBlock, IModuleBlock<string>
    {
        /// <summary>
        /// 后缀
        /// </summary>
        string Suffix { get; }
    }

    /// <summary>
    /// 表示单行内容块。
    /// </summary>
    public interface ISingleLineBlockDataUI : ISingleLineBlock, IModuleBlockDataUI
    {
        
    }

    /// <summary>
    /// 表示单行内容块。
    /// </summary>
    public interface ISingleLineBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<string>
    {
        /// <summary>
        /// 后缀
        /// </summary>
        string Suffix { get; set; }
    }
    
    /// <summary>
    /// 表示单行内容块。
    /// </summary>
    public class SingleLineBlock : ModuleBlock, ISingleLineBlock
    {
        
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (SingleLineBlock)block;
            return Fallback == bb.Fallback &&
                   Suffix == bb.Suffix;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (SingleLineBlock)block;
            return bb.Value == Value;
        }
        /// <summary>
        /// 清除当前值。
        /// </summary>
        public override void ClearValue() => Value = string.Empty;
        
        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; init; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// 默认值
        /// </summary>
        public string Fallback { get; init; }
    }
}
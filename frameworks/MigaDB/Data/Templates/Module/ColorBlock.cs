﻿namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public interface IColorBlock : IModuleBlock
    {
        
    }

    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public interface IColorBlockDataUI : IColorBlock, IModuleBlockDataUI
    {
        
    }

    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public interface IColorBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<string>
    {
        
    }

    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public class ColorBlock : ModuleBlock, IColorBlock, IModuleBlock<string>
    {
        /// <summary>
        /// 清除当前值。
        /// </summary>
        public override void ClearValue() => Value = string.Empty;
        
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
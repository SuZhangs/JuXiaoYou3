namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示滑块内容块。
    /// </summary>
    public class SliderBlock : ModuleBlock, INumberBlock
    {
        
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (SliderBlock)block;
            return Fallback == bb.Fallback &&
                   Suffix == bb.Suffix &&
                   Maximum == bb.Maximum && 
                   Minimum == bb.Minimum;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (SliderBlock)block;
            return bb.Value == Value;
        }
        
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
        public int Minimum { get;init; }
        
        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get;init; }
    }
}
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    public class MultiLineBlockDataUI : ModuleBlockDataUI<MultiLineBlock, string>, IMultiLineBlockDataUI
    {
        public MultiLineBlockDataUI(MultiLineBlock block) : base(block)
        {
            EnableExpression = block.EnableExpression;
            CharacterLimited = block.CharacterLimited;
        }

        /// <summary>
        /// 开启表达式
        /// </summary>
        public bool EnableExpression { get; }

        /// <summary>
        /// 字数限制，如果值为-1，则表示没有限制。
        /// </summary>
        public int CharacterLimited { get; }
    }

    public class MultiLineBlockEditUI : ModuleBlockEditUI<MultiLineBlock, string>, IMultiLineBlockEditUI
    {
        private int  _characterLimited;
        private bool _enableExpression;

        public MultiLineBlockEditUI(MultiLineBlock block) : base(block)
        {
            EnableExpression = block.EnableExpression;
            CharacterLimited = block.CharacterLimited;
        }

        protected override MultiLineBlock CreateInstanceOverride()
        {
            return new MultiLineBlock
            {
                Id               = Id,
                Name             = Name,
                EnableExpression = EnableExpression,
                CharacterLimited = CharacterLimited,
                Metadata         = Metadata,
                Fallback         = Fallback,
                ToolTips         = ToolTips,
            };
        }
        
        /// <summary>
        /// 开启表达式
        /// </summary>
        public bool EnableExpression
        {
            get => _enableExpression;
            set => SetValue(ref _enableExpression, value);
        }

        /// <summary>
        /// 字数限制，如果值为-1，则表示没有限制。
        /// </summary>
        public int CharacterLimited
        {
            get => _characterLimited;
            set => SetValue(ref _characterLimited, value);
        }
    }
}
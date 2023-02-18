using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlockDataUI : ObservableObject, IModuleBlockDataUI
    {
        protected ModuleBlockDataUI(ModuleBlock block)
        {
            Id       = block.Id;
            Metadata = block.Metadata;
            Name     = block.Name;
            ToolTips = block.ToolTips;
        }

        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 喵喵咒语
        /// </summary>
        public string Metadata { get; }

        /// <summary>
        /// 提示，可以是Markdown
        /// </summary>
        public string ToolTips { get; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; }
    }

    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlockDataUI<TBlock, TValue> : ModuleBlockDataUI where TBlock : ModuleBlock, IModuleBlock<TValue>
    {
        private TValue _value;

        protected ModuleBlockDataUI(TBlock block) : base(block)
        {
            TargetBlock = block;
            Fallback    = block.Fallback;
            Value       = block.Value;
        }

        protected abstract TValue OnValueChanged(TValue oldValue, TValue newValue);
        
        protected TBlock TargetBlock { get; }

        /// <summary>
        /// 默认值
        /// </summary>
        public TValue Fallback { get; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public TValue Value
        {
            get => _value;
            set
            {
                var old = _value;
                
                if (SetValue(ref _value, value))
                {
                    OnValueChanged(old, value);
                }
            }
        }
    }

    // TODO:
    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ChartBlockDataUI<TBlock, TValue> : ModuleBlockDataUI
        where TBlock : ModuleBlock, IModuleBlock<TValue>
    {
        protected ChartBlockDataUI(TBlock block) : base(block)
        {
        }

        public abstract ModuleBlock CreateInstance();
    }

    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlockEditUI : ObservableObject, IModuleBlockEditUI
    {
        private string _name;
        private string _toolTips;
        private string _metadata;
        
        protected ModuleBlockEditUI(IModuleBlock block)
        {
            Name     = block.Name;
            ToolTips = block.ToolTips;
            Metadata = block.Metadata;
        }
        public abstract ModuleBlock CreateInstance();

        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; } = ID.Get();

        /// <summary>
        /// 喵喵咒语
        /// </summary>
        public string Metadata
        {
            get => _metadata;
            set => SetValue(ref _metadata, value);
        }

        /// <summary>
        /// 提示，可以是Markdown
        /// </summary>
        public string ToolTips
        {
            get => _toolTips;
            set => SetValue(ref _toolTips, value);
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }


    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlockEditUI<TBlock, TValue> : ModuleBlockEditUI, IModuleBlockEditUI<TValue> 
        where TBlock : ModuleBlock, IModuleBlock, IModuleBlock<TValue>
    {
        private TValue _fallback;

        protected ModuleBlockEditUI(TBlock block) : base(block)
        {
            Fallback    = block.Fallback;
            TargetBlock = block;
        }

        public sealed override ModuleBlock CreateInstance()
        {
            return CreateInstanceOverride();
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <returns></returns>
        protected abstract TBlock CreateInstanceOverride();
        
        /// <summary>
        /// 目标内容块
        /// </summary>
        protected TBlock TargetBlock { get; }
        
        /// <summary>
        /// 默认值
        /// </summary>
        public TValue Fallback
        {
            get => _fallback;
            set => SetValue(ref _fallback, value);
        }
    }
}
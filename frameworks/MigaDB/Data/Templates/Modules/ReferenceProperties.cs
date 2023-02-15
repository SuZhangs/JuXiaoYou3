using Acorisoft.FutureGL.MigaDB.Contracts;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public abstract class TargetProperty : ModuleProperty, ITargetSink
    {
        /// <summary>
        /// 显示的源
        /// </summary>
        /// <remarks>用来存放ID，或者数据位置</remarks>
        public string DisplaySource { get; set; }
        
        /// <summary>
        /// 显示的名字
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        public sealed override void ClearValue()
        {
            DisplaySource = null;
            DisplayName   = null;
            Thumbnail     = null;
            base.ClearValue();
        }

        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            return $"{Name}:\x20{DisplayName}";
        }

        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        // TODO:
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }
    
    public class ReferenceProperty : TargetProperty
    {
        
    }

    
    public class MusicProperty : TargetProperty
    {
        
    }
    
    public class ImageProperty : TargetProperty
    {
        
    }
    
    public class AudioProperty : TargetProperty
    {
        
    }
    
    public class VideoProperty : TargetProperty
    {
        
    }
    
    public class FileProperty : TargetProperty
    {
        
    }
}
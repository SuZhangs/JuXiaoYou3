namespace Acorisoft.FutureGL.MigaDB.Contracts
{
    public interface ITargetSink
    {
        /// <summary>
        /// 显示的源
        /// </summary>
        /// <remarks>用来存放ID，或者数据位置</remarks>
        string DisplaySource { get; set; }
        
        /// <summary>
        /// 显示的名字
        /// </summary>
        string DisplayName { get; set; }
        
        /// <summary>
        /// 缩略图
        /// </summary>
        string Thumbnail { get; set; }
    }
    
    public interface IBindableTargetSink : ITargetSink
    {
    }
}
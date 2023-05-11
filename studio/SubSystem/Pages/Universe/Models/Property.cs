namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class BrowsableProperty : BrowsableElement
    {
        /// <summary>
        /// 设置属性
        /// </summary>
        public BrowsablePropertyDataSource Source { get; init; }
    }

    public enum BrowsablePropertyDataSource
    {
        NPC,
        
    }
}
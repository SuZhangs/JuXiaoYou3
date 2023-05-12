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
        Country,
        Gangbang,
        Team,
        Planets,
        Creatures,
        Material,
        Ore,
        NPC,
        Gods,
        Devils,
        Technology,
        Elemental,
        Poison,
        Calamity,
        Magic,
        Physic,
    }
}
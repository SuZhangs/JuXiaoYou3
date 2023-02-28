namespace Acorisoft.FutureGL.MigaDB.Data
{
    [Obsolete("这个功能仍在讨论，不会在生产环境中使用")]
    public enum OriginCreatorAnnouncement
    {
        Doujinshi        = 0x1, // 允许同人
        OOC              = 0x2, // 允许OOC
        Kichiku          = 0x4, // 允许鬼畜
        OfficialMatching = 0x8, // 是否有官配
        Erotic           = 0xf, // 允许涩图
    }
}
namespace Acorisoft.FutureGL.MigaDB.Data
{
    /// <summary>
    /// <see cref="RecomposeAcceptability"/> 类型表示二创的接受程度
    /// </summary>
    [Flags]
    [Obsolete("这个功能仍在讨论，不会在生产环境中使用")]
    public enum RecomposeAcceptability : int
    {
        Forbid         = 0x0, //禁止二创
        AllowDoujinshi = 0x1, // 同人
        AllowOOC       = 0x10, //允许OOC
        AllowKichiku   = 0x100, //允许鬼畜
        NoLimit        = 0x111, // 无限制
    }
}
namespace Acorisoft.FutureGL.MigaDB.Data
{
    /// <summary>
    /// <see cref="AffectionAnnouncement"/> 类型表示情感状态的声明
    /// </summary>
    [Flags]
    [Obsolete("这个功能仍在讨论，不会在生产环境中使用")]
    public enum AffectionAnnouncement : int
    {
        /*
         * 有无官配有无cp，不影响情感组合之间的绑定
         * 例如，这个故事虽然没有CP，但是有CB
         */
        StrongBinding, // 有官配，强绑定不能拆CP
        WeakBinding, // 有官配，但是可以解CP但不能是瞎绑定
    }
}
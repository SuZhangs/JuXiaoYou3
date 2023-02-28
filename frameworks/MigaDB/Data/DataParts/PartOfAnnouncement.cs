using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    [Obsolete("这个功能仍在讨论，不会在生产环境中使用")]
    public class PartOfAnnouncement : DataPart
    {
        private bool   _allowDoujinshi;
        private bool   _allowOOC;
        private bool   _allowKichiku;
        private string _statement;

        /// <summary>
        /// 获取或设置 <see cref="Statement"/> 属性。
        /// </summary>
        public string Statement
        {
            get => _statement;
            set => SetValue(ref _statement, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="AllowKichiku"/> 属性。
        /// </summary>
        public bool AllowKichiku
        {
            get => _allowKichiku;
            set => SetValue(ref _allowKichiku, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="AllowOOC"/> 属性。
        /// </summary>
        public bool AllowOOC
        {
            get => _allowOOC;
            set => SetValue(ref _allowOOC, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="AllowDoujinshi"/> 属性。
        /// </summary>
        public bool AllowDoujinshi
        {
            get => _allowDoujinshi;
            set => SetValue(ref _allowDoujinshi, value);
        }
    }
}
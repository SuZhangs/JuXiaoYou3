using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Models;

namespace Acorisoft.FutureGL.MigaDB.Documents
{
    /// <summary>
    /// 文档缓存
    /// </summary>
    public class DocumentCache : StorageObject
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 介绍
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 关键字
        /// </summary>
        public ObservableCollection<string> Keywords { get; init; }

        /// <summary>
        /// 类型
        /// </summary>
        public DocumentType Type { get; init; }
    }
}
using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Documents
{
    /// <summary>
    /// 文档缓存
    /// </summary>
    public class DocumentCache : StorageObject, IDataCache
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
        /// 是否可删除
        /// </summary>
        public bool Removable { get; init; }
        
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime TimeOfCreated { get; init; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime TimeOfModified { get; set; }
        
        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }

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
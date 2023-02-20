using Acorisoft.FutureGL.MigaDB.Models;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    public class MetadataCache : StorageObject
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 引用数量
        /// </summary>
        public int RefCount { get; set; }
    }
}
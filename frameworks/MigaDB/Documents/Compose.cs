
namespace Acorisoft.FutureGL.MigaDB.Documents
{
    /// <summary>
    /// 表示一个创作
    /// </summary>
    public class Compose : StorageObject, IData
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 介绍
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 类型
        /// </summary>
        public DocumentType Type { get; init; }
        
        /// <summary>
        /// 部件
        /// </summary>
        public DataPartCollection Parts { get; init; }
        
        /// <summary>
        /// 元数据
        /// </summary>
        public MetadataCollection Metas { get; init; }
    }
}
using Acorisoft.FutureGL.MigaDB.Documents;
using QuikGraph;

namespace Acorisoft.FutureGL.MigaDB.Data.Relationships
{
    /// <summary>
    /// 人物关系
    /// </summary>
    public class CharacterRelationship : StorageUIObject, IEdge<DocumentCache>
    {
        private string _nameToSource;
        private string _nameToTarget;

        /// <summary>
        /// 获取或设置 <see cref="NameToTarget"/> 属性。
        /// </summary>
        public string NameToTarget
        {
            get => _nameToTarget;
            set => SetValue(ref _nameToTarget, value);
        }
        
        /// <summary>
        /// 关系名
        /// </summary>
        public string NameToSource
        {
            get => _nameToSource;
            set => SetValue(ref _nameToSource, value);
        }
        
        /// <summary>
        /// 是否为双向关系
        /// </summary>
        public bool IsBidirection { get; init; }
        
        /// <summary>
        /// 程度，1-10
        /// </summary>
        public int Degree { get; init; }
        
        /// <summary>
        /// 关系类型
        /// </summary>
        public Relationship Relationship { get; init; }
        
        /// <summary>
        /// 源
        /// </summary>
        [BsonRef(Constants.Name_Cache_Document)]
        public DocumentCache Source { get; init; }
        
        /// <summary>
        /// 目标
        /// </summary>
        [BsonRef(Constants.Name_Cache_Document)]
        public DocumentCache Target { get; init; }
    }
}
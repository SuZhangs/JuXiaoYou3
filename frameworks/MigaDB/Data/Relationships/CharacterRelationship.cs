using Acorisoft.FutureGL.MigaDB.Documents;
using QuikGraph;

namespace Acorisoft.FutureGL.MigaDB.Data.Relationships
{
    /// <summary>
    /// 人物关系
    /// </summary>
    public class CharacterRelationship : StorageUIObject, IEdge<DocumentCache>
    {
        private string       _nameToSource;
        private string       _nameToTarget;
        private bool         _isBidirection;
        private int          _degree;
        private Relationship _relationship;
        
        /// <summary>
        /// 类型
        /// </summary>
        public DocumentType Type { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Relationship"/> 属性。
        /// </summary>
        public Relationship Relationship
        {
            get => _relationship;
            set => SetValue(ref _relationship, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Degree"/> 属性。
        /// </summary>
        public int Degree
        {
            get => _degree;
            set => SetValue(ref _degree, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsBidirection"/> 属性。
        /// </summary>
        public bool IsBidirection
        {
            get => _isBidirection;
            set => SetValue(ref _isBidirection, value);
        }

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
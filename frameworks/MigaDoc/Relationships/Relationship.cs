using QuikGraph;
// ReSharper disable ConstantConditionalAccessQualifier
// ReSharper disable NotNullMemberIsNotInitialized

namespace Acorisoft.Miga.Doc.Relationships
{
    public class Relationship : PropertyChanger, IEdge<DocumentIndex>, IDocumentNameService
    {
        private string _name;
        
        [BsonId]
        public string Id { get; init; }
        
        [BsonRef(Constants.cn_index)]
        public DocumentIndex Source { get; set; }
        
        [BsonRef(Constants.cn_index)]
        public DocumentIndex Target { get; set; }
        
        public bool IsBidirection { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }

    public class RelCopy : PropertyChanger
    {
        private string _relationName;
        
        public string Id { get; init; }
        public string Target { get; init; }
        public string Avatar { get; init; }
        public string Name { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="RelationName"/> 属性。
        /// </summary>
        public string RelationName
        {
            get => _relationName;
            set => SetValue(ref _relationName, value);
        }

    }

    public sealed class RelationshipGraph : BidirectionalGraph<DocumentIndex, Relationship>
    {
        
    }
}
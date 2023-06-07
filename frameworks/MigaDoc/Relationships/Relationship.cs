// ReSharper disable ConstantConditionalAccessQualifier
// ReSharper disable NotNullMemberIsNotInitialized

namespace Acorisoft.Miga.Doc.Relationships
{
    public class Relationship : ObservableObject
    {
        private string _name;
        
        public string Id { get; init; }
        
        public DocumentIndex Source { get; set; }
        
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

    public class RelCopy : ObservableObject
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
}
namespace Acorisoft.Miga.Doc.Parts
{
    public class ModuleIndex : PropertyChanger
    {
        private string _name;
        private string _fileName;
        private string _author;
        private string _organization;

        /// <summary>
        /// 获取或设置 <see cref="Id"/> 属性。
        /// </summary>
        [BsonId]
        public Guid Id { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public int Version { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public DocumentKind Type { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Organization"/> 属性。
        /// </summary>
        public string Organization
        {
            get => _organization;
            set => SetValue(ref _organization, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Author"/> 属性。
        /// </summary>
        public string Author
        {
            get => _author;
            set => SetValue(ref _author, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="FileName"/> 属性。
        /// </summary>
        public string FileName
        {
            get => _fileName;
            set => SetValue(ref _fileName, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
}
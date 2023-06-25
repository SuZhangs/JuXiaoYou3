namespace Acorisoft.Miga.Doc.Parts
{
    public class ModuleIndex
    {
        
        private string _fileName;
        private string _author;
        private string _organization;

        /// <summary>
        /// 获取或设置 <see cref="Id"/> 属性。
        /// </summary>
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
        public string Organization{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Author"/> 属性。
        /// </summary>
        public string Author{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="FileName"/> 属性。
        /// </summary>
        public string FileName{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
       { get; set; }
    }
}
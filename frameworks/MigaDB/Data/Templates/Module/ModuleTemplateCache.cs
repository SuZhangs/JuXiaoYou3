using Acorisoft.FutureGL.MigaDB.Models;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    public class ModuleTemplateCache : StorageObject
    {
        /// <summary>
        /// 获取或设置 <see cref="ForType"/> 属性。
        /// </summary>
        public int ForType { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="MetadataList"/> 属性。
        /// </summary>
        public string MetadataList { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Version"/> 属性。
        /// </summary>
        public int Version { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="AuthorList"/> 属性。
        /// </summary>
        public string AuthorList { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name { get; set; }
    }
}
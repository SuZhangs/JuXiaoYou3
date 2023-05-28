// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Documents
{
    /// <summary>
    /// <see cref="Document"/> 类型表示一个文档。
    /// </summary>
    public class Document : StorageObject, IDocumentNameService
    {
        #region ICloneable Interface

        /// <summary>
        /// 创建新的实例
        /// </summary>
        /// <returns>返回一个新的实例</returns>
        protected override StorageObject CreateInstanceOverride()
        {
            return new Document();
        }

        /// <summary>
        /// 复制当前的数据
        /// </summary>
        /// <param name="target">要复制到的目标实例。</param>
        protected sealed override void ShadowCopy(StorageObject target)
        {
            base.ShadowCopy(target);
        }

        #endregion

        /// <summary>
        /// 获取或设置当前的文档类型
        /// </summary>
        public DocumentKind Type { get; init; }

        /// <summary>
        /// 获取或设置当前的部件
        /// </summary>
        public DataPartCollection Parts { get; set; }

        /// <summary>
        /// 获取或设置当前的元数据
        /// </summary>
        public MetadataCollection Metas { get; set; }

        public static Document Create()
        {
            return new Document
            {
                Id     = ShortGuidString.GetId(),
                Name   = DocSR.Field_Untitle,
                Parts = new DataPartCollection(),
                Metas = new MetadataCollection()
            };
        }

        public static Document Create(DocumentKind kind)
        {
            return new Document
            {
                Id = ShortGuidString.GetId(),
                Name = DocSR.Field_Untitle,
                Type = kind,
                Parts = new DataPartCollection(),
                Metas = new MetadataCollection()
            };
        }
        
        public static Document Create(string id, DocumentKind kind)
        {
            return new Document
            {
                Id = id,
                Name = DocSR.Field_Untitle,
                Type = kind,
                Parts = new DataPartCollection(),
                Metas = new MetadataCollection()
            };
        }
    }
}
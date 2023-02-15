using Acorisoft.FutureGL.MigaDB.Contracts;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils;
using LiteDB;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// <see cref="ModuleProperty"/> 类型表示一个模组属性。
    /// </summary>
    public abstract class ModuleProperty : ObservableObject, IMetadataConvertible, IConvertibleDataSource
    {
        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public abstract string ToPlainText();

        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public abstract string ToMarkdown();

        public virtual void ClearValue()
        {
            Value = null;
        }

        /// <summary>
        /// Id
        /// </summary>
        [BsonId]
        public string Id { get; init; }

        /// <summary>
        /// 当前值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Fallback { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 元数据
        /// </summary>
        public string Metadata { get; set; }
        
        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 创建模组属性。
        /// </summary>
        /// <param name="kind">属性类型。</param>
        /// <returns>返回一个新的模组属性实例。</returns>
        public static ModuleProperty Create(MetadataKind kind)
        {
            return kind switch
            {
                /*
                 * Basic
                 */
                MetadataKind.Color => new ColorProperty { Id   = ID.Get() },
                MetadataKind.Degree => new DegreeProperty { Id = ID.Get() },
                MetadataKind.Number => new NumberProperty { Id = ID.Get() },
                MetadataKind.Slider => new SliderProperty { Id = ID.Get() },
                MetadataKind.Text => new TextProperty { Id     = ID.Get() },
                MetadataKind.Page => new PageProperty { Id     = ID.Get() },


                /*
                 * Option
                 */
                MetadataKind.Switch => new SwitchProperty { Id     = ID.Get() },
                MetadataKind.Radio => new RadioProperty { Id       = ID.Get() },
                MetadataKind.Talent => new TalentProperty { Id     = ID.Get() },
                MetadataKind.Favorite => new FavoriteProperty { Id = ID.Get() },
                MetadataKind.Binary => new BinaryProperty { Id     = ID.Get() },
                MetadataKind.Sequence => new SequenceProperty { Id = ID.Get() },

                /*
                * Reference
                */
                MetadataKind.Reference => new ReferenceProperty { Id = ID.Get() },
                MetadataKind.Image => new ImageProperty { Id         = ID.Get() },
                MetadataKind.Video => new VideoProperty { Id         = ID.Get() },
                MetadataKind.Music => new MusicProperty { Id         = ID.Get() },
                MetadataKind.Audio => new AudioProperty { Id         = ID.Get() },
                MetadataKind.File => new FileProperty { Id           = ID.Get() },


                /*
                 * Chart
                 */
                MetadataKind.Histogram => new HistogramProperty { Id = ID.Get() },
                MetadataKind.Radar => new RadarProperty { Id         = ID.Get() },

                /*
                 * Group
                 */
                MetadataKind.Group => new GroupProperty { Id           = ID.Get() },
                MetadataKind.Rate => new RateProperty { Id             = ID.Get() },
                MetadataKind.Likability => new LikabilityProperty { Id = ID.Get() },

                _ => new TextProperty { Id = ID.Get() }
            };
        }
    }
}
using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Parts
{
    public class Module : IAddChild
    {
        private string       _name;
        private string       _id;
        private string       _author;
        private string       _organization;
        private string       _contract;
        private int          _version;
        private DocumentKind _type;

        public void Accept(object instance)
        {
            Items.Add(instance as InputProperty);
        }

        public ObservableCollection<InputProperty> Items { get; } = new ObservableCollection<InputProperty>();

        /// <summary>
        /// 获取或设置唯一标识符
        /// </summary>
        public string Id{ get; set; }

        /// <summary>
        /// 获取或设置作者
        /// </summary>
        public string Author{ get; set; }

        /// <summary>
        /// 获取或设置组织
        /// </summary>
        [Alias("org")]
        public string Organization{ get; set; }

        /// <summary>
        /// 获取或设置联系方式
        /// </summary>
        public string Contract{ get; set; }

        /// <summary>
        /// 获取或设置模组名称
        /// </summary>
        public string Name
       { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Version"/> 属性。
        /// </summary>
        [Alias("ver")]
        public int Version{ get; set; }

        [Alias("type")]
        public DocumentKind Type{ get; set; }
        public CustomDataPart Active()
        {
            var part = new CustomDataPart
            {
                Id         = Guid.Parse(_id),
                Name       = _name,
                Properties = new List<InputProperty>(32),
                Version    = _version
            };

            //
            // 复制
            part.Properties.AddRange(Items.Select(x => x.Clone() as InputProperty));

            return part;
        }
    }
}
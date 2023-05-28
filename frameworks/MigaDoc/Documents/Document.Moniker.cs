using System.Collections.ObjectModel;
using Acorisoft.Miga.Doc.Groups;

namespace Acorisoft.Miga.Doc.Documents
{
    public class Moniker : PropertyChanger
    {

        private string _name;

        private DocumentKind _type;
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public DocumentKind Type
        {
            get => _type;
            set => SetValue(ref _type, value);
        }
        
        public DocumentIndex Create()
        {
            return new DocumentIndex
            {
                Id               = Id,
                Name             = _name,
                DocumentType     = _type,
                CreatedDateTime  = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                Keywords         = new ObservableCollection<string>(),
                EntityType       = EntityType.Document,
                GroupType        = GroupType.None
            };
        }
    }
}
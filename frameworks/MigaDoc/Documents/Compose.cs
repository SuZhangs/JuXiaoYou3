using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Documents
{
    public class Compose : PropertyChanger
    {
        public string Id { get; init; }
        public ObservableCollection<ComposeVersion> Drafts { get; init; }
        public ObservableCollection<ComposeVersion> RecycleBin { get; init; }
        private ComposeVersion _current;
        private string         _name;

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Current"/> 属性。
        /// </summary>
        public ComposeVersion Current
        {
            get => _current;
            set => SetValue(ref _current, value);
        }
    }

    public class ComposeVersion : PropertyChanger
    {
        private string   _name;
        private DateTime _modifiedDateTime;
        private string   _hash;

        /// <summary>
        /// 获取或设置 <see cref="Hash"/> 属性。
        /// </summary>
        public string Hash
        {
            get => _hash;
            set => SetValue(ref _hash, value);
        }
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="ModifiedDateTime"/> 属性。
        /// </summary>
        public DateTime ModifiedDateTime
        {
            get => _modifiedDateTime;
            set => SetValue(ref _modifiedDateTime, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        
        public DateTime CreatedDateTime { get; init; }
        public string Content { get; set; }
    }
}
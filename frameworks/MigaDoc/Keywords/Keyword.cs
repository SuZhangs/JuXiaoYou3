using System.Collections.ObjectModel;


namespace Acorisoft.Miga.Doc.Keywords
{
    public class Keyword : ObservableObject
    {
        private string _name;
        private string _summary;
        
        [BsonId]
        public string Id { get; init; }
        public string Owner { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary
        {
            get => _summary;
            set => SetValue(ref _summary, value);
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
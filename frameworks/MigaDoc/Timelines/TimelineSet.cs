namespace Acorisoft.Miga.Doc.Entities.Timelines
{
    public class TimelineSet : ObservableObject
    {
        private string _name;
        
        [BsonId]
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
        /// 
        /// </summary>
        public List<Timeline> Timelines { get; init; }
    }
}
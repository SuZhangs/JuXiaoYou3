using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Entities.Timelines
{
    public class Timeline : ObservableObject
    {
        private int    _index;
        private string _name;
        private string _content;
        private string _time;

        /// <summary>
        /// 获取或设置 <see cref="Time"/> 属性。
        /// </summary>
        public string Time
        {
            get => _time;
            set => SetValue(ref _time, value);
        }
        
        /// <summary>
        /// 
        /// </summary>
        [BsonId]
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Index"/> 属性。
        /// </summary>
        public int Index
        {
            get => _index;
            set => SetValue(ref _index, value);
        }
        
        /// <summary>
        /// 关联的角色
        /// </summary>
        public ObservableCollection<DocumentIndex> Characters { get; set; }
        
        /// <summary>
        /// 关联的地图
        /// </summary>
        public ObservableCollection<DocumentIndex> Maps { get; set; }
        
        /// <summary>
        /// 关联的势力
        /// </summary>
        public ObservableCollection<DocumentIndex> Forces { get; set; }

        public override string ToString()
        {
            return $"{Index}-{_name}";
        }
    }
}
using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Channels
{
    public class Channel : ObservableObject
    {
        private string _name;
        private string _summary;
        
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; init; }

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
        
        /// <summary>
        /// 房主
        /// </summary>
        public DocumentIndex Owner { get; init; }
        
        /// <summary>
        /// 所有成员
        /// </summary>
        public ObservableCollection<DocumentIndex> Members { get; init; }
        
        /// <summary>
        /// 所有消息
        /// </summary>
        public ObservableCollection<ChannelMessage> Messages { get; init; }
    }
}
namespace Acorisoft.FutureGL.MigaDB.Data.Communications
{
    public class SpecialTitle : ObservableObject
    {
        public string Owner { get; init; }

        private string _title;
        private string _color;

        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public string Color
        {
            get => _color;
            set => SetValue(ref _color, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Title"/> 属性。
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }
        
    }
    public class BubbleChannel : ObservableObject
    {
        private string _name;
        private string _owner;
        
        /// <summary>
        /// 消息列表
        /// </summary>
        public List<Bubble> MessageList { get; init; }

        /// <summary>
        /// 成员列表
        /// </summary>
        public List<string> MemberList { get; init; }
        
        /// <summary>
        /// 管理员列表
        /// </summary>
        public List<string> ManagerList { get; init; }
        
        /// <summary>
        /// 头衔列表
        /// </summary>
        public List<SpecialTitle> TitleList { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public PreferChatStyle Style { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Owner"/> 属性。
        /// </summary>
        public string Owner
        {
            get => _owner;
            set => SetValue(ref _owner, value);
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
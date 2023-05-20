namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class Thread : StorageUIObject
    {
        private string _content;
        private List<string> _images;
        
        [BsonRef(Constants.Name_Chat_Character)]
        public Character Character { get; init; }

        /// <summary>
        /// 配图
        /// </summary>
        public List<string> Images
        {
            get => _images;
            set => SetValue(ref _images, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
    }
}
namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    /// <summary>
    /// <see cref="CommentSection"/> 表示评论区功能
    /// </summary>
    public class CommentSection : StorageUIObject
    {
        private string _content;
        
        /// <summary>
        /// <see cref="CommentSection"/> 的父级ID
        /// </summary>
        public string Parent { get; init; }
        
        [BsonRef(Constants.Name_Chat_Character)]
        public Character Character { get; init; }

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
namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfMarkdown : PartOfManifest
    {
        private string _content;
        private string _intro;
        
        
        public PartOfMarkdown()
        {
            Id = Constants.IdOfMarkdownPart;
        }


        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
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

namespace Acorisoft.FutureGL.MigaStudio.SubSystem.Models
{
    public class Music : ForestObject
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; init; }
        
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; init; }

        private string _authorName;
        private string _name;
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="AuthorName"/> 属性。
        /// </summary>
        public string AuthorName
        {
            get => _authorName;
            set => SetValue(ref _authorName, value);
        }
    }
}
namespace Acorisoft.FutureGL.MigaDB.Models
{
    public class DatabaseProperty : StorageObject
    {
        // TODO: add
        public static bool IsValid(DatabaseProperty property) => property is not null;
        
        /// <summary>
        /// 世界观名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 世界观外文名（拼音或者英文）
        /// </summary>
        public string ForeignName { get; set; }
        
        /// <summary>
        /// 作者名称
        /// </summary>
        public string Author { get; set; }
        
        /// <summary>
        /// 协作者名称
        /// </summary>
        public string Partner { get; set; }
        
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
    }
}
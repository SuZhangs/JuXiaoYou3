namespace Acorisoft.FutureGL.MigaDB.Interfaces
{
    public interface IDataCache
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 
        /// </summary>
        bool Removable { get; }
        
        /// <summary>
        /// 
        /// </summary>
        bool IsDeleted { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        string Avatar { get; set; }
        
        /// <summary>
        /// 版本
        /// </summary>
        int Version { get; set; }
                
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime TimeOfCreated { get; init; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime TimeOfModified { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        string Intro { get; set; }
    }
}
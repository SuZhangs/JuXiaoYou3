namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public enum DatabaseFailedReason
    {
        /// <summary>
        /// 被占用
        /// </summary>
        Occupied,
        
        
    }
    
    public class DatabaseResult : Result<DatabaseFailedReason>
    {
        
    }
}
namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public enum DatabaseFailedReason
    {
        /// <summary>
        /// 被占用
        /// </summary>
        Occupied,
        
        DirectoryNotExists,
        
        DatabaseNotExists,
        
        DataBroken,
        
        MissingFileName,
        
        Unexpected,
    }
    
    public class DatabaseResult
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; init; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public DatabaseFailedReason Reason { get; init; }

        /// <summary>
        /// 返回一个失败的操作结果
        /// </summary>
        /// <param name="reason">失败的原因</param>
        /// <returns>返回一个失败的操作结果</returns>
        public static DatabaseResult Failed(DatabaseFailedReason reason) => new DatabaseResult
        {
            IsFinished = false,
            Reason     = reason
        };

        /// <summary>
        /// 表示一个成功的结果。
        /// </summary>
        public static readonly DatabaseResult Successful = new DatabaseResult { IsFinished = true };
    }
}
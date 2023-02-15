using Acorisoft.FutureGL.MigaDB.Core;
using MediatR;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public abstract class DatabaseOperation : INotification
    {
        /// <summary>
        /// 获取当前的数据库会话。
        /// </summary>
        public DatabaseSession Session { get; init; }
        
        /// <summary>
        /// 获取当前的数据库同步工具。
        /// </summary>
        public IDatabaseSynchronizer Synchronizer { get; init; }
    }
    
    public class DatabaseOpenOperation : DatabaseOperation
    {
        
    }
    
    public class DatabaseCloseOperation : DatabaseOperation
    {
        public static readonly DatabaseCloseOperation Instance = new DatabaseCloseOperation();
    }
    
    
    public class DatabaseUpdateOperation : DatabaseOperation
    {
        
    }
}
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaDB.IO
{
    public enum IOReason
    {
        
        /// <summary>
        /// 数据为空
        /// </summary>
        DataNOE,
        
        /// <summary>
        /// 数据错误
        /// </summary>
        DataError,
        
        /// <summary>
        /// 载荷为空
        /// </summary>
        PayloadNOE,
        
        /// <summary>
        /// 载荷错误
        /// </summary>
        PayloadError,
        
        /// <summary>
        /// 来源数据的路径为空
        /// </summary>
        InputFileNameNOE,
        
        /// <summary>
        /// 输出数据的路径为空
        /// </summary>
        OutputFileNameNOE,
        
        /// <summary>
        /// 没有权限写入
        /// </summary>
        WriteUnauthorized,
        
        /// <summary>
        /// 没有权限读取
        /// </summary>
        ReadUnauthorized,
        
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown
    }
}
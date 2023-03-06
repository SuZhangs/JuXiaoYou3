namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public enum EngineFailedReason
    {
        /// <summary>
        /// 用户取消
        /// </summary>
        Cancelled,
        
        /// <summary>
        /// 没有改变
        /// </summary>
        NoChanged,
        
        /// <summary>
        /// 缺失参数
        /// </summary>
        MissingParameter,
        
        /// <summary>
        /// Id为空
        /// </summary>
        MissingId,
        
        /// <summary>
        /// 重复
        /// </summary>
        Duplicated,
        
        /// <summary>
        /// 数据一致性被破坏
        /// </summary>
        ConsistencyBroken,
        
        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterDataError,
        
        /// <summary>
        /// 参数为空
        /// </summary>
        ParameterEmptyOrNull,
        
        /// <summary>
        /// 参数为文件，无访问权限
        /// </summary>
        ParameterSourceUnauthorizedAccess,
        
        /// <summary>
        /// 参数为文件，文件不存在
        /// </summary>
        ParameterSourceNotExists,
        
        /// <summary>
        /// 参数为文件，文件占用
        /// </summary>
        ParameterSourceOccupied,
    }
    
    public class EngineResult
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; init; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public EngineFailedReason Reason { get; init; }

        /// <summary>
        /// 返回一个失败的操作结果
        /// </summary>
        /// <param name="reason">失败的原因</param>
        /// <returns>返回一个失败的操作结果</returns>
        public static EngineResult Failed(EngineFailedReason reason) => new EngineResult
        {
            IsFinished = false,
            Reason     = reason
        };

        /// <summary>
        /// 表示一个成功的结果。
        /// </summary>
        public static readonly EngineResult Successful = new EngineResult { IsFinished = true };
    }
}
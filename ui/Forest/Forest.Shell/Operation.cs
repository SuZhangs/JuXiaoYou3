﻿namespace Acorisoft.FutureGL.Forest
{
    public class Operation<TValue>
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; init; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public string Reason { get; init; }
        
        /// <summary>
        /// 成功时获取结果。
        /// </summary>
        public TValue Value { get; init; }
        
        
        /// <summary>
        /// 返回一个失败的操作结果
        /// </summary>
        /// <param name="reason">失败的原因</param>
        /// <returns>返回一个失败的操作结果</returns>
        public static Operation<TValue> Failed(string reason) => new Operation<TValue>
        {
            IsFinished = false,
            Reason     = reason
        };

        /// <summary>
        /// 返回一个成功的操作结果以及值。
        /// </summary>
        /// <param name="value">正确操作之后的值</param>
        /// <returns>返回一个成功的操作结果以及值。</returns>
        public static Operation<TValue> Success(TValue value) => new Operation<TValue>{ IsFinished = true, Value = value };
    }
}
namespace Acorisoft.FutureGL.MigaDB.Contracts
{
    public interface IClampSink
    {
        /// <summary>
        /// 最大值
        /// </summary>
        int Maximum { get; }
        
        /// <summary>
        /// 最小值
        /// </summary>
        int Minimum { get; }
    }

    public interface IBindableClampSink : IClampSink, IBindableSink<int>
    {
    }
}
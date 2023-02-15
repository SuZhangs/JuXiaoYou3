namespace Acorisoft.FutureGL.MigaDB.Contracts
{
    public interface ISuffixSink
    {
        /// <summary>
        /// 后缀
        /// </summary>
        string Suffix { get; }
    }

    public interface IBindableSuffixSink : ISuffixSink
    {
        
    }
}
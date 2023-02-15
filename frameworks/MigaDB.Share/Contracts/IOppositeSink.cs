namespace Acorisoft.FutureGL.MigaDB.Contracts
{
    public interface IOppositeSink
    {
        /// <summary>
        /// 负面值
        /// </summary>
        string Negative { get; set; }
        
        /// <summary>
        /// 正面值
        /// </summary>
        string Positive { get; set; }
    }

    public interface IBindableOppositeSink : IOppositeSink, IBindableSink<bool>
    {
    }
}
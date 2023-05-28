namespace Acorisoft.Miga.Doc.Networks
{
    public interface ITransactionResolver
    {
        void Resolve(Transaction transaction);
        void Process(Transaction transaction);
    }
}
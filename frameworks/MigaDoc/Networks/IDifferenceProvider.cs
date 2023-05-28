namespace Acorisoft.Miga.Doc.Networks
{ 
    public interface IDifferenceProvider : ITransactionResolver
    {
        List<DifferenceDescription> GetDescriptions();
    }
}
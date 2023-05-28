namespace Acorisoft.Miga.Doc.Networks
{
    public interface IBinaryDifferenceProvider : ITransactionResolver
    {
        List<BinaryDescription> GetDescriptions();
    }
}
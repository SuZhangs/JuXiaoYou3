namespace Acorisoft.Miga.Doc.Networks
{
    public interface IDirectlyDifferenceProvider : ITransactionResolver
    {
        void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context);
        void GetDescriptions(IList<DirectlyDescription> context);
    }
}
namespace Acorisoft.Miga.Doc.Networks
{
    public class TransactionManifest
    {
        public List<BinaryTransaction> Module { get; init; }
        public List<BinaryTransaction> Image { get; init; }
        public List<DifferenceTransaction> Difference { get; init; }
        public List<DirectlyTransaction> Directly { get; init; }
    }
}
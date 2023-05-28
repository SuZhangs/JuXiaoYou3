namespace Acorisoft.Miga.Doc.Networks
{
    public enum Difference
    {
        Replace,
        Added,
        Removed
    }

    public enum DifferencePosition
    {
        Compose,
        Draft,
        Recycle,
        Current
    }
    
    
    public class DifferenceDescription : Transaction
    {
        public string Id { get; set; }
        public string CurrentHash { get; init; }
        public List<Tuple<string,string>> Draft { get; init; }
        public List<Tuple<string,string>> RecycleBin { get; init; }
    }

    public class DifferenceTransaction : Transaction
    {
        public DifferencePosition Position { get; init; }
        public Difference Difference { get; init; }
        public string Id { get; set; }
        public string SubId { get; set; }
    }
}
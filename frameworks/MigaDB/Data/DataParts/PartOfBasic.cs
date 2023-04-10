namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfBasic : DataPart
    {
        public PartOfBasic()
        {
            Id = Constants.PartOfBasic;
        }
        public Dictionary<string, string> Buckets { get; init; }
    }
}
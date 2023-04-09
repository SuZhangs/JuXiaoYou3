namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfBasic : DataPart
    {
        public PartOfBasic()
        {
            Id = "__Basic";
        }
        public Dictionary<string, string> Buckets { get; init; }
    }
}
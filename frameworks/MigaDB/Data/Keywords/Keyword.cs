namespace Acorisoft.FutureGL.MigaDB.Data.Keywords
{
    public class Keyword : StorageObject
    {
        public string Value { get; init; }
        public int RefCount { get; set; }
    }
}
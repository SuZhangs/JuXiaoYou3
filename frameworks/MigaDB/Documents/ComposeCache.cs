
namespace Acorisoft.FutureGL.MigaDB.Documents
{
    public class ComposeCache : StorageObject, IDataCache
    {
        public string Name { get; }
        public bool Removable { get; }
        public bool IsDeleted { get; set; }
        public string Avatar { get; set; }
        public int Version { get; set; }
        public DateTime TimeOfCreated { get; init; }
        public DateTime TimeOfModified { get; set; }
        public string Intro { get; set; }
    }
}
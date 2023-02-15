using LiteDB;

namespace Acorisoft.FutureGL.MigaDB.Models
{
    public abstract class StorageObject
    {
        [BsonId]
        public string Id { get; init; }
    }
}
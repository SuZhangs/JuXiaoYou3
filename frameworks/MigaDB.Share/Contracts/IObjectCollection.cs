using LiteDB;

namespace Acorisoft.FutureGL.MigaDB.Contracts
{
    public interface IObjectCollection
    {
        ILiteCollection<BsonDocument> Props { get; }
    }
}
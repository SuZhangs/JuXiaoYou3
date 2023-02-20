using LiteDB;

namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public static class LiteDB
    {
        public static bool HasID<T>(this ILiteCollection<T> db, string id)
        {
            return db.Exists(Query.EQ("_id", id));
        }
    }
}
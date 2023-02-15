namespace Acorisoft.FutureGL.MigaUtils.Collections
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> handler)
        {
            if (collection is null)
            {
                return;
            }

            if (handler is null)
            {
                return;
            }

            foreach (var item in collection)
            {
                handler(item);
            }
        }
    }
}
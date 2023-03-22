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

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> target, bool clear = false)
        {
            if (collection is null)
            {
                return;
            }
            
            if (clear)
            {
                collection.Clear();
            }

            if (collection is List<T> l)
            {
                l.AddRange(target);
            }
            else
            {
                foreach (var item in target)
                {
                    collection.Add(item);
                }
            }
        }
    }
}
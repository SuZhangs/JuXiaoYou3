using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaUtils.Collections
{
    public static class EnumerableExtensions
    {
        private static void Empty<T>(T a, int b, int c){}

        public static int IndexOf<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (predicate is null)
            {
                return -1;
            }

            if (collection is null)
            {
                return -1;
            }

            var i = 0;
            
            foreach (var item in collection)
            {
                if (predicate(item))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        #region ForEach

        
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

        #endregion

        #region AddRange

        

        public static void AddRange<T>(this Queue<T> collection, IEnumerable<T> target, bool clear = false)
        {
            if (collection is null)
            {
                return;
            }
            
            if (clear)
            {
                collection.Clear();
            }

            
            foreach (var item in target)
            {
                collection.Enqueue(item);
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
            else if(target is not null)
            {
                foreach (var item in target)
                {
                    collection.Add(item);
                }
            }
        }
        
        #endregion

        #region ShiftUp

                
        public static void ShiftUp<T>(this ObservableCollection<T> source, T target)
        {
            ShiftUp(source, target, Empty); 
        }
        
        public static void ShiftUp<T>(this ObservableCollection<T> source, T target, Action callback)
        {
            ShiftUp(source, target, (_, _, _) => callback?.Invoke()); 
        }
        
        public static void ShiftUp<T>(this List<T> source, T target)
        {
            ShiftUp(source, target, Empty); 
        }
        
        public static void ShiftUp<T>(this List<T> source, T target, Action callback)
        {
            ShiftUp(source, target, (_, _, _) => callback?.Invoke()); 
        }

        public static void ShiftUp<T>(this List<T> source, T target, Action<T, int, int> callback)
        {
            if (ReferenceEquals(target, default(T)))
            {
                return;
            }

            if (source is null)
            {
                return;
            }

            var index = source.IndexOf(target);


            if (index < 1)
            {
                return;
            }
            
            source.RemoveAt(index);
            source.Insert(index - 1, target);
            callback?.Invoke(target, index, index - 1);
        }

        public static void ShiftUp<T>(this ObservableCollection<T> source, T target, Action<T, int, int> callback)
        {
            if (ReferenceEquals(target, default(T)))
            {
                return;
            }

            if (source is null)
            {
                return;
            }

            var index = source.IndexOf(target);

            if (index < 1)
            {
                return;
            }
            
            source.Move(index, index - 1);
            
            callback?.Invoke(target, index, index - 1);
        }
        
        #endregion

        #region ShiftDown

        public static void ShiftDown<T>(this ObservableCollection<T> source, T target)
        {
            ShiftDown(source, target, Empty); 
        }
        
        public static void ShiftDown<T>(this List<T> source, T target)
        {
            ShiftDown(source, target, Empty); 
        }
        
        public static void ShiftDown<T>(this List<T> source, T target, Action callback)
        {
            ShiftDown(source, target, (_, _, _) => callback?.Invoke());
        }
        
        public static void ShiftDown<T>(this ObservableCollection<T> source, T target, Action callback)
        {
            ShiftDown(source, target, (_, _, _) => callback?.Invoke());
        }

        public static void ShiftDown<T>(this List<T> source, T target, Action<T, int, int> callback)
        {
            if (ReferenceEquals(target, default(T)))
            {
                return;
            }

            if (source is null)
            {
                return;
            }

            var index = source.IndexOf(target);

            if (index >= source.Count - 1)
            {
                return;
            }
            
            source.RemoveAt(index);
            source.Insert(index + 1, target);
            callback?.Invoke(target, index, index + 1);
        }
        
        
        
        public static void ShiftDown<T>(this ObservableCollection<T> source, T target, Action<T, int, int> callback)
        {
            if (ReferenceEquals(target, default(T)))
            {
                return;
            }

            if (source is null)
            {
                return;
            }

            var index = source.IndexOf(target);

            if (index >= source.Count - 1)
            {
                return;
            }
            
            source.Move(index, index + 1);
            
            callback?.Invoke(target, index, index + 1);
        }
        
        #endregion

        public static List<T> Flat<TSource, T>(this IEnumerable<TSource> collection, Func<TSource, IEnumerable<T>> selector)
        {
            if (collection is null || selector is null)
            {
                return null;
            }

            var array = new List<T>(32);
            foreach (var item in collection)
            {
                var result = selector(item);
                array.AddRange(result);
            }

            return array;
        }
    }
}
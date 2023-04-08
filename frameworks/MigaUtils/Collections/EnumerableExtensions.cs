﻿using System.Collections.ObjectModel;

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
        
        public static void ShiftDown<T>(this ObservableCollection<T> source, T target)
        {
            ShiftDown(source, target, Empty); 
        }
        
        public static void ShiftUp<T>(this ObservableCollection<T> source, T target)
        {
            ShiftUp(source, target, Empty); 
        }
        
        public static void ShiftUp<T>(this ObservableCollection<T> source, T target, Action callback)
        {
            ShiftUp(source, target, (_, _, _) => callback?.Invoke()); 
        }
        
        public static void ShiftDown<T>(this ObservableCollection<T> source, T target, Action callback)
        {
            ShiftDown(source, target, (_, _, _) => callback?.Invoke());
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
    }
}
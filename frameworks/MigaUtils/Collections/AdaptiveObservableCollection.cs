using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaUtils.Collections
{
    internal static class Cache
    {
        internal static readonly PropertyChangedEventArgs         CountPropertyChanged   = new PropertyChangedEventArgs("Count");
        internal static readonly PropertyChangedEventArgs         IndexerPropertyChanged = new PropertyChangedEventArgs("Item[]");
        internal static readonly NotifyCollectionChangedEventArgs ResetCollectionChanged = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

    }
    
    public class ObservableCollectionAdapter<T> : CollectionAdapter<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        
        SimpleMonitor _monitor;

        [NonSerialized]
        private int _blockReentrancyCount;

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> class.</summary>
        public ObservableCollectionAdapter() : base(new List<T>())
        {
        }


        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> class that contains elements copied from the specified collection.</summary>
        /// <param name="list">The collection from which the elements are copied.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="list" /> parameter cannot be <see langword="null" />.</exception>
        public ObservableCollectionAdapter(IList<T> list) : base(list)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> class that contains elements copied from the specified list.</summary>
        /// <param name="list">The list from which the elements are copied.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="list" /> parameter cannot be <see langword="null" />.</exception>
        public ObservableCollectionAdapter(ObservableCollection<T> list) : base(list)
        {
        }


        /// <summary>Moves the item at the specified index to a new location in the collection.</summary>
        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        public void Move(int oldIndex, int newIndex) => MoveItem(oldIndex, newIndex);


        /// <summary>Occurs when a property value changes.</summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => PropertyChanged += value;
            remove => PropertyChanged -= value;
        }

        /// <summary>Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.</summary>
        [field: NonSerialized]
        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>Removes all items from the collection.</summary>
        protected override void ClearItems()
        {
            CheckReentrancy();
            base.ClearItems();
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionReset();
        }

        /// <summary>Removes the item at the specified index of the collection.</summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            CheckReentrancy();
            var obj = this[index];
            base.RemoveItem(index);
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, obj, index);
        }

        /// <summary>Inserts an item into the collection at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            CheckReentrancy();
            base.InsertItem(index, item);
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>Replaces the element at the specified index.</summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        protected override void SetItem(int index, T item)
        {
            CheckReentrancy();
            var oldItem = this[index];
            base.SetItem(index, item);
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, item, index);
        }

        /// <summary>Moves the item at the specified index to a new location in the collection.</summary>
        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            CheckReentrancy();
            var obj = this[oldIndex];
            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, obj);
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Move, obj, newIndex, oldIndex);
        }

        /// <summary>Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.PropertyChanged" /> event with the provided arguments.</summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, e);
        }

        /// <summary>Occurs when a property value changes.</summary>
        [field: NonSerialized]
        protected virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged" /> event with the provided arguments.</summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var collectionChanged = CollectionChanged;
            if (collectionChanged == null)
                return;
            ++_blockReentrancyCount;
            try
            {
                collectionChanged(this, e);
            }
            finally
            {
                --_blockReentrancyCount;
            }
        }

        /// <summary>Disallows reentrant attempts to change this collection.</summary>
        /// <returns>An <see cref="T:System.IDisposable" /> object that can be used to dispose of the object.</returns>
        protected IDisposable BlockReentrancy()
        {
            ++_blockReentrancyCount;
            return EnsureMonitorInitialized();
        }

        /// <summary>Checks for reentrant attempts to change this collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">If there was a call to <see cref="M:System.Collections.ObjectModel.ObservableCollection`1.BlockReentrancy" /> of which the <see cref="T:System.IDisposable" /> return value has not yet been disposed of. Typically, this means when there are additional attempts to change this collection during a <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged" /> event. However, it depends on when derived classes choose to call <see cref="M:System.Collections.ObjectModel.ObservableCollection`1.BlockReentrancy" />.</exception>
        protected void CheckReentrancy()
        {
            if (_blockReentrancyCount <= 0)
                return;
            var collectionChanged = CollectionChanged;
            if ((collectionChanged != null ? collectionChanged.GetInvocationList().Length > 1 ? 1 : 0 : 0) != 0)
                throw new InvalidOperationException();
        }

        private void OnCountPropertyChanged() => OnPropertyChanged(Cache.CountPropertyChanged);

        private void OnIndexerPropertyChanged() => OnPropertyChanged(Cache.IndexerPropertyChanged);


        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index) => OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));

        private void OnCollectionChanged(
            NotifyCollectionChangedAction action,
            object item,
            int index,
            int oldIndex)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        private void OnCollectionChanged(
            NotifyCollectionChangedAction action,
            object oldItem,
            object newItem,
            int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        private void OnCollectionReset() => OnCollectionChanged(Cache.ResetCollectionChanged);

        private SimpleMonitor EnsureMonitorInitialized() => _monitor ??= new SimpleMonitor(this);

        [Serializable]
        private sealed class SimpleMonitor : IDisposable
        {
            [NonSerialized]
            internal ObservableCollectionAdapter<T> _collection;

            public SimpleMonitor(ObservableCollectionAdapter<T> collection) => _collection = collection;

            public void Dispose() => --_collection._blockReentrancyCount;
        }
    }
}
using CommunityToolkit.Mvvm.Input;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NMF.Expressions.Linq;
using System;
using System.Collections.Specialized;

namespace AvaloniaTodoListApplication.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ICollection<ProjectModel> Projects { get; } = new ObservableCollection<ProjectModel>();

        public IEnumerable<TodoModel> TodosInOrder { get; }

        private static readonly DateTimeOffset EndOfTime = DateTimeOffset.MaxValue;

        public MainWindowViewModel()
        {
            TodosInOrder = 
                   (from p in Projects.WithUpdates()
                    where !p.IsOnHold
                    from t in p.Todos
                    where !t.IsDone
                    orderby t.Priority ascending, t.Deadline ?? EndOfTime
                    select t).RestoreIndices();
        }

        [RelayCommand]
        private void AddProject()
        {
            Projects.Add(new ProjectModel { Name = $"Project {Projects.Count + 1}" });
        }
    }

    public class BufferCollection<T> : INotifyCollectionChanged, IList, IReadOnlyCollection<T>, ICollection<T>
    {
        private readonly List<T> _items;
        private readonly IEnumerable<T> _elements;

        public BufferCollection(IEnumerable<T> elements)
        {
            ObservableExtensions.KeepOrder = true;
            _items = new List<T>(elements);
            if (elements is INotifyCollectionChanged collectionChanged)
            {
                collectionChanged.CollectionChanged += InnerCollectionChanged;
            }
            _elements = elements;
        }

        private void InnerCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                _items.Clear();
                _items.AddRange(_elements);
                CollectionChanged?.Invoke(this, e);
                return;
            }
            var oldItemsIndex = e.OldStartingIndex;
            var newItemsIndex = e.NewStartingIndex;
            if (e.OldItems != null)
            {
                oldItemsIndex = RemoveAll(e.OldItems, e.OldStartingIndex);
            }
            if (e.NewItems != null)
            {
                if (e.NewStartingIndex == -1)
                {
                    int idx = _items.Count;
                    foreach (var foundItem in FindItems(e.NewItems))
                    {
                        _items.Insert(foundItem.index, foundItem.item);
                        idx = Math.Min(idx, foundItem.index);
                    }
                    newItemsIndex = idx;
                }
                else
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        _items.Insert(e.NewStartingIndex + i, (T)e.NewItems[i]);
                    }
                }
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.NewItems, newItemsIndex));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.OldItems, oldItemsIndex)); 
                    break;
                case NotifyCollectionChangedAction.Replace:
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.NewItems, e.OldItems, oldItemsIndex));
                    break;
                case NotifyCollectionChangedAction.Move:
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, e.NewItems, newItemsIndex, oldItemsIndex));
                    break;
            }
        }

        private int RemoveAll(IList items, int stopIndex)
        {
            if (stopIndex < 0)
            {
                stopIndex = 0;
            }
            var idx = -1;
            for (int i = _items.Count - 1; i >= stopIndex; i--)
            {
                if (items.Contains(_items[i]))
                {
                    _items.RemoveAt(i);
                    idx = i;
                }
            }
            return idx;
        }

        private IEnumerable<(int index, T item)> FindItems(IList items)
        {
            var index = 0;
            var itemIndex = 0;
            if (items == null || items.Count == 0)
            {
                yield break;
            }
            var targetItem = (T)items[index];
            foreach (var item in _elements)
            {
                if (EqualityComparer<T>.Default.Equals(item, targetItem))
                {
                    yield return (index, item);
                    itemIndex++;
                    if (itemIndex == items.Count)
                    {
                        yield break;
                    }
                    targetItem = (T)items[itemIndex];
                }
                index++;
            }
        }

        public object? this[int index] { get => ((IList)_items)[index]; set => ((IList)_items)[index] = value; }

        public bool IsFixedSize => ((IList)_items).IsFixedSize;

        public bool IsReadOnly => ((IList)_items).IsReadOnly;

        public int Count => ((ICollection)_items).Count;

        public bool IsSynchronized => ((ICollection)_items).IsSynchronized;

        public object SyncRoot => ((ICollection)_items).SyncRoot;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        int IList.Add(object? value)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(object? value)
        {
            return ((IList)_items).Contains(value);
        }

        public bool Contains(T item)
        {
            return ((ICollection<T>)_items).Contains(item);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_items).CopyTo(array, index);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)_items).CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(object? value)
        {
            return ((IList)_items).IndexOf(value);
        }

        void IList.Insert(int index, object? value)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object? value)
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackList
{
    /// <summary>
    /// Smart collection for controlled notifications
    /// </summary>
    /// <typeparam name="T">Type of collection</typeparam>
    public class SmartCollection<T> : ObservableCollection<T>
    {
        public SmartCollection()
            : base()
        {
        }

        public SmartCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public SmartCollection(List<T> list)
            : base(list)
        {
        }
        /// <summary>
        /// Adds a range of items then notifies when all of them had been added
        /// </summary>
        /// <param name="range"></param>
        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Items.Add(item);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        /// <summary>
        /// Adds an item and notifies with custom changedaction
        /// </summary>
        /// <param name="item">Item to add</param>
        public void AddWithNotify(T item)
        {
            Items.Add(item);
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }
        /// <summary>
        /// Removes an item and notifies with custom changedaction sending item's index from the collection
        /// </summary>
        /// <param name="item">Item to remove</param>
        public void RemoveWithNotify(T item)
        {
            var index = Items.IndexOf(item);
            Items.Remove(item);
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }
        /// <summary>
        /// Resets list
        /// </summary>
        /// <param name="range">List of items</param>
        public void Reset(IEnumerable<T> range)
        {
            this.Items.Clear();

            AddRange(range);
        }
    }
}

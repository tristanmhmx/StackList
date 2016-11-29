using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StackList
{
    public class StackListControl : ContentView
    {
        #region Private Properties
        private static StackListControl element;
        private bool refreshAllowed = true;
        private bool RefreshAllowed
        {
            set
            {
                if (refreshAllowed == value)
                    return;

                refreshAllowed = value;
                OnPropertyChanged();
            }
            get { return refreshAllowed; }
        }
        ObservableCollection<object> observableSource;
        private ObservableCollection<object> ObservableSource
        {
            get { return observableSource; }
            set
            {
                if (observableSource != null)
                {
                    observableSource.CollectionChanged -= CollectionChanged;
                }
                observableSource = value;

                if (observableSource != null)
                {
                    observableSource.CollectionChanged += CollectionChanged;
                }
            }
        }
        #endregion

        #region BindableProperties

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(StackListControl), null, BindingMode.Default, null, OnItemsSourcePropertyChanged);

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemsSource), typeof(DataTemplate), typeof(StackListControl));

        public static BindableProperty IsRefreshingProperty = BindableProperty.Create(nameof(IsRefreshing), typeof(bool), typeof(StackListControl), false, BindingMode.TwoWay, null, Refreshed);

        public static readonly BindableProperty RefreshCommandProperty = BindableProperty.Create(nameof(RefreshCommand), typeof(ICommand), typeof(StackListControl), null, propertyChanged: OnRefreshCommandChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        public bool IsRefreshing
        {
            get { return (bool)GetValue(IsRefreshingProperty); }
            set { SetValue(IsRefreshingProperty, value); }
        }

        #endregion

        public StackListControl()
        {
            InitializeComponent();
        }

        #region ItemsSource
        /// <summary>
        /// Called when [items source property changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            element = (StackListControl)bindable;

            var notifyCollection = newValue as INotifyCollectionChanged;

            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += NotifyCollectionOnCollectionChanged;
            }

            element.BuildTransaction();
        }
        /// <summary>
        /// Called when [item changed]
        /// </summary>
        /// <param name="sender">The item</param>
        /// <param name="e">Changed args</param>
        private static void NotifyCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            int index = e.NewStartingIndex;
                            foreach (var item in e.NewItems)
                                element.ItemsStack.Children.Insert(index++, element.GetItemView(item));
                        }
                        break;
                    case NotifyCollectionChangedAction.Move:
                        {
                            var item = element.ObservableSource[e.OldStartingIndex];
                            element.ItemsStack.Children.RemoveAt(e.OldStartingIndex);
                            element.ItemsStack.Children.Insert(e.NewStartingIndex, element.GetItemView(item));
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        {
                            element.ItemsStack.Children.RemoveAt(e.OldStartingIndex);
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        {
                            element.ItemsStack.Children.RemoveAt(e.OldStartingIndex);
                            element.ItemsStack.Children.Insert(e.NewStartingIndex, element.GetItemView(element.ObservableSource[e.NewStartingIndex]));
                        }
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        element.ItemsStack.Children.Clear();
                        foreach (var item in element.ItemsSource)
                            element.ItemsStack.Children.Add(element.GetItemView(item));
                        break;
                }
            });
        }
        /// <summary>
        /// Build card list for the first time
        /// </summary>
        private void BuildTransaction()
        {
            ItemsStack.Children.Clear();

            if (ItemsSource == null)
            {
                ObservableSource = null;
                return;
            }

            foreach (var item in ItemsSource)
                ItemsStack.Children.Add(GetItemView(item));

            var isObs = ItemsSource.GetType().GetGenericTypeDefinition() == typeof(ObservableCollection<>);
            if (isObs)
            {
                ObservableSource = new ObservableCollection<object>(ItemsSource.Cast<object>());
            }
        }
        /// <summary>
        /// Gets view from datatemplate
        /// </summary>
        /// <param name="item">Card</param>
        /// <returns>View</returns>
        protected virtual View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();

            var view = content as View;
            if (view == null)
                return null;

            view.BindingContext = item;

            return view;
        }
        /// <summary>
        /// Adds a card when internal collection changed
        /// </summary>
        /// <param name="sender">Item</param>
        /// <param name="e">Collection Args</param>
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var index = e.NewStartingIndex;
                        foreach (var item in e.NewItems)
                            ItemsStack.Children.Insert(index++, GetItemView(item));
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    {
                        var item = ObservableSource[e.OldStartingIndex];
                        ItemsStack.Children.RemoveAt(e.OldStartingIndex);
                        ItemsStack.Children.Insert(e.NewStartingIndex, GetItemView(item));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        ItemsStack.Children.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        ItemsStack.Children.RemoveAt(e.OldStartingIndex);
                        ItemsStack.Children.Insert(e.NewStartingIndex, GetItemView(ObservableSource[e.NewStartingIndex]));
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ItemsStack.Children.Clear();
                    foreach (var item in ItemsSource)
                        ItemsStack.Children.Add(GetItemView(item));
                    break;
            }
        }
        #endregion

        #region Refresh
        /// <summary>
        /// Disables/Enables Refresh depending on scroll position
        /// </summary>
        /// <param name="sender">ScrollView</param>
        /// <param name="e">Event Args</param>
        private void OnScrolled(object sender, ScrolledEventArgs e)
        {
            var scrollView = sender as ScrollView;
            if (scrollView != null)
            {
                RefreshLayout.IsPullToRefreshEnabled = e.ScrollY == 0;
            }
        }
        /// <summary>
        /// Refresh Command Changed
        /// </summary>
        /// <param name="bindable">Control</param>
        /// <param name="oldValue">Old Command</param>
        /// <param name="newValue">New Command</param>
        private static void OnRefreshCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var lv = (StackListControl)bindable;
            var oldCommand = (ICommand)oldValue;
            var command = (ICommand)newValue;

            lv.OnRefreshCommandChanged(oldCommand, command);
        }
        /// <summary>
        /// Action to assign new command
        /// </summary>
        /// <param name="oldCommand">Old Command</param>
        /// <param name="newCommand">New Command</param>
        private void OnRefreshCommandChanged(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= OnCommandCanExecuteChanged;
            }

            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += OnCommandCanExecuteChanged;
                RefreshAllowed = newCommand.CanExecute(null);
            }
            else
            {
                RefreshAllowed = true;
            }
        }
        /// <summary>
        /// Sets if command can execute
        /// </summary>
        /// <param name="sender">Command</param>
        /// <param name="eventArgs">Event Args</param>
        private void OnCommandCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            RefreshAllowed = RefreshCommand.CanExecute(null);
        }
        /// <summary>
        /// Changes is refreshing control
        /// </summary>
        /// <param name="bindable">Control</param>
        /// <param name="oldValue">Bool Old Value</param>
        /// <param name="newValue">Bool New Value</param>
        private static void Refreshed(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
                ((StackListControl)bindable).RefreshLayout.IsRefreshing = (bool)newValue;
        }
        #endregion
    }
}

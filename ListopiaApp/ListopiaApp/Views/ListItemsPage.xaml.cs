using ListopiaApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListopiaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListItemsPage : ContentPage , INotifyPropertyChanged

	{
        private List _list;
        private ListItem _currentListItem;
        private ObservableCollection<ListItem> _items;
        private System.Timers.Timer pollingTimer;

		public ListItemsPage (List list)
		{
            _list = list;
            Title = _list.Title;

			InitializeComponent ();


            EditEntry.Focused += ToggleControls;
            EditEntry.Focused += (sender, e) => ItemsList.IsEnabled = false;
            EditEntry.Unfocused += ToggleControls;
            EditEntry.Unfocused += (sender, e) => ItemsList.IsEnabled = true;

            AddEntry.Focused += (sender, e) => ItemsList.IsEnabled = false;
            AddEntry.Unfocused += (sender, e) => ItemsList.IsEnabled = true;

            pollingTimer = new System.Timers.Timer { Interval = 5000 };
            pollingTimer.Elapsed += PollItems;
            
        }

        private async void PollItems(object sender, EventArgs e)
        {
            var items = await REST.Service.GetListItems(_list.Id);

            foreach(var item in items)
            {
                var existingItem = _items.SingleOrDefault(i => i.Id == item.Id);
                if(existingItem != null)
                {
                    if (!existingItem.Equals(item))
                    {
                        existingItem.Name = item.Name;
                        existingItem.IsChecked = item.IsChecked;
                        existingItem.CheckedByUserName = item.CheckedByUserName;
                    }
                }
                else
                {
                    _items.Add(item);
                }
            }

            for (var i = 0; i < _items.Count; i++)
            {
                var currentItem = items.SingleOrDefault(li => li.Id == _items[i].Id);
                if (currentItem == null)
                {
                    _items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            _items = await REST.Service.GetListItems(_list.Id);
             ItemsList.ItemsSource = _items;
            pollingTimer.Start();
        }

        protected override void OnDisappearing()
        {
            pollingTimer.Stop();
            
        }

        private async void Switch_Toggled(object sender, ItemTappedEventArgs e)
        {
            var listItem = e.Item as ListItem;

            if (listItem != null)
            {
                listItem.IsChecked = !listItem.IsChecked;
                if (listItem.IsChecked)
                    await REST.Service.CheckItem(listItem);
                else
                    await REST.Service.UncheckItem(listItem);
            }
        }

        private void ContextEdit_Clicked(object sender, EventArgs e)
        {
            ItemsList.IsEnabled = false;
            _currentListItem = (sender as MenuItem).BindingContext as ListItem;
            EditEntry.Text = _currentListItem.Name;
            EditEntry.Focus();
        }

        private async void ContextDelete_Clicked(object sender, EventArgs e)
        {
            var item = (sender as MenuItem).BindingContext as ListItem;

            if (item != null && await REST.Service.DeleteListItem(item))
                _items.Remove(item);
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            _items.Add(await REST.Service.AddListItem(new ListItem { Name = AddEntry.Text, CheckedByUserName = "", ListId = _list.Id}));
            AddEntry.Text = "";
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            _currentListItem.Name = EditEntry.Text;
            await REST.Service.EditListItem(_currentListItem);
            ItemsList.IsEnabled = true;
        }

        private void ToggleControls(object Sender, EventArgs e)
        {
            AddEntry.IsVisible = !AddEntry.IsVisible;
            AddButton.IsVisible = !AddButton.IsVisible;

            EditEntry.IsVisible = !EditEntry.IsVisible;
            SaveButton.IsVisible = !SaveButton.IsVisible;
        }

        private async void ItemsList_Refreshing(object sender, EventArgs e)
        {
            ItemsList.IsRefreshing = true;
            _items = await REST.Service.GetListItems(_list.Id);
            ItemsList.ItemsSource = _items;
            ItemsList.IsRefreshing = false;
        }
    }
}
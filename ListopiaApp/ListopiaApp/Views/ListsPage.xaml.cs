using ListopiaApp.Models;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListopiaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListsPage : ContentPage
	{
        public ObservableCollection<List> Lists = new ObservableCollection<List>();

		public ListsPage ()
		{
			InitializeComponent ();
            Title = (App.Current as App).AuthInfo.username;
        }

        protected async override void OnAppearing()
        {
            try
            {
                Lists = await REST.Service.GetLists();
                ListsList.ItemsSource = Lists;
                ListsList.SelectedItem = null;
                base.OnAppearing();
            }
            catch
            {
                var app = App.Current as App;
                app.AuthInfo = null;
                app.MainPage = new LoginPage();
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var list = e.SelectedItem as List;
            if (list != null)
                Navigation.PushAsync(new ListItemsPage(list));
        }

        private void LogoutItem_Clicked(object sender, EventArgs e)
        {
            (App.Current as App).AuthInfo = null;
            (App.Current as App).MainPage = new LoginPage();
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            Lists.Add(await REST.Service.AddList(new List { Title = AddEntry.Text, OwnerName = "" }));
        }

        private async void ListsList_Refreshing(object sender, EventArgs e)
        {
            ListsList.IsRefreshing = true;
            Lists = await REST.Service.GetLists();
            ListsList.ItemsSource = Lists;
            ListsList.IsRefreshing = false;
        }

        private async void ContextDelete_Clicked(object sender, EventArgs e)
        {
            var list = (sender as MenuItem).BindingContext as List;

            if (list != null && await REST.Service.DeleteList(list))
                Lists.Remove(list);
        }

        private async void ContextEdit_Clicked(object sender, EventArgs e)
        {

        }
    }
}
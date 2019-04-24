using ListopiaApp.Models;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListopiaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListSettingsPage : ContentPage
	{
        private List _list;
        private ObservableCollection<ShareInvite> _shareInvites;

		public ListSettingsPage (List list)
		{
			InitializeComponent();
            _list = list;
            TitleEntry.Text = _list.Title;
            
        }

        protected async override void OnAppearing()
        {
            _shareInvites = await REST.Service.GetListShareInvites(_list);
            InvitesList.ItemsSource = _shareInvites;
        }


        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            var invite = await REST.Service.AddShareInvite(new ShareInvite { ListId = _list.Id, InvitedUsername = AddEntry.Text });

            if (invite == null)
                await DisplayAlert("Error", "The username entered was not found, please verify", "OK");
            else
            {
                _shareInvites.Add(invite);
                AddEntry.Text = "";
            }
        }

        private async void SaveTitleButton_Clicked(object sender, EventArgs e)
        {
            _list.Title = TitleEntry.Text;
            if(!await REST.Service.EditList(_list))
                DisplayAlert("Something's gone wrong", "Oops", "OK");
        }

        private async void ContextDelete_Clicked(object sender, EventArgs e)
        {

        }
    }
}
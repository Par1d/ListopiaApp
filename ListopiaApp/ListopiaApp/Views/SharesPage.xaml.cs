using ListopiaApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListopiaApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SharesPage : ContentPage
	{
        private ObservableCollection<ShareInvite> _invites;

		public SharesPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            _invites = await REST.Service.GetShareInvites();
            ShareList.ItemsSource = _invites;
            base.OnAppearing();
        }
    }
}
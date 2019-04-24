using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListopiaApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var login = new REST.Models.Login { username = EmailEntry.Text, password = PasswordEntry.Text };

            if (await REST.Service.Login(login))
            {
                REST.Service.RefreshAuth();
                (App.Current as App).MainPage = new NavigationPage(new ListsPage())
                {
                    BarBackgroundColor = (Color)App.Current.Resources["CompanyColor"],
                    BarTextColor = Color.White
                };
            }
            else
                ErrorLabel.IsVisible = true;
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}
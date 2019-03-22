using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ListopiaApp
{
    public partial class App : Application
    {
        public REST.Models.AuthInfo AuthInfo
        {
            get
            {
                if (Properties.ContainsKey("AuthInfo"))
                    return JsonConvert.DeserializeObject<REST.Models.AuthInfo>(Properties["AuthInfo"].ToString());

                return null;
            }

            set
            {
                Properties["AuthInfo"] = JsonConvert.SerializeObject(value);
            }
        }

        public App()
        {

            InitializeComponent();
            REST.Service.RefreshAuth();
            if (AuthInfo != null)
                MainPage = new NavigationPage(new Views.ListsPage())
                {
                    BarBackgroundColor = (Color)Application.Current.Resources["CompanyColor"],
                    BarTextColor = Color.White
                };
            else
                MainPage = new Views.LoginPage();
        }

        protected override void OnStart()
        {
            REST.Service.RefreshAuth();
            if (AuthInfo != null)
                MainPage = new NavigationPage(new Views.ListsPage())
                {
                    BarBackgroundColor = (Color)Application.Current.Resources["CompanyColor"],
                    BarTextColor = Color.White
                };
            else
                MainPage = new Views.LoginPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

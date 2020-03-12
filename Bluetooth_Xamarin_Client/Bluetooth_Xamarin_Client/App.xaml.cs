using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bluetooth_Xamarin_Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var navigationPage = new NavigationPage(new MainPage()); // <== Added
            MainPage = navigationPage; //Added - Originally before : MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

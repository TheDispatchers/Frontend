using System;
using Xamarin.Forms;

namespace iTaxApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void OnCreateRide(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RidePage());
        }

        void OnHistory(object sender, EventArgs e)

        {

        }
        void OnSettings(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }

        void OnLogout(object sender, EventArgs e)
        {
        }

    }
}
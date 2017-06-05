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
            Navigation.PushAsync(new HistoryPage());
        }
        void OnSettings(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }

        async void OnLogout(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
            await Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(this);
        }
        void OnClear(object sender, EventArgs e)
        {
            //TO DO: Ask user if he actually wants to do this.
            SQLite.ClearHistory();
        }

    }
}
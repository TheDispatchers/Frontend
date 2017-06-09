using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iTaxApp.Droid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmPage : ContentPage
    {
        public ConfirmPage()
        {
            InitializeComponent();
            TempData data = SQLite.ReadTempData();
            //Set the labels.
            startLoc.Text = data.startLoc;
            destination.Text = data.destination;
            distance.Text = data.distance;
            time.Text = data.time;
            price.Text = data.price;
        }
        async void OnCancel(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
        async void OnConfirm(object sender, EventArgs e)
        {
            Ride ride;
            TempData data = SQLite.ReadTempData();
            string sessionKey = data.sessionKey;
            string fromLatitude = data.fromLat;
            string fromLongitude = data.fromLng;
            string toLatitude = data.toLat;
            string toLongitude = data.toLng;
            Console.WriteLine(System.DateTime.Today);
            ride = new Ride(fromLatitude, fromLongitude, toLatitude, toLongitude, sessionKey);
            ride.function = "orderRide";
            object obj = SynchronousSocketClient.StartClient("orderRide", ride);
            ride = (Ride)obj;
            Console.WriteLine("Response: " + ride.response);
            RideDetails details = JsonConvert.DeserializeObject<RideDetails>(ride.response);
            RideHistory history = new RideHistory() { driverID = details.driver, user = Convert.ToString(App.Current.Properties["user"]), price = data.price, rating = "N/A", rideDate = details.date };
            SQLite.ConnectDatabase();
            SQLite.InsertHistoryData(history);
            await this.DisplayAlert("Ride", "Your taxi is on it's way. ETA: " + details.time, "OK");
            await Navigation.PopToRootAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            startLoc.Text = Convert.ToString(App.Current.Properties["startLoc"]);
            destination.Text = Convert.ToString(App.Current.Properties["destination"]);
            distance.Text = Convert.ToString(App.Current.Properties["distance"]);
            time.Text = Convert.ToString(App.Current.Properties["time"]);
            price.Text = Convert.ToString(App.Current.Properties["price"]);
        }

        void OnCancel(object sender, EventArgs e)
        {
            
        }
        void OnConfirm(object sender, EventArgs e)
        {
            Ride ride;
            string sessionKey = Convert.ToString(App.Current.Properties["sessionKey"]);
            string fromLatitude = Convert.ToString(App.Current.Properties["fromLat"]);
            string fromLongitude = Convert.ToString(App.Current.Properties["fromLng"]);
            string toLatitude = Convert.ToString(App.Current.Properties["toLat"]);
            string toLongitude = Convert.ToString(App.Current.Properties["toLng"]);
            ride = new Ride(fromLatitude, fromLongitude, toLatitude, toLongitude, sessionKey);
            ride.function = "orderRide";
            object obj = SynchronousSocketClient.StartClient("orderRide", ride);
            ride = (Ride)obj;
            Console.WriteLine("Response: " + ride.response);
        }
    }
}
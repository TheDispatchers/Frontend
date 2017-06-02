using iTaxApp.Droid;
using Plugin.Geolocator;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace iTaxApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RidePage : ContentPage
    {
        /* VARIABLES DECLARATION */
        Geocoder geoCoder;
        Pin pin;
        string fromLatitude;
        string fromLongitude;
        string toLatitude;
        string toLongitude;

        /* CONSTRUCTOR */
        public RidePage()
        {
            InitializeComponent();
            geoCoder = new Geocoder();
        }

        /// <summary>
        /// Method that runs when the "Find my location" button is pressed. 
        /// It finds the current position of the device and uses other method to translate it to an address.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnFind(object sender, EventArgs e)
        {
            MyMap.Pins.Clear(); //Clear all previous pins that were on the map.

            var locator = CrossGeolocator.Current; // Create the geo locator
            var pos = await locator.GetPositionAsync(timeoutMilliseconds: 10000); // Get the current position from the device's GPS HW
            var position = new Position(pos.Latitude, pos.Longitude); // Instantiate the Position with the data obtained from the device

            reverseGeocodedOutputLabel.Text = "Searching..";  // Verification for user, that the device is searching for location.

            latitude.Text = "Latitude: " + pos.Latitude; // Update label.
            longitude.Text = "Longitude: " + pos.Longitude; // Update label.

            fromLatitude = pos.Latitude.ToString(); // Store the value for later access.
            fromLongitude = pos.Longitude.ToString(); // Store the value for later access.

            pin = new Pin /* Create new Pin on the map - Start location */
            {
                Type = PinType.Place,
                Position = position,
                Label = "Start location",
                Address = "Your taxi will pick you up here."
            };
            MyMap.Pins.Add(pin); // Add the pin to the map.

            DecodeAddress(position); // Decode the GPS Coordinates into an address *** More info in method description ***

            if (destination.Text != null) //Prevent trying to decode address to GPS coordinates if the address field is empty.
            {
                DecodeCoords(); // Decode the address to GPS Coordinates *** More info in method description ***
            }
        }

        /// <summary>
        /// This method uses the GeoCoder plugin to get the most probable address for the GPS position provided to it. 
        /// The GeoCoder works asynchronously so the method can't actually return a value, therefore it updates the relevant label instead.
        /// As this function by default returns multiple possible addresses, counter is attached to it to limit the output to only one address.
        /// </summary>
        /// <param name="position"></param>
        public async void DecodeAddress(Position position)
        {
            string[] myAddress = new string[3];
            {
                var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
                int counter = 0;
                reverseGeocodedOutputLabel.Text = "";
                foreach (var address in possibleAddresses)
                {
                    if (counter < 1)
                    {
                        reverseGeocodedOutputLabel.Text += address;
                        location.Text = address;
                        counter++;
                    }
                }
            }
        }
        /// <summary>
        /// This method uses the GeoCoder plugin to get the GPS position for the address provided. 
        /// Then it places a pin on the map with the destination and some information for the user.
        /// It interacts with the UI directly.
        /// </summary>
        public async void DecodeCoords()
        {
            var addressToCode = destination.Text;
            var approximateLocations = await geoCoder.GetPositionsForAddressAsync(addressToCode);
            foreach (var destinationpos in approximateLocations)
            {
                geocodedOutputLabel.Text = destinationpos.Latitude + ", " + destinationpos.Longitude + "\n";
                toLatitude = destinationpos.Latitude.ToString();
                toLongitude = destinationpos.Longitude.ToString();
                pin = new Pin
                {
                    Type = PinType.Place,
                    Position = destinationpos,
                    Label = "Destination",
                    Address = "Your taxi will drop you off here."
                };
                MyMap.Pins.Add(pin);
            }
        }

        /// <summary>
        /// This method opens the ExtrasPage for user to choose extras from.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnExtras(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ExtrasPage());
        }

        /// <summary>
        /// This method gathers all the necessary data to make an order of a taxi and directs the user to the confirm page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnOrder(object sender, EventArgs e)
        {
            Ride ride;
            string sessionKey = Convert.ToString(App.Current.Properties["sessionKey"]);
            ride = new Ride(fromLatitude, fromLongitude, toLatitude, toLongitude, sessionKey);
            object obj = SynchronousSocketClient.StartClient("oderRide", ride);
            Navigation.PushAsync(new ConfirmPage());
        }
    }
}

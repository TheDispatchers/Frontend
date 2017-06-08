using iTaxApp.Droid;
using Plugin.Geolocator;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace iTaxApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RidePage : ContentPage
    {
        /* VARIABLES DECLARATION */
        Geocoder geoCoder;
        Pin pin;
        Pin destinationPin;
        private string fromLatitude;
        private string fromLongitude;
        private string toLatitude;
        private string toLongitude;
        private Position position;
        private Position destinationPosition;
        private Plugin.Geolocator.Abstractions.Position pos;

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
        private async void OnFind(object sender, EventArgs e)
        {
            MyMap.Pins.Clear(); //Clear all previous pins that were on the map.

            var locator = CrossGeolocator.Current; // Create the geo locator
            pos = await locator.GetPositionAsync(timeoutMilliseconds: 10000); // Get the current fused position from the device's GPS HW
            position = new Position(pos.Latitude, pos.Longitude); // Instantiate the Position with the data obtained from the device

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
            try
            {
                double km = 10.0; //CalcApproxKm(position, destinationPosition);
                MapSpan span = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(km));
                MyMap.MoveToRegion(span); // Move the map to the selected region.
            }
            catch (Exception)
            {
                MapSpan span = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(5));
                MyMap.MoveToRegion(span); // Move the map to the selected region.
                throw;
            }
        }

        /// <summary>
        /// This method uses the GeoCoder plugin to get the most probable address for the GPS position provided to it. 
        /// The GeoCoder works asynchronously so the method can't actually return a value, therefore it updates the relevant label instead.
        /// As this function by default returns multiple possible addresses, counter is attached to it to limit the output to only one address.
        /// </summary>
        /// <param name="position"></param>
        private async void DecodeAddress(Position position)
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
        private async void DecodeCoords()
        {
            MyMap.Pins.Remove(destinationPin);
            var addressToCode = destination.Text;
            var approximateLocations = await geoCoder.GetPositionsForAddressAsync(addressToCode);
            foreach (var destinationpos in approximateLocations)
            {
                geocodedOutputLabel.Text = destinationpos.Latitude + ", " + destinationpos.Longitude + "\n";
                toLatitude = destinationpos.Latitude.ToString();
                toLongitude = destinationpos.Longitude.ToString();
                destinationPosition = destinationpos;
                destinationPin = new Pin
                {
                    Type = PinType.Place,
                    Position = destinationpos,
                    Label = "Destination",
                    Address = "Your taxi will drop you off here."
                };
                MyMap.Pins.Add(destinationPin);
            }
        }

        private void OnUnfocus()
        {
            if (destination.Text != null) //Prevent trying to decode address to GPS coordinates if the address field is empty.
            {
                DecodeCoords(); // Decode the address to GPS Coordinates *** More info in method description ***
                try
                {
                    MapSpan span = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(5));
                    MyMap.MoveToRegion(span); // Move the map to the selected region.
                }
                catch
                {

                }
            }
        }
        /// <summary>
        /// This method opens the ExtrasPage for user to choose extras from.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExtras(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ExtrasPage());
        }

        /// <summary>
        /// This method gathers all the necessary data to make an order of a taxi and directs the user to the confirm page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrder(object sender, EventArgs e)
        {
            Ride ride;
            string sessionKey = Convert.ToString(App.Current.Properties["sessionKey"]);
            ride = new Ride(fromLatitude, fromLongitude, toLatitude, toLongitude, sessionKey);
            ride.function = "getDistanceTimePrice";
            object obj = SynchronousSocketClient.StartClient("getDistanceTimePrice", ride);
            ride = (Ride)obj;
            Console.WriteLine("Response: " + ride.response);
            RideDetails details = JsonConvert.DeserializeObject<RideDetails>(ride.response);
            TempData data = new TempData()
            {
                sessionKey = sessionKey,
                distance = details.distance,
                time = details.time,
                price = details.price,
                startLoc = details.start,
                destination = details.destination,
                fromLat = fromLatitude,
                fromLng = fromLongitude,
                toLat = toLatitude,
                toLng = toLongitude
            };
            SQLite.CreateDatabase();
            SQLite.InsertTempData(data);

            Navigation.PushAsync(new ConfirmPage());
        }

        private static double CalcDist(double lat1, double lat2, double lon1, double lon2)
        {
            const double r = 6371; // meters

            var sdlat = Math.Sin((lat2 - lat1) / 2);
            var sdlon = Math.Sin((lon2 - lon1) / 2);
            var q = sdlat * sdlat + Math.Cos(lat1) * Math.Cos(lat2) * sdlon * sdlon;
            var d = 2 * r * Math.Asin(Math.Sqrt(q));

            return d;
        }

        
    }
}

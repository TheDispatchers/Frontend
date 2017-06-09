namespace iTaxApp
{
    public class Ride
    {
        public string function { get; set; }
        public string fromlat { get; set; }
        public string fromlng { get; set; }
        public string tolat { get; set; }
        public string tolng { get; set; }
        public string sessionKey { get; set; }
        public string response { get; set; }
        public Ride(string fromLatitude, string fromLongitude, string toLatitude, string toLongitude, string sessionkey)
        {
            fromlat = fromLatitude;
            fromlng = fromLongitude;
            tolat = toLatitude;
            tolng = toLongitude;
            sessionKey = sessionkey;
        }
    }
}

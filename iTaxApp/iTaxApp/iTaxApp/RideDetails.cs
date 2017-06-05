using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace iTaxApp
{
    public class RideDetails
    {

        // Do this for each property you want to map.
        [JsonProperty(PropertyName = "destination")]
        public string destination { get; set; }

        [JsonProperty(PropertyName = "start")]
        public string start { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public string distance { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string time { get; set; }

        [JsonProperty(PropertyName = "price")]
        public string price { get; set; }

        [JsonProperty(PropertyName = "driverID")]
        public string driver { get; set; }

        [JsonProperty(PropertyName = "currentDate")]
        public string date { get; set; }
    }
}

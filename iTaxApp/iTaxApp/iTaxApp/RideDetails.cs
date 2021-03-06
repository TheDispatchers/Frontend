﻿using Newtonsoft.Json;

namespace iTaxApp
{
    public class RideDetails
    {
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

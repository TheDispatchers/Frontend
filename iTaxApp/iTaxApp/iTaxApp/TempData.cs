using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace iTaxApp
{
    [Table("TempData")]
    class TempData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string sessionKey { get; set; }
        public string distance { get; set; }
        public string time { get; set; }
        public string price { get; set; }
        public string startLoc { get; set; }
        public string destination { get; set; }
        public string fromLat { get; set; }
        public string fromLng { get; set; }
        public string toLat { get; set; }
        public string toLng { get; set; }

        public override string ToString()
        {
            return string.Format("[RideHistory: ID={0}, sessionKey={1}, distance={2}, time={3}, price={4}, startLoc={5}, destination={6}, fromLat={7}, fromLng={8}, toLat={9}, toLng={10}]", ID, sessionKey, distance, time, price, startLoc, destination, fromLat, fromLng, toLat, toLng);
        }
        
    }
}

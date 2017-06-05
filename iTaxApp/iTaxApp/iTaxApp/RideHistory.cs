using SQLite;

namespace iTaxApp
{
    [Table("RideHistory")]
    class RideHistory
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string user { get; set; }
        public string driverID { get; set; }
        public string rideDate { get; set; }
        public string price { get; set; }
        public string rating { get; set; }

        public override string ToString()
        {
            return string.Format("[RideHistory: ID={0}, userID={1}, driverID={2}, rideData={3}, price={4}, rating={5}]", ID, user, driverID, rideDate, price, rating);
        }
    }
}

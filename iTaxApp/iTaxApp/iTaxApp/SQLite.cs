using SQLite;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace iTaxApp
{
    class SQLite
    {
        /// <summary>
        /// Method that connects to an existing database.
        /// </summary>
        public static void ConnectDatabase()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "iTax.db3");
            App.Current.Properties["dbPath"] = dbPath; //Store the db path
            var db = new SQLiteConnection(dbPath);
        }
        /// <summary>
        /// Method that tries to connect to a database and creates a new one if there is no existing database.
        /// It also stores the path as a property of the app, so it can be referenced when necessary.
        /// This method also creates tables as they are predefined in their respective C# table code files.
        /// </summary>
        public static void CreateDatabase()
        {
            Console.WriteLine("Creating database, if it doesn't already exist");
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "iTax.db3");
            App.Current.Properties["dbPath"] = dbPath; //Store the db path
            var db = new SQLiteConnection(dbPath);
            // TABLES CREATION
            db.CreateTable<TempData>();
            db.CreateTable<RideHistory>();
        }
        /// <summary>
        /// Quick method to delete all records in the RideHistory table.
        /// </summary>
        public static void ClearHistory()
        {
            var db = new SQLiteConnection(Convert.ToString(App.Current.Properties["dbPath"]));
            db.DeleteAll<RideHistory>();
        }
        /// <summary>
        /// Method to write temporary data into the TempData table.
        /// This method deletes all records prior to writing anything, so there are no unwanted data artifacts in the table.
        /// </summary>
        /// <param name="data"></param>
        public static void InsertTempData(TempData data)
        {
            var db = new SQLiteConnection(Convert.ToString(App.Current.Properties["dbPath"]));
            db.DeleteAll<TempData>();
            if (db.Table<TempData>().Count() == 0)
            {
                // Only insert the data if it doesn't already exist, else update.
                Console.WriteLine("WRITE: " + data.ToString());
                db.Insert(data);
                Console.WriteLine("Writing data...");
            }
            else
            {
                Console.WriteLine("UPDATE: " + data.ToString());
                db.Update(data);
                Console.WriteLine("Updating data...");

            }
        }
        /// <summary>
        /// Method to read the written data into the TempData table.
        /// </summary>
        /// <returns> TempData object filled with whatever data was available in the table. </returns>
        public static TempData ReadTempData()
        {
            var db = new SQLiteConnection(Convert.ToString(App.Current.Properties["dbPath"]));
            var table = db.Table<TempData>();
            Console.WriteLine("Reading data...");
            TempData data = new TempData();
            foreach (var s in table)
            {
                data.ID = s.ID;
                data.sessionKey = s.sessionKey;
                data.distance = s.distance;
                data.time = s.time;
                data.price = s.price;
                data.startLoc = s.startLoc;
                data.destination = s.destination;
                data.fromLat = s.fromLat;
                data.fromLng = s.fromLng;
                data.toLat = s.toLat;
                data.toLng = s.toLng;
            }
            Console.WriteLine("READ: " + data.ToString());
            return data;
        }
        /// <summary>
        /// Simple method to store a RideHistory object into the relevant table in the database.
        /// This method should be called every time an order is placed succefully.
        /// </summary>
        /// <param name="history"></param>
        public static void InsertHistoryData(RideHistory history)
        {
            var db = new SQLiteConnection(Convert.ToString(App.Current.Properties["dbPath"]));
            Console.WriteLine("WRITE: " + history.ToString());
            db.Insert(history);
            Console.WriteLine("Writing data...");
        }
        /// <summary>
        /// Method to read the data in the RideHistory table.
        /// </summary>
        /// <returns> Collection of RideHistory objects that is bound to the view on the history page. </returns>
        public static ObservableCollection<RideHistory> ReadHistoryData()
        {
            var db = new SQLiteConnection(Convert.ToString(App.Current.Properties["dbPath"]));
            var table = db.Table<RideHistory>();
            Console.WriteLine("Reading data...");
            ObservableCollection<RideHistory> historyList = new ObservableCollection<RideHistory>();
            foreach (var s in table)
            {
                historyList.Add(new RideHistory() { ID = s.ID, user = s.user, driverID = s.driverID, rideDate = s.rideDate, price = s.price, rating = s.rating });
            }
            return historyList;
        }
    }
}

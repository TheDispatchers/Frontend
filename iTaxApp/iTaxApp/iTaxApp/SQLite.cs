using SQLite;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;

namespace iTaxApp
{
    class SQLite
    {
        public static void ConnectDatabase()
        {
            string dbPath = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                 "iTax.db3");
            App.Current.Properties["dbPath"] = dbPath;
            var db = new SQLiteConnection(dbPath);
        }
        public static void CreateDatabase()
        {
            Console.WriteLine("Creating database, if it doesn't already exist");
            string dbPath = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                 "iTax.db3");
            App.Current.Properties["dbPath"] = dbPath;
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<TempData>();
            db.CreateTable<RideHistory>();
        }
        public static void ClearHistory()
        {
            var db = new SQLiteConnection(Convert.ToString(App.Current.Properties["dbPath"]));
            db.DeleteAll<RideHistory>();
        }
        public static void InsertTempData(TempData data)
        {
            var db = new SQLiteConnection(Convert.ToString(App.Current.Properties["dbPath"]));
            db.DeleteAll<TempData>();
            if (db.Table<TempData>().Count() == 0)
            {
                // only insert the data if it doesn't already exist
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

        public static void InsertHistoryData(RideHistory history)
        {
            var db = new SQLiteConnection(Convert.ToString(App.Current.Properties["dbPath"]));
            Console.WriteLine("WRITE: " + history.ToString());
            db.Insert(history);
            Console.WriteLine("Writing data...");
        }
        public static ObservableCollection<RideHistory> ReadHistoryData()
        {
            var db = new SQLiteConnection(Convert.ToString(App.Current.Properties["dbPath"]));
            var table = db.Table<RideHistory>();
            Console.WriteLine("Reading data...");
            var listView = new ListView(); //Maybe not necessary here?
            ObservableCollection<RideHistory> historyList = new ObservableCollection<RideHistory>();
            foreach (var s in table)
            {
                historyList.Add(new RideHistory() { ID = s.ID, user = s.user, driverID = s.driverID, rideDate = s.rideDate, price = s.price, rating = s.rating });
            }
            listView.ItemsSource = historyList; //Maybe not necessary here?
            return historyList;
        }
    }
}

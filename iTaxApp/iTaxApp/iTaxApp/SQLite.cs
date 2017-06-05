using SQLite;
using System;
using System.IO;

namespace iTaxApp
{
    class SQLite
    {
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
    }
}

namespace iTaxApp
{
    public class User
    {
        public string function { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string sessionKey { get; set; }
        public User(string uname, string pass)
        {
            username = uname;
            password = pass;
        }
    }
}

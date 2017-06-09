namespace iTaxApp
{
    public class NewUser
    {
        public string function { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int carTypeID { get; set; }
        public string response { get; set; }
        public NewUser(string mail, string uname, string pass, string fname, string lname, int car)
        {
            email = mail;
            username = uname;
            password = pass;
            firstName = fname;
            lastName = lname;
            carTypeID = car;
        }
    }
}

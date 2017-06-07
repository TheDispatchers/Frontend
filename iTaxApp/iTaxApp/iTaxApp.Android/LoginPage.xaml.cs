using iTaxApp;
using System;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace iTaxApp
{
    public partial class LoginPage : ContentPage
    {
        User client;
        int counter = 0;
        public LoginPage()
        {
            InitializeComponent();
        }

        async void OnLogin(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new MainPage());

            
            if (username.Text != null || password.Text != null)
            {
                client = new User(username.Text, Core.LoginSystem.CalculateMD5Hash(password.Text));
                client.function = "login";
            }
            else
            {
                client = new User("user", "pass");
            }
            object obj = SynchronousSocketClient.StartClient("login", client);
            client = (User)obj;
            App.Current.Properties["sessionKey"] = client.sessionKey;
            App.Current.Properties["user"] = client.username;
            if (!client.sessionKey.Equals("invalid") && !client.sessionKey.Equals("") && !client.sessionKey.Equals(null) && client.sessionKey.Length == 32)
            {
                await this.DisplayAlert("Login", "User " + client.username + " logged in.", "Continue");
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            }
            else
            {
                await this.DisplayAlert("Login", "Make sure you entered correct credentials and that you are connected to the internet.", "Continue");
            }    
        }
        protected override bool OnBackButtonPressed()
        {
            if (counter == 0)
            {
                DependencyService.Get<IMessage>().ShortAlert("Press again to exit.");
                counter += 1;
                return true;
            }
            else
            {
                counter = 0;
                return false;
            }

        }
        async void OnRegister(object sender, EventArgs e)

        {
            if (await this.DisplayAlert(
                    "Register",
                    "Would you like to create a new account?",
                    "Yes",
                    "No"))
            {
                await Navigation.PushAsync(new RegisterPage());
            }
        }
        void OnTest(object sender, EventArgs e)
        {
            bool test = SynchronousSocketClient.TestConnection();
            if (test)
            {
                DependencyService.Get<IMessage>().ShortAlert("Connection established.");
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Connection NOT established.");
            }
        }
    }
}
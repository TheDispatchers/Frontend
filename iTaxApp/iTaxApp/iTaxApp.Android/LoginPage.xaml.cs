using iTaxApp;
using System;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace iTaxApp
{
    public partial class LoginPage : ContentPage
    {
        User client;
        public LoginPage()
        {
            InitializeComponent();
        }

        async void OnLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
            
            /*
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
            if (!client.sessionKey.Equals("invalid"))
            {
                await this.DisplayAlert("Login", "User " + client.username + " logged in.", "Continue");
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            }
            else
            {
                await this.DisplayAlert("Login", "Make sure you entered correct credentials and that you are connected to the internet.", "Continue");
            }
            */
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
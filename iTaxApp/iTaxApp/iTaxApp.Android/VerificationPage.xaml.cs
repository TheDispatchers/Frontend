using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iTaxApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerificationPage : ContentPage
    {
        NewUser newUser;
        public VerificationPage()
        {
            InitializeComponent();
        }

        
        async void OnVerify()
        {
            if (code.Text != null)
            {
                newUser.username = Convert.ToString(App.Current.Properties["username"]);
                newUser.password = Convert.ToString(App.Current.Properties["password"]);
                newUser.function = "confirmRegister";
                object obj = SynchronousSocketClient.StartClient("confirmRegister", newUser);
                newUser = (NewUser)obj;
                if (newUser.response.Equals("success", StringComparison.OrdinalIgnoreCase))
                {
                    DependencyService.Get<IMessage>().ShortAlert("Server says: " + newUser.response);
                    await this.DisplayAlert("Register", "Your e-mail has been succesfully verified. You can log in now.", "Go to login");
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Server says: " + newUser.response);
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Verification code can NOT be empty.");
            }

        }
    }

}
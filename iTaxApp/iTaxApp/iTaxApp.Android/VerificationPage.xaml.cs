using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iTaxApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerificationPage : ContentPage
    {
        private NewUser newUser;
        public VerificationPage(NewUser newUser)
        {
            InitializeComponent();
            this.newUser = newUser;
        }
        async void OnVerify()
        {
            if (code.Text != null)
            {
                newUser.function = "confirmRegister";
                newUser.code = code.Text;
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
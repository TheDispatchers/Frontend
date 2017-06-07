using Android.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace iTaxApp
{
    class MailSystem
    {
        public static void SendMail(string recipient)
        {
            var email = new Intent(Intent.ActionSend);
            email.PutExtra(Intent.ExtraEmail, recipient);
           
            email.PutExtra(Intent.ExtraSubject, "iTax Verification");

            email.PutExtra(Android.Content.Intent.ExtraText,
            "Hello, thank you for registering with iTax. Your verification code is: " + VerificationNumber());

            email.SetType("message/rfc822");

            //StartActivity(email);
        }

        
        public static int VerificationNumber()
        {
            Random rand = new Random();
            return rand.Next(000000, 999999);
        }


    }
}

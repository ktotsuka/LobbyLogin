using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Collections.ObjectModel;
using System.Threading;

namespace SignInMail
{
    public static class Mail
    {
        private static readonly ReadOnlyCollection<string> carriers = new ReadOnlyCollection<string>(new[]
        {
            "vtext.com",
            "txt.att.net",
            "messaging.sprintpcs.com",
            "tmomail.net",
            "vmobl.com",
            "messaging.nextel.com",
            "myboostmobile.com",
            "message.alltel.com"
        });

        public static ReadOnlyCollection<string> Carriers
        {
            get { return carriers; }
        }

        public static List<string> GetPhoneEmailAddresses(string phone_number)
        {
            List<string> addresses = new List<string>();

            foreach (string carrier in carriers)
            {
                addresses.Add(phone_number + "@" + carrier);
            }
            return addresses;
        }

        public static void SendEmail(List<string> email_addresses, string message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("bav.georgetown@gmail.com");
                foreach (string address in email_addresses)
                {
                    mail.To.Add(address);
                }
                mail.Subject = "Visitor alert";
                mail.Body = message;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("bav.georgetown", "Georgetown@4321!");
                SmtpServer.EnableSsl = true;

                //SmtpServer.Send(mail);
                Thread.Sleep(10000);                
            }
            catch
            {

            }
        }

        public static bool IsValidEmail(string email_address)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email_address);
                return addr.Address == email_address;
            }
            catch
            {
                return false;
            }
        }
    }
}

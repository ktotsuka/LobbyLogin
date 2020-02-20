using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Collections.ObjectModel;

namespace LobbyLogin
{
    public class Mail
    {
        //public string[] carriers = new string[4] { "vtext.com", "txt.att.net", "messaging.sprintpcs.com", "tmomail.net"};

        private static readonly ReadOnlyCollection<string> carriers = new ReadOnlyCollection<string>(new[]
        {
            "vtext.com",
            "txt.att.net",
            "messaging.sprintpcs.com",
            "tmomail.net"
        });

        public static ReadOnlyCollection<string> Carriers
        {
            get { return carriers; }
        }

        public static void SendText(string phone_number, string message)
        {
            List<string> addresses = new List<string>();

            foreach (string carrier in carriers)
            {
                addresses.Add(phone_number + "@" + carrier);
            }
            SendEmail(addresses, message);
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

                SmtpServer.Send(mail);
            }
            catch
            {

            }
        }
    }
}
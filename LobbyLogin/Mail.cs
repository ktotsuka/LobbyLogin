using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace LobbyLogin
{
    public class Mail
    {
        public string[] carriers = new string[4] { "vtext.com", "txt.att.net", "messaging.sprintpcs.com", "tmomail.net"};

        public static void SendText()
        {

        }

        public static void SendEmail(string email_address, string message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("bav.georgetown@gmail.com");
                mail.To.Add(email_address);
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
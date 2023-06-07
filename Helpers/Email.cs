using BookStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace BookStore.Helpers
{
    public class Email
    {
        public void SendEmail(string subject, string body, string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
                mail.From = from;
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.To.Add(email);

                //mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential("HotelListVX@gmail.com", "ujzzmzrxomafbwkb");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
                //Dispose of email.
                mail.Dispose();
            }
            catch (Exception ex)
            {
                //alternate logic
            }

        }
    }
}
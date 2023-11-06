using BookStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Configuration;

namespace BookStore.Helpers
{
    public class Email
    {
        public void SendEmail(string subject, string body, string email, Attachment attachment = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
                mail.From = from;
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;
                if (attachment != null)
                {
                    mail.Attachments.Add(attachment);
                }
                mail.To.Add(email);

                mail.IsBodyHtml = true;
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
        public void SendEmail(string subject, string name, string body, string email, string emailType)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
                mail.From = from;
                mail.Subject = subject;
                mail.IsBodyHtml = true;

                if (emailType == "e_appointment_confirm")
                {
                    var emailContent = EmailTemplates.AppointmentConfirmation;
                    emailContent = emailContent.Replace("[UserFullname]", name);
                    emailContent = emailContent.Replace("[EMAILBODY]", body);
                    emailContent = emailContent.Replace("[LOGINURL]", String.Format("{0}/Account/Login", ConfigurationManager.AppSettings["SITE_URL"]));
                    mail.Body = emailContent;
                }
                else if(emailType == "e_repair_confirmation")
                {
                    var emailContent = EmailTemplates.RepairConfirmation;
                    emailContent = emailContent.Replace("[CUSTOMERNAME]", name);
                    emailContent = emailContent.Replace("[EMAILBODY]", body);
                    emailContent = emailContent.Replace("[LOGINURL]", String.Format("{0}/Account/Login", ConfigurationManager.AppSettings["SITE_URL"]));
                    emailContent = emailContent.Replace("[SHOPNOWURL]", String.Format("{0}/Products/OurProducts", ConfigurationManager.AppSettings["SITE_URL"]));
                    mail.Body = emailContent;
                }
                else if(emailType == "e_auto_repair_complete")
                {
                    var emailContent = EmailTemplates.AutoRepaired;
                    emailContent = emailContent.Replace("[CUSTOMERNAME]", name);
                    emailContent = emailContent.Replace("[EMAILBODY]", body);
                    emailContent = emailContent.Replace("[LOGINURL]", String.Format("{0}/Account/Login", ConfigurationManager.AppSettings["SITE_URL"]));
                    emailContent = emailContent.Replace("[SHOPNOWURL]", String.Format("{0}/Products/OurProducts", ConfigurationManager.AppSettings["SITE_URL"]));
                    emailContent = emailContent.Replace("[SITEURL]",  ConfigurationManager.AppSettings["SITE_URL"]);
                    mail.Body = emailContent;
                }
                else
                {
                    mail.Body = body;
                }

                mail.To.Add(email);

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential("HotelListVX@gmail.com", "ujzzmzrxomafbwkb");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
                mail.Dispose();
            }
            catch (Exception ex)
            {
                //alternate logic
            }

        }
    }
}
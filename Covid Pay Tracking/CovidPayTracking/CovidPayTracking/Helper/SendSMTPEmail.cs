using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CovidPayTracking.Helper
{
    public static class SendSMTPEmail
    {
        /// <summary>
        /// This is used to send email 
        /// To -> Single email Address
        /// </summary>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public static bool SendEmail(string body, string subject, string mailTo)
        {
            try
            {
                var smtpClient = new SmtpClient();
                var mailmessage = new MailMessage
                {
                    From = new MailAddress("shruti.patrabansh@northside.com"),
                    Body = body,
                    Subject = subject,
                    IsBodyHtml=true
                };
                mailmessage.To.Add(mailTo);

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// This is used to send email 
        /// To -> <see cref="List{string}"/> of email addresses
        /// </summary>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public static bool SendEmail(string body, string subject, List<string> mailTo)
        {
            try
            {
                var smtpClient = new SmtpClient();
                var mailmessage = new MailMessage
                {
                    From = new MailAddress("shruti.patrabansh@northside.com"),
                    Body = body,
                    Subject = subject,
                    IsBodyHtml = true
                };
                foreach (var email in mailTo)
                {
                    mailmessage.To.Add(email);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.EmailSenders.Interfaces;

namespace Clinic.Core.EmailSenders
{
    public class GMailSender : IEmailSender
    {
        private const string Email = "Developingtest741@gmail.com";
        private const string Password = "741741000";
        public Task Execute(string userEmail, string body, string title)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(Email, Password);

            MailMessage message = new MailMessage(Email, userEmail, title, body);
            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            client.Send(message);
            return Task.CompletedTask;
        }
    }
}

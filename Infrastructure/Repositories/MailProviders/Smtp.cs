using Application.Mail.Interfaces;
using Application.Mail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.MailProviders
{
    public class Smtp : IMailServiceProvider
    {
        public async Task Send(Settings settings, string messageString, string titleString)
        {
            MailMessage message = new MailMessage();

            message.Subject = titleString;
            message.Body = messageString;
            message.IsBodyHtml = true;
            foreach (var address in settings.ToAddresses)
            {
                message.To.Add(address);
            }
            message.From = new MailAddress(settings.FromAddress);

            SmtpClient client = new SmtpClient();
            client.Host = settings.Server ?? string.Empty;
            client.Port = settings.Port ?? 0;
            client.EnableSsl = settings.SSL;
            client.Credentials = new NetworkCredential(settings.Username, settings.Password);
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
            }
        }
    }
}

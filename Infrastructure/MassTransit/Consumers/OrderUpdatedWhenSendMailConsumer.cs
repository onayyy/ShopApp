using Application.Common.Interfaces;
using Application.Mail.Interfaces;
using Application.Mail.Models;
using Application.Mail.Enums;
using Domain.Events.Order;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MassTransit.Consumers
{
    public class OrderUpdatedWhenSendMailConsumer : IConsumer<OrderUpdateEvent>
    {
        private IMailProviderFactory _mailProviderFactory;

        public OrderUpdatedWhenSendMailConsumer(IMailProviderFactory mailProviderFactory)
        {
            _mailProviderFactory = mailProviderFactory;
        }

        public async Task Consume(ConsumeContext<OrderUpdateEvent> context)
        {
            var settings = new Settings();

            settings.TemplateHtml = "<p>Sipariş Durumu.</p>";
            settings.Title = "Sipariş Durumu Bilgilendirme.";
            settings.Server = "smtp.gmail.com";
            settings.Username = "cimenfurkan19@gmail.com";
            settings.Password = "codgublylhhrzohq";
            settings.SSL = true;
            settings.Port = 587;
            settings.FromAddress = "cimenfurkan19@gmail.com";
            settings.ToAddresses = new List<string>
                {
                    "onay5400@gmail.com"  // tokendaki userId ye göre email getirme işlemi yapılacak.
                };

            settings.Provider = (MailServiceProvider)1;

            IMailServiceProvider mailServiceProvider = _mailProviderFactory.GetProvider(settings);

            if (mailServiceProvider is null)
            {
                throw new Exception("PROVIDER_NOT_FOUND");
            }

            await mailServiceProvider.Send(settings, context.Message.Defination, context.Message.Subject);

        }
    }
}

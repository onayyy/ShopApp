using Application.Common.Interfaces;
using Application.DTOs;
using Application.Mail.Interfaces;
using Application.Mail.Models;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Commands
{
    public class UpdateOrderStatusCommand : IRequest<OrderAggregate.OrderStatus>
    {
        public int Id { get; set; }

        public OrderAggregate.OrderStatus Status { get; set; }

        public UpdateOrderStatusCommand(int id, OrderAggregate.OrderStatus status)
        {
            Id = id;
            Status = status;
        }

        public class Handler : IRequestHandler<UpdateOrderStatusCommand, OrderAggregate.OrderStatus>
        {
            private readonly IPizzaAppDbContext _dbContext;
            private readonly IMailProviderFactory _mailProviderFactory;

            public Handler(IPizzaAppDbContext dbContext, IMailProviderFactory mailProviderFactory)
            {
                _dbContext = dbContext;
                _mailProviderFactory = mailProviderFactory;
            }

            public async Task<OrderAggregate.OrderStatus> Handle (UpdateOrderStatusCommand request, CancellationToken cancellationToken)
            {
                var order = await _dbContext.Orders.FindAsync(request.Id, cancellationToken);

                if (order is null)
                    throw new Exception($"Order with ID {request.Id} not found.");

                order.OrderStatusUpdate(request.Status);

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

                settings.Provider = (Mail.Enums.MailServiceProvider) 1;

                IMailServiceProvider mailServiceProvider = _mailProviderFactory.GetProvider(settings);

                if (mailServiceProvider is null) 
                {
                    throw new Exception("PROVIDER_NOT_FOUND");
                }

                var defination = request.Status.ToString();
                var subject = request.Status.ToString();

                mailServiceProvider.Send(settings, defination, subject);

                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return request.Status;
            }

        }
    }
}

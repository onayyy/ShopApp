using Application.Common.Interfaces;
using Application.DTOs;
using Application.Mail.Interfaces;
using Application.Mail.Models;
using Domain.Events.Order;
using Domain.Model;
using MassTransit;
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
            private readonly ISendEndpointProvider _sendEndpointProvider;

            public Handler(IPizzaAppDbContext dbContext, ISendEndpointProvider sendEndpointProvider)
            {
                _dbContext = dbContext;
                _sendEndpointProvider = sendEndpointProvider;
            }

            public async Task<OrderAggregate.OrderStatus> Handle (UpdateOrderStatusCommand request, CancellationToken cancellationToken)
            {
                var order = await _dbContext.Orders.FindAsync(request.Id, cancellationToken);

                if (order is null)
                    throw new Exception($"Order with ID {request.Id} not found.");

                order.OrderStatusUpdate(request.Status);

                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:shopApp.order.updated"));
                await endpoint.Send(new OrderUpdateEvent
                {
                    Subject = order.Status.ToString(),
                    Defination = "Sipariş Durumu",
                }, cancellationToken);

                return request.Status;
            }

        }
    }
}

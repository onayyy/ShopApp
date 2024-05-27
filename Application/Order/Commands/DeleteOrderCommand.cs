using Application.Common.Interfaces;
using Application.Product.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Commands
{
    public class DeleteOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteOrderCommand(int ıd)
        {
            Id = ıd;
        }

        public class Handler : IRequestHandler<DeleteOrderCommand, Unit>
        {
            private readonly IPizzaAppDbContext _dbContext;

            public Handler(IPizzaAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var order = await _dbContext.Orders.FindAsync(request.Id, cancellationToken);
                if (order == null)
                {
                    throw new Exception($"Product with ID {request.Id} not found.");
                }
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }


        }
    }
}

using Application.Common.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteProductCommand(int productId)
        {
            Id = productId;
        }

        public class Handler : IRequestHandler<DeleteProductCommand, Unit>
        {
            private readonly IPizzaAppDbContext _dbContext;

            public Handler(IPizzaAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var product = await _dbContext.Products.FindAsync(request.Id, cancellationToken);
                if (product == null)
                {
                    throw new Exception($"Product with ID {request.Id} not found.");
                }
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }


        }
    }
}

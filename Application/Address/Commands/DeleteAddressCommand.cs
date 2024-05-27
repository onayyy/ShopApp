using Application.Common.Interfaces;
using Application.Order.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Address.Commands
{
    public class DeleteAddressCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteAddressCommand(int id)
        {
            Id = id;
        }

        public class Handler : IRequestHandler<DeleteAddressCommand, Unit>
        {
            private readonly IPizzaAppDbContext _dbContext;

            public Handler(IPizzaAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
            {
                var address = await _dbContext.Address.FindAsync(request.Id, cancellationToken);
                if (address == null)
                {
                    throw new Exception($"Address with ID {request.Id} not found.");
                }
                _dbContext.Address.Remove(address);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }


        }
    }
}

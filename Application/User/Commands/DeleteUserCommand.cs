using Application.Common.Interfaces;
using Application.Product.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Commands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteUserCommand(int userId)
        {
            Id = userId;
        }

        public class Handler : IRequestHandler<DeleteUserCommand, Unit>
        {
            private readonly IPizzaAppDbContext _dbContext;

            public Handler(IPizzaAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.Users.FindAsync(request.Id, cancellationToken);
                if (user == null)
                {
                    throw new Exception($"User with ID {request.Id} not found.");
                }
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }


        }
    }
}

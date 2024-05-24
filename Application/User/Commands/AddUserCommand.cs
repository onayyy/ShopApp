using Application.Common.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Commands
{
    public class AddUserCommand : IRequest<UserAggregate>
    {
        public string Name { get; set; }


        public AddUserCommand(string name)
        {
            Name = name;
        }

        public class Handler : IRequestHandler<AddUserCommand, UserAggregate>
        {
            private readonly IPizzaAppDbContext _dbContext;

            public Handler(IPizzaAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<UserAggregate> Handle(AddUserCommand request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    throw new Exception("PRODUCT_NAME_CANNOT_BE_EMPTY");
                }

                var user = UserAggregate.Create(request.Name);
                await _dbContext.Users.AddAsync(user, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return user;
            }
        }
    }
}

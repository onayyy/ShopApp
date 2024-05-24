using Application.Common.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Commands
{
    public class AddAddressWithUserByIdCommand : IRequest<UserAggregate>
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public AddAddressWithUserByIdCommand(int id, string city, string street)
        {
            Id = id;
            City = city;
            Street = street;
        }

        public class Handler : IRequestHandler<AddAddressWithUserByIdCommand, UserAggregate>
        {
            private readonly IPizzaAppDbContext _dbContext;

            public Handler(IPizzaAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<UserAggregate> Handle(AddAddressWithUserByIdCommand request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.Users
                               .Include(u => u.Addresses)
                               .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

                if (user == null)
                {
                    throw new Exception($"Product with ID {request.Id} not found.");
                }

                if (request.City == null || request.Street == null)
                {
                    throw new Exception("ADDRESS_CANNOT_BE_EMPTY");
                }

                user.Addresses.Add(AddressAggregate.Create(user.Id, request.City, request.Street));
                await _dbContext.SaveChangesAsync(cancellationToken);
                return user;
            }
        }
    }
}

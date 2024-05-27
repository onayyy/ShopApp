using Application.Common.Interfaces;
using Application.User.Commands;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Address.Commands
{
    public class UpdateAddressCommand : IRequest<AddressAggregate>
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public UpdateAddressCommand(int id, string city, string street)
        {
            Id = id;
            City = city;
            Street = street;
        }

        public class Handler : IRequestHandler<UpdateAddressCommand, AddressAggregate>
        {
            private readonly IPizzaAppDbContext _dbContext;

            public Handler(IPizzaAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<AddressAggregate> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
            {
                var address = await _dbContext.Address.FindAsync(request.Id);
           
                if (address == null)
                {
                    throw new Exception($"Product with ID {request.Id} not found.");
                }

                if (request.City == null || request.Street == null)
                {
                    throw new Exception("ADDRESS_CANNOT_BE_EMPTY");
                }

                address.Update(request.City, request.Street);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return address;
            }
        }
    }
}

using Application.Common.Interfaces;
using Application.User.Queries;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Address.Queries
{
    public class GetAddressByIdQuery : IRequest<AddressAggregate>
    {
        public GetAddressByIdQuery(int id)
        {
            AddressId = id;
        }

        public int AddressId { get; set; }

        public class Handler : IRequestHandler<GetAddressByIdQuery, AddressAggregate>
        {
            private readonly IPizzaAppDbContext _pizzaAppDbContext;

            public Handler(IPizzaAppDbContext pizzaAppDbContext)
            {
                _pizzaAppDbContext = pizzaAppDbContext;
            }

            public async Task<AddressAggregate> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
            {
                var dbQuery = _pizzaAppDbContext.Address.AsQueryable();

                if (request.AddressId > 0)
                {
                    dbQuery = dbQuery.Where(x => x.Id == request.AddressId);
                }

                var address = await dbQuery.FirstOrDefaultAsync(cancellationToken);

                if (address is null)
                {
                    throw new Exception("Address not found");
                }

                return address;
            }
        }
    }
}

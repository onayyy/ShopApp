using Application.Common.Interfaces;
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
    public class GetAddressQuery : IRequest<List<AddressAggregate>>
    {
        public class Handler : IRequestHandler<GetAddressQuery, List<AddressAggregate>>
        {
            private readonly IPizzaAppDbContext _pizzaAppDbContext;

            public Handler(IPizzaAppDbContext pizzaAppDbContext)
            {
                _pizzaAppDbContext = pizzaAppDbContext;
            }

            public async Task<List<AddressAggregate>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
            {
                var dbQuery = _pizzaAppDbContext.Address.AsQueryable();


                var address = await dbQuery.ToListAsync(cancellationToken);

                if (address is null)
                {
                    throw new Exception("Address not found");
                }

                return address;
            }
        }
    }
}

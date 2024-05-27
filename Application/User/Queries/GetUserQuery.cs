using Application.Common.Interfaces;
using Application.Product.Queries;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Queries
{
    public class GetUserQuery : IRequest<List<UserAggregate>>
    {
        public class Handler : IRequestHandler<GetUserQuery, List<UserAggregate>>
        {
            private readonly IPizzaAppDbContext _pizzaAppDbContext;

            public Handler(IPizzaAppDbContext pizzaAppDbContext)
            {
                _pizzaAppDbContext = pizzaAppDbContext;
            }

            public async Task<List<UserAggregate>> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var dbQuery = _pizzaAppDbContext.Users.AsQueryable();

                var users = await dbQuery.Include(x => x.Addresses).ToListAsync(cancellationToken);

                return users;
            }
        }
    }
}

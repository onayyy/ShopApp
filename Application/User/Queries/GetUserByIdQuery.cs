using Application.Common.Interfaces;
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
    public class GetUserByIdQuery : IRequest<UserAggregate>
    {
        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; set; }

        public class Handler : IRequestHandler<GetUserByIdQuery, UserAggregate>
        {
            private readonly IPizzaAppDbContext _pizzaAppDbContext;

            public Handler(IPizzaAppDbContext pizzaAppDbContext)
            {
                _pizzaAppDbContext = pizzaAppDbContext;
            }

            public async Task<UserAggregate> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var dbQuery = _pizzaAppDbContext.Users.AsQueryable();

                if (request.UserId > 0)
                {
                    dbQuery = dbQuery.Where(x => x.Id == request.UserId);
                }

                var users = await dbQuery.Include(x => x.Addresses).FirstOrDefaultAsync(cancellationToken);

                if (users is null)
                {
                    throw new Exception("User not found");
                }

                return users;
            }
        }
    }
}

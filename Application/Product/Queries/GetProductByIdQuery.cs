using Application.Common.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Queries
{
    public class GetProductByIdQuery : IRequest<ProductAggregate>
    {
        public int ProductId { get; set; }

        public GetProductByIdQuery(int productId)
        {
            ProductId = productId;
        }

        public class Handler : IRequestHandler<GetProductByIdQuery, ProductAggregate>
        {
            private readonly IPizzaAppDbContext _pizzaAppDbContext;

            public Handler(IPizzaAppDbContext pizzaAppDbContext)
            {
                _pizzaAppDbContext = pizzaAppDbContext;
            }

            public async Task<ProductAggregate> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                var dbQuery = _pizzaAppDbContext.Products.AsQueryable();

                if (request.ProductId > 0)
                {
                    dbQuery = dbQuery.Where(x => x.Id == request.ProductId);
                }

                var product = await dbQuery.Include(x => x.Orders).FirstOrDefaultAsync(cancellationToken);

                if (product is null)
                {
                    throw new Exception("Product not found");
                }

                return product;
            }
        }
    }
}

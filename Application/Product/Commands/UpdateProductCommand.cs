using Application.Common.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Commands
{
    public class UpdateProductCommand : IRequest<ProductAggregate>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public List<Ingredients> Ingredients { get; set; }
        public double Quantity { get; set; }

        public UpdateProductCommand(int id, string name, string description, double price, List<Ingredients> ingredients, double quantity)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Ingredients = ingredients;
            Quantity = quantity;
        }

        public class Handler : IRequestHandler<UpdateProductCommand, ProductAggregate>
        {
            private IMediator _mediator;
            private IPizzaAppDbContext _dbContext;

            public Handler(IMediator mediator, IPizzaAppDbContext dbContext)
            {
                _mediator = mediator;
                _dbContext = dbContext;
            }

            public async Task<ProductAggregate> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var product = await _dbContext.Products.FindAsync(request.Id, cancellationToken);
                if (product == null)
                {
                    throw new Exception($"Product with ID {request.Id} not found.");
                }

                if (request.Ingredients != null && request.Ingredients.Count > 0)
                {
                    var ingredients = request.Ingredients.Select(x => new Ingredients
                    {
                        Key = x.Key,
                        Value = x.Value,
                    }).ToList();

                    product.Ingredients = ingredients;
                }

                if (!string.IsNullOrEmpty(request.Name) && !string.IsNullOrEmpty(request.Description))
                {
                    product.Name = request.Name;
                    product.Description = request.Description;
                }

                product.Price = request.Price;
                product.Quantity = request.Quantity;

                _dbContext.Products.Update(product);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return product;
            }
        }


    }
}

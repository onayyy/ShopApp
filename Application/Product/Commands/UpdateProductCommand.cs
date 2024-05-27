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
            private IPizzaAppDbContext _dbContext;

            public Handler(IPizzaAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<ProductAggregate> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var product = await _dbContext.Products.FindAsync(request.Id, cancellationToken);

                if (product == null)
                    throw new Exception($"Product with ID {request.Id} not found.");

                if (request.Ingredients == null)
                    throw new Exception("INGREDIENTS_CANNOT_BE_EMPTY");

                if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Description))
                    throw new Exception("NAME_OR_DESCRIPTION_CANNOT_BE_EMPTY");

                product.Update(request.Name, request.Description, request.Price, request.Ingredients, request.Quantity);
                _dbContext.Products.Update(product);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return product;
            }
        }


    }
}

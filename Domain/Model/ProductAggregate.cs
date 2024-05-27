using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ProductAggregate
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Price { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public List<Ingredients> Ingredients { get; private set; }
        public double Quantity { get; private set; }
        [JsonIgnore]
        public virtual List<OrderAggregate> Orders { get; private set; }

        public ProductAggregate()
        {
            // only db
        }
        private ProductAggregate(string name, string description, double price, List<Ingredients> ingredients, double quantity)
        {
            Name = name;
            Description = description;
            Price = price;
            CreatedDate = DateTime.Now;
            Ingredients = ingredients;
            Quantity = quantity;
        }

        public static ProductAggregate Create(string name, string description, double price, List<Ingredients> ingredients, double quantity)
        {
            return new ProductAggregate(name, description, price, ingredients, quantity);
        }

        public void Update(string name, string description, double price, List<Ingredients> ingredients, double quantity)
        {
            Name = name;
            Description = description;
            Price = price;
            Ingredients = ingredients;
            Quantity = quantity;
        }

        public void DeincrementQuantity()
        {
            Quantity--;
        }
 
    }
}

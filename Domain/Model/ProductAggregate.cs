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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; private set; }
        public DateTime CreatedDate { get; set; }
        public List<Ingredients> Ingredients { get; set; }
        public double Quantity { get; set; }
        [JsonIgnore]
        public virtual List<OrderAggregate> Orders { get; set; }

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

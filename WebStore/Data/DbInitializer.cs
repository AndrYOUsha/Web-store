using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Models;

namespace WebStore.Data
{
    public static class DbInitializer
    {
        public static void Initializer(ProductContext context)
        {
            if (context.Products.Any())
                return;

            var product1 = new Product()
            {
                Title = "Кроссовки",
                Description = "Неплохие такие кроссовки",
                Discount = 10f,
                Price = 2500M
            };

            var product2 = new Product()
            {
                Title = "Майка",
                Description = "Хлопковая майка",
                Discount = 0f,
                Price = 999.99M
            };

            context.Products.Add(product1);
            context.Products.Add(product2);

            var characteristic1 = new Characteristic()
            {
                Product = product1,
                Article = 156788,
                Brand = "Nike",
                Color = "Black",
                FullName = "Air Monarch IV",
                Brunch = "Air",
                Size = 41f,
                Count = 25f,
                Type = "Shoes",
                Gender = Gender.Male
            };
            var characteristic2 = new Characteristic()
            {
                Product = product1,
                Article = 156789,
                Brand = "Nike",
                Color = "Black",
                FullName = "Air Monarch IV",
                Brunch = "Air",
                Size = 42f,
                Count = 10f,
                Type = "Shoes",
                Gender = Gender.Male
            };
            var characteristic3 = new Characteristic()
            {
                Product = product1,
                Article = 156790,
                Brand = "Nike",
                Color = "Black",
                FullName = "Air Monarch IV",
                Brunch = "Air",
                Size = 43f,
                Count = 6f,
                Type = "Shoes",
                Gender = Gender.Male
            };
            var characteristic4 = new Characteristic()
            {
                Product = product2,
                Article = 895466,
                Brand = "Adidas",
                Color = "White",
                FullName = "Max Extreme",
                Brunch = "Sleeveless",
                SizeISS = InternationalSizingSystem.M,
                Count = 10f,
                Type = "T-Shirt",
                Gender = Gender.Female
            };
            var characteristic5 = new Characteristic()
            {
                Product = product2,
                Article = 895467,
                Brand = "Adidas",
                Color = "White",
                FullName = "Max Extreme",
                Brunch = "Sleeveless",
                SizeISS = InternationalSizingSystem.L,
                Count = 5f,
                Type = "T-Shirt",
                Gender = Gender.Female
            };

            context.Characteristics.Add(characteristic1);
            context.Characteristics.Add(characteristic2);
            context.Characteristics.Add(characteristic3);
            context.Characteristics.Add(characteristic4);
            context.Characteristics.Add(characteristic5);

            context.SaveChanges();
        }
    }
}

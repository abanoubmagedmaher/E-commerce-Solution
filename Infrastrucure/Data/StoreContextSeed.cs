using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastrucure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Infrastrucure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrands.AddRange(brands);
                await context.SaveChangesAsync();
            }
            //ProductTypes
            if (!context.ProductTypes.Any())
            {
                var ProductTypes = File.ReadAllText("../Infrastrucure/Data/SeedData/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(ProductTypes);
                context.ProductTypes.AddRange(Types);
                await context.SaveChangesAsync();
            }
            if (!context.Products.Any())
            {
                //Product
                var ProductData = File.ReadAllText("../Infrastrucure/Data/SeedData/products.json");
                var Product = JsonSerializer.Deserialize<List<Product>>(ProductData);
                context.Products.AddRange(Product);
                await context.SaveChangesAsync();
            }
        }
        }
}

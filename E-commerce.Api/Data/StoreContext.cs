using E_commerce.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Api.Data
{
    public class StoreContext :DbContext
    {
        public StoreContext(DbContextOptions options) :base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}

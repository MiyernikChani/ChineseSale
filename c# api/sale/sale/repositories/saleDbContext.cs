using ChneseSaleApi.models;
using Microsoft.EntityFrameworkCore;
using sale.models;
using System.Threading.Tasks;

namespace ChneseSaleApi.repositories
{
    public class saleDbContext : DbContext
    {
        public saleDbContext(DbContextOptions<saleDbContext> options) : base(options)
        {

        }
        public DbSet<Donator> Donators { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Winner> Winners { get; set; }
        public DbSet<TotalRevenue> totalRevenues { get; set; }
        public DbSet<Count> Counts { get; set; }
    }
}

using ChneseSaleApi.models;
using Microsoft.EntityFrameworkCore;
using sale.models;

namespace sale.repositories
{
    public class DonatorDbContext: DbContext
    {
        public DonatorDbContext(DbContextOptions<DonatorDbContext> options) : base(options)
        {

        }
        public DbSet<Donator> Donators { get; set; }
        //public DbSet<Gift> Gifts { get; set; }
        //public DbSet<Category> Categories { get; set; }

    }
}

using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Repositories
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Product { get; set; }=default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //IEntityTypeConfiguration implement edenlerin hepsini al
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

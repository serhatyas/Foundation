using App.Repositories.Categories;
using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Repositories.Extension
{
    public static class RepostiroyExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            //veritabanına gittiğimiz için scoped kullanıyoruz
            services.AddDbContext<AppDbContext>(options =>
            {
                //appsettingsteki sqlserver connection stringini al
                var connectionStrings = configuration.GetSection
                (ConnectionStringOption.Key).Get<ConnectionStringOption>();

                //burada dinamik ayarlamış olduk

                options.UseSqlServer(connectionStrings!.SqlServer,sqlServerOptionsAction =>
                {

                    //RepositoryAssembly içinde bulunduğu assemblynin adını alıyor
                    sqlServerOptionsAction.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.FullName);
                });
            });
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            //IGenericRepository<> birden fazla generic alsaydı IGenericRepository<,,> araya virgül koyacaktık
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}

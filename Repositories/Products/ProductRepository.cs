using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Products
{
    public class ProductRepository(AppDbContext context) : GenericRepository<Product>(context), IProductRepository
    {
        public Task<List<Product>> GetTopPriceProductAsync(int count)
        {
            //tek satır olduğu için await kullanmadık
            return Context.Product.OrderByDescending(x=>x.Price).Take(count).ToListAsync();
        }
    }
}

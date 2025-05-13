using App.Repositories;
using App.Repositories.Products;
using App.Services.Products;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services
{
    //GENERIC SERVICE YOKTUR!!
    //TODO: IProductRepository ile ProductRepository extensionsta eklediğimiz addScoped sayesinde eşler
    //Interface kullanıyoruz esneklik sağlıyor. Repositoryde bi değişiklik yaptık diyelim
    //sadece DI bağlılığı değiştirerek buraya dokunmaya gerek kalmadan işlemleri tamamlamış oluyorum.
    //örnek vermek gerekirse;
    //services.AddScoped<IProductRepository, ProductRepository>();
    //services.AddScoped<IProductRepository, ProductRepository2>(); 
    //burada sadece DI kaydını değiştirerek esneklik sağlamış olduk.
    //business logic katmanında repository ile iş yapıyoruz
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) :IProductService
    {
        public async Task<ServiceResult<List<ProductsDto>>> GetTopPriceProductAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductAsync(count);

            var productsAsDto = products.Select(x => new ProductsDto(x.Id, x.Name, x.Price, x.Stock)).ToList();

            return new ServiceResult<List<ProductsDto>>()
            {
                Data = productsAsDto,
            };
        }

        public async Task<ServiceResult<List<ProductsDto>>> GetAllListAsync()
        {
            var products = await productRepository.GetAll().ToListAsync();

            var productsAsDto = products.Select(x => new ProductsDto(x.Id, x.Name, x.Price, x.Stock)).ToList();

            return ServiceResult<List<ProductsDto>>.Success(productsAsDto);
        }

        //fail olduğunda ProductsDto boş olabilir 
        public async Task<ServiceResult<ProductsDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult<ProductsDto>.Fail("Product not found", System.Net.HttpStatusCode.NotFound);
            }

            var productAsDto = new ProductsDto(product.Id, product.Name, product.Price, product.Stock);

            //ünlem işareti null olmayacağına eminim demek
            return ServiceResult<ProductsDto>.Success(productAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };
            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult<CreateProductResponse>.Success(new CreateProductResponse(product.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            //Fast fail
            //Guard clauses
            //mümkün oldukça else yazma!..


            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult.Fail("Product not found", System.Net.HttpStatusCode.NotFound);
            }
            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult.Fail("Product not found", System.Net.HttpStatusCode.NotFound);
            }
            productRepository.Delete(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}

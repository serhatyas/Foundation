using App.Repositories;
using App.Repositories.Products;
using App.Services.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using AutoMapper;
using Azure.Core;
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
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductAsync(count);

            var productsAsDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock)).ToList();

            return new ServiceResult<List<ProductDto>>()
            {
                Data = productsAsDto,
            };
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            var products = await productRepository.GetAll().ToListAsync();

            //manuel mapping
            //var productsAsDto = products.Select(x => new ProductsDto(x.Id, x.Name, x.Price, x.Stock)).ToList();

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            // 1 - 20 => ilk 10 kayıt skip(0).Take(10
            // 2 - 10 => 11 - 20 kayıt skip(10).Take(10)

            var products = await productRepository.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //manuel mapping
            //var productsAsDto = products.Select(x => new ProductsDto(x.Id, x.Name, x.Price, x.Stock)).ToList();

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        //fail olduğunda ProductsDto boş olabilir 
        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult<ProductDto?>.Fail("Product not found", HttpStatusCode.NotFound);
            }

            //manuel mapping
            //var productAsDto = new ProductsDto(product.Id, product.Name, product.Price, product.Stock);
              var productAsDto = mapper.Map<ProductDto>(product);

            //ünlem işareti null olmayacağına eminim demek
            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {

            var anyProduct = await productRepository.Where(x => x.Name == request.Name).AnyAsync();
            if (anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail("Ürün ismi veritabanında bulunmaktadır", System.Net.HttpStatusCode.BadRequest);
            }

            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };
            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id), $"api/products/{product.Id}");
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


        //parametre ikiden fazla olunca recorda çevirmek daha mantıklı
         public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);
            if (product is null)
            {
                return ServiceResult.Fail("Product not found", System.Net.HttpStatusCode.NotFound);
            }

            product.Stock = request.Quantity;   
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

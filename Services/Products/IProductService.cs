using App.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public interface IProductService
    {
        Task<ServiceResult<List<ProductsDto>>> GetTopPriceProductAsync(int count);
        Task<ServiceResult<ProductsDto?>> GetByIdAsync(int id);
        Task<ServiceResult<List<ProductsDto>>> GetAllListAsync();
        Task<ServiceResult<List<ProductsDto>>> GetPagedAllListAsync(int pageNumber, int pageSize);
        Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request);
        Task<ServiceResult> DeleteAsync(int id);
    }
}

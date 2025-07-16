using App.Repositories;
using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Categories
{
    public class CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper):ICategoryService
    {
        public async Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProductsAsync(int categoryId)
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync(categoryId);
            if (category is null)
            {
                return ServiceResult<CategoryWithProductsDto>.Fail("Kategori bulunamadı!", HttpStatusCode.NotFound);
            }
            //var categoryAsDto = new CategoryWithProductsDto(category.Id, category.Name, category.Products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock)).ToList());
            var categoryAsDto = mapper.Map<CategoryWithProductsDto>(category);
            return ServiceResult<CategoryWithProductsDto>.Success( categoryAsDto );
        }
        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await categoryRepository.GetAll().ToListAsync();
            //var categoriesAsDto = categories.Select(x => new CategoryDto(x.Id, x.Name)).ToList();
            var categoriesAsDto = mapper.Map<List<CategoryDto>>(categories);
            return ServiceResult<List<CategoryDto>>.Success(categoriesAsDto);
        }

        public async Task<ServiceResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category is null)
            {
                return ServiceResult<CategoryDto>.Fail("Kategori bulunamadı!", HttpStatusCode.NotFound);
            }
            //var categoryAsDto = new CategoryDto(category.Id, category.Name);
            var categoryAsDto = mapper.Map<CategoryDto>(category);
            return ServiceResult<CategoryDto>.Success(categoryAsDto);
        }
        public async Task<ServiceResult<int>> Create(CreateCategoryRequest request)
        {
            var anyCategory = await categoryRepository.Where(x=>x.Name==request.Name).AnyAsync();

            if (anyCategory)
            {
                return ServiceResult<int>.Fail("Kategori ismi veritabanında bulunmaktadır!", HttpStatusCode.NotFound);

            }

            var newCategory = new Category { Name = request.Name };
            await categoryRepository.AddAsync(newCategory);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<int>.SuccessAsCreated(newCategory.Id, $"api/categories/{newCategory.Id}");
        }

        public async Task<ServiceResult> Update(UpdateCategoryRequest request)
        {
            var category = await categoryRepository.GetByIdAsync(request.Id);
            if (category is null)
            {
                return ServiceResult.Fail("Kategori bulunamadı!", HttpStatusCode.NotFound);
            }
            var anyCategory = await categoryRepository.Where(x => x.Name == request.Name && x.Id != request.Id).AnyAsync();
            if (anyCategory)
            {
                return ServiceResult.Fail("Kategori ismi veritabanında bulunmaktadır!", HttpStatusCode.BadRequest);
            }
            //category.Name = request.Name;
            category=mapper.Map(request, category);
            categoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return ServiceResult.Fail("Kategori bulunamadı!",HttpStatusCode.NotFound);
            }
            categoryRepository.Delete(category);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}

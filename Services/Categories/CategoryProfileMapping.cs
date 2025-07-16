using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Update;
using AutoMapper;

namespace App.Services.Categories
{
    public class CategoryProfileMapping : Profile
    {
        public CategoryProfileMapping()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<Category, CategoryWithProductsDto>().ReverseMap();
        }
    }
}

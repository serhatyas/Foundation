using App.Repositories.Products;
using FluentValidation;

namespace App.Services.Products.Create
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly IProductRepository _productRepository;
        public CreateProductRequestValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .Length(3, 10).WithMessage("Ürün ismi 3 ile 10 arasında olmalıdır.");
            //.Must(MustUniqueProductName).WithMessage("Ürün ismi daha önce kullanılmıştır.");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");

            RuleFor(x => x.Stock).InclusiveBetween(1, 100).WithMessage("Stok miktarı 1 ile 100 arasında olmalıdır.");
        }

        //1. way sync validation
        //private bool MustUniqueProductName(string name)
        //{

        //    return !_productRepository.Where(x => x.Name == name).Any();
        //}
    }
}

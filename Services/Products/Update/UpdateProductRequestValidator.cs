using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products.Update
{
    public class UpdateProductRequestValidator:AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .Length(3, 10).WithMessage("Ürün ismi 3 ile 10 arasında olmalıdır.");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");

            RuleFor(x => x.Stock).InclusiveBetween(1, 100).WithMessage("Stok miktarı 1 ile 100 arasında olmalıdır.");
        }
    }
}

﻿using App.Services;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => CreateActionResult(await productService.GetAllListAsync());

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize) => 
            CreateActionResult(await productService.GetPagedAllListAsync(pageNumber, pageSize));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request) => 
            CreateActionResult(await productService.CreateAsync(request));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request) =>
            CreateActionResult(await productService.UpdateAsync(id, request));

        //[HttpPatch("UpdateStock")]
        //public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request) =>
        //    CreateActionResult(await productService.UpdateStockAsync(request));

        //patch kısmi güncelleme için kullanılır
        [HttpPatch("Stock")]
        public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request) =>
            CreateActionResult(await productService.UpdateStockAsync(request));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            CreateActionResult(await productService.DeleteAsync(id));
    }
}

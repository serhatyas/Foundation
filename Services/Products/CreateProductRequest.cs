namespace App.Services.Products;

public record CreateProductRequest(int id, string Name, decimal Price, int Stock);

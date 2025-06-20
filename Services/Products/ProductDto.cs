namespace App.Services.Products
{
    //karşılaştırma yaptığım zaman propertylerine göre karşılaştırma yapacağımız için record yaptık
    //record otomatikman oluşturuyor

    public record ProductDto(int Id, string Name, decimal Price, int Stock);

    //public record ProductsDto
    //{
    //    //init kullandık çünkü controllerda veriler değiştirilmemesi için
    //    public int Id { get; init; }
    //    public string Name { get; init; } = string.Empty;
    //    public decimal Price { get; init; }
    //    public int Stock { get; init; }
    //}
}

using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task AddProductTest()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(AddProductTest));
        
        var service = new ProductService(context);
        var product = new CreateProductDto
        {
            Name = "Apple",
            Price = 50,
            Description = "This is test"
        };

        var response = await service.AddProduct(product);
        Assert.Equal("ok", response);
    }

    [Fact]
    public async Task GetProductsTask()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(GetProductsTask));
        context.products.AddRange(
            new Product{Id=1, Name="Apple", Description = "qwertyu", Price=100},
            new Product{Id=2, Name="Banana", Description = "qwertyu", Price=90},
            new Product{Id=3, Name="Cherry", Description = "qwertyu", Price=80}
        );
        await context.SaveChangesAsync();
        
        var service = new ProductService(context);
        var result = await service.GetProducts();
        var ints = new int[3]{1,2,3};
        var nums = result.Select(x=>x.Id);
        Assert.Equal(ints, nums.ToArray());

    }
}
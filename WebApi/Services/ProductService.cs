using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Services;

public class ProductService(ApplicationDbContext _dbContext) : IProductService
{
    private readonly ApplicationDbContext context = _dbContext;
    public async Task<string> AddProduct(CreateProductDto productDto)
    {
        var product = new Product()
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Description = productDto.Description
        };
        await context.products.AddAsync(product);
        await context.SaveChangesAsync();
        return "ok";
    }

    public async Task<List<ProductDto>> GetProducts()
    {
        return await context.products.Select(x=>new ProductDto(x.Id, x.Name, x.Price, x.Description)).ToListAsync();
    }

    public async Task<ProductDto> GetProduct(int id)
    {
        var product = await context.products.FindAsync(id);
        return new ProductDto(product.Id, product.Name, product.Price, product.Description);
    }

    public async Task<string> DeleteProduct(int id)
    {
        var product = await context.products.FindAsync(id);
        if (product !=null)
        {
            context.products.Remove(product);
            await context.SaveChangesAsync();
            return "Deleted";
        }
        return "Not Found";
    }

    public async Task<string> UpdateProduct(int id, CreateProductDto productDto)
    {
        var product = context.products.Find(id);
        product.Name = productDto.Name;
        product.Price = productDto.Price;
        product.Description = productDto.Description;
        await context.SaveChangesAsync();
        return "updated";
    }
}

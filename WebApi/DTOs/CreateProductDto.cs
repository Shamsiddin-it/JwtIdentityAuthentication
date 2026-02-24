using System;

namespace WebApi.DTOs;

public class CreateProductDto
{
    public string Name {get; set;}=null!;
    public decimal Price {get; set;}
    public string Description {get; set;}=null!;
    public CreateProductDto(string name, decimal price, string description)
    {
        Name = name;
        Price = price;
        Description = description;   
    }
    public CreateProductDto(){}

} 
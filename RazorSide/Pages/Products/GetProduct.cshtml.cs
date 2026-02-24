using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Interfaces;

namespace RazorSide.Pages.Products
{
    public class GetProductModel(IProductService productService) : PageModel
    {
        private readonly IProductService service = productService;
        public ProductDto productDto {get; set;}=new();
        public async Task OnGet(int id)
        {
            productDto = await service.GetProduct(id);
        }
    }
}

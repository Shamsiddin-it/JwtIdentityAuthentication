using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Interfaces;

namespace RazorSide.Pages.Products
{
    public class ProductsModel(IProductService productService) : PageModel
    {
        private readonly IProductService service = productService;
        public List<ProductDto> products = new();
        public async Task OnGet()
        {
            products = await service.GetProducts();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;

namespace Server_Side_Code_Aol_SoftEng.Controllers
{
    [Authorize(Roles = "Admin,Developer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost(Name = "Add New Product")]
        public async Task<IActionResult> OnPost([FromBody] ProductDto requestBody)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => x.Value.Errors.First().ErrorMessage)
                    .FirstOrDefault();

                return BadRequest(new
                {
                    Message = firstError ?? "Validasi gagal."
                });
            }

            await _productService.AddProductAsync(requestBody);

            return Ok(new
            {
                Message = "Produk baru berhasil ditambahkan"
            });
        }
        [HttpGet(Name = "Get all product")]
        public async Task<IActionResult> OnGet()
        {
            List<ProductDto> products = await _productService.GetAllProductsAsync();

            if (products.Count == 0) return NoContent();

            return Ok(products);
        }
    }
}

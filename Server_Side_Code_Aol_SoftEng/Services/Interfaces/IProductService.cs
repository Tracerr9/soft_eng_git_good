using Server_Side_Code_Aol_SoftEng.Models;

namespace Server_Side_Code_Aol_SoftEng.Services.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(ProductDto product);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task UpdateProductAsync(ProductDto product);
    }
}

using Microsoft.Data.SqlClient;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server_Side_Code_Aol_SoftEng.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly SqlConnection _sqlConn;
        public ProductService(ILogger<ProductService> logger, SqlConnection sqlConn)
        {
            _logger = logger;
            _sqlConn = sqlConn;
        }
        public async Task AddProductAsync(ProductDto product)
        {
            const string procedure = "Product_AddProduct";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@SKU", SqlDbType.VarChar, 255) { Value = product.SKU });
                command.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.VarChar, 255) { Value = product.ProductName });
                command.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = product.Stock });
                command.Parameters.Add(new SqlParameter("@StockThreshold", SqlDbType.Int) { Value = product.StockThreshold });
                command.Parameters.Add(new SqlParameter("@Discount", SqlDbType.Decimal) { Value = product.Discount });
                command.Parameters.Add(new SqlParameter("@BasePrice", SqlDbType.Decimal) { Value = product.BasePrice });

                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Menambah produk baru dengan SKU: {s}, Nama: {n}, dan stock: {st}", 
                    product.SKU, product.ProductName, product.Stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat menambah produk baru denganSKU: {s}, Nama: {n}, dan stock: {st}",
                    product.SKU, product.ProductName, product.Stock);
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            const string procedure = "Product_GetAll";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using var reader = await command.ExecuteReaderAsync();

                var products = new List<ProductDto>();

                while (await reader.ReadAsync())
                {
                    products.Add(new ProductDto
                    {
                        SKU = reader.GetString(reader.GetOrdinal("SKU")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                        StockThreshold = reader.GetInt32(reader.GetOrdinal("StockThreshold")),
                        Discount = reader.GetDecimal(reader.GetOrdinal("Discount")),
                        BasePrice = reader.GetDecimal(reader.GetOrdinal("BasePrice")),
                    });
                }

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat mengambil semua produk.");
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
    }
}

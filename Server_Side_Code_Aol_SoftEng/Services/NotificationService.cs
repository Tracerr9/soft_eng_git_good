using Microsoft.Data.SqlClient;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;
using System.Data;

namespace Server_Side_Code_Aol_SoftEng.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly SqlConnection _sqlConn;
        public NotificationService(ILogger<NotificationService> logger, SqlConnection sqlConn)
        {
            _logger = logger;
            _sqlConn = sqlConn;
        }
        public async Task<List<ProductNotificationDto>> GetProductNotificationAsync()
        {
            const string procedure = "Product_Notification_GetLowStock";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using var reader = await command.ExecuteReaderAsync();

                var productNotifications = new List<ProductNotificationDto>();

                while (await reader.ReadAsync())
                {
                    productNotifications.Add(new ProductNotificationDto
                    {
                        SKU = reader.GetString(reader.GetOrdinal("SKU")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                        StockThreshold = reader.GetInt32(reader.GetOrdinal("StockThreshold"))
                    });
                }

                return productNotifications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat mengambil data notifikasi produk");
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
    }
}

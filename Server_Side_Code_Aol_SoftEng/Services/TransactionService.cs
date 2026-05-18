using Microsoft.Data.SqlClient;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;
using System.Data;
using System.Diagnostics;
using System.Transactions;

namespace Server_Side_Code_Aol_SoftEng.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly SqlConnection _sqlConn;
        public TransactionService(ILogger<TransactionService> logger, SqlConnection sqlConn)
        {
            _logger = logger;
            _sqlConn = sqlConn;
        }
        public async Task ProcessAddTransactionAsync(TransactionHeaderRequestDto transaction, string username)
        {
            await _sqlConn.OpenAsync();
            using var trans = _sqlConn.BeginTransaction();
            try
            {
                int transactionId = await AddTransactionHeaderAsync(transaction, username, trans);

                foreach (var detail in transaction.Details) 
                {
                    await AddTransactionDetailAsync(detail, transactionId, trans);   
                }

                await trans.CommitAsync();
                _logger.LogInformation("Proses menambah transaksi baru dengan Id: {i} berhasil", transactionId);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                _logger.LogError(ex, "Terjadi kesalahan selama proses menambah transaksi baru.");
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
        private async Task<int> AddTransactionHeaderAsync(TransactionHeaderRequestDto transaction, string username, SqlTransaction trans)
        {
            const string procedure = "Transaction_AddTransaction_Header";
            try
            {
                using var command = new SqlCommand(procedure, _sqlConn, trans)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime) { Value = transaction.TransactionDate });
                command.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar, 255) { Value = username });

                var transactionId = Convert.ToInt32(await command.ExecuteScalarAsync());
                _logger.LogInformation("Menambah header transaksi dengan id: {i}", transactionId);

                return transactionId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan selama menambah transaksi header");
                throw;
            }
        }
        private async Task AddTransactionDetailAsync(TransactionDetailDto transactionDetail, int transactionId, SqlTransaction trans)
        {
            const string procedure = "Transaction_AddTransaction_Detail";
            try
            {
                using var command = new SqlCommand(procedure, _sqlConn, trans)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@TransactionId", SqlDbType.Int) { Value = transactionId });
                command.Parameters.Add(new SqlParameter("@ProductSKU", SqlDbType.VarChar, 255) { Value = transactionDetail.ProductSKU });
                command.Parameters.Add(new SqlParameter("@ProductAmount", SqlDbType.Int) { Value = transactionDetail.ProductAmount });

                await command.ExecuteNonQueryAsync();
                _logger.LogInformation("Menambah detail transaksi dengan id transaksi: {i}, untuk produk {sku}", transactionId, transactionDetail.ProductSKU);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat menambah detail transaksi dengan id transaksi: {i}, untuk produk {sku}", transactionId, transactionDetail.ProductSKU);
                throw;
            }
        }
        public async Task<List<TransactionResponseDto>> GetAllTransactionAsync()
        {
            const string procedure = "Transaction_GetAllTransaction";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var transactions = new List<TransactionResponseDto>();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    transactions.Add(new TransactionResponseDto
                    {
                        TransactionId = reader.GetInt32(reader.GetOrdinal("Id")),
                        Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                        TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                        Status = reader.GetString(reader.GetOrdinal("Status")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                    });
                }

                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat mengambil informasi semua transaksi");
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
        public async Task<TransactionDetailResponseDto> GetTransactionDetailAsync(int transactionId)
        {
            const string procedure = "Transaction_GetTransaction_Details";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@TransactionId", SqlDbType.Int) { Value = transactionId });

                var transactionDetail = new TransactionDetailResponseDto();

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    transactionDetail.TransactionId = reader.GetInt32(reader.GetOrdinal("Id"));
                    transactionDetail.Total = reader.GetDecimal(reader.GetOrdinal("Total"));
                    transactionDetail.TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate"));
                    transactionDetail.Status = reader.GetString(reader.GetOrdinal("Status"));
                    transactionDetail.Username = reader.GetString(reader.GetOrdinal("Username"));
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        transactionDetail.Details.Add(new TransactionItemDto
                        {
                            ProductSKU = reader.GetString(reader.GetOrdinal("ProductSKU")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            ProductAmount = reader.GetInt32(reader.GetOrdinal("ProductAmount")),
                            DiscountAmount = reader.GetDecimal(reader.GetOrdinal("DiscountAmount")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName"))
                        });
                    }
                }

                return transactionDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat mengambil informasi transaksi dengan Id: {i}", transactionId);
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
        public async Task VoidTransactionAsync(int transactionId)
        {
            const string procedure = "Transaction_VoidTransaction";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@TransactionId", SqlDbType.Int) { Value = transactionId });

                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Membatalkan transaksi dengan id: {I}", transactionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat membatalkan transaksi dengan id: {i}", transactionId);
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
    }
}

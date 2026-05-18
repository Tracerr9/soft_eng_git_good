using Microsoft.Data.SqlClient;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;
using System.Data;

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
        public async Task ProcessAddTransactionAsync(TransactionHeaderCreateDto transaction, string username)
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
        private async Task<int> AddTransactionHeaderAsync(TransactionHeaderCreateDto transaction, string username, SqlTransaction trans)
        {
            const string procedure = "Transaction_AddTransaction_Header";
            try
            {
                using var command = new SqlCommand(procedure, _sqlConn, trans)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@Total", SqlDbType.Decimal) { Value = transaction.Total });
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
    }
}

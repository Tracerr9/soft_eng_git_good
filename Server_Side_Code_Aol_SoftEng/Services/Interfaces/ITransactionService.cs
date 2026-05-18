using Server_Side_Code_Aol_SoftEng.Models;

namespace Server_Side_Code_Aol_SoftEng.Services.Interfaces
{
    public interface ITransactionService
    {
        Task ProcessAddTransactionAsync(TransactionHeaderRequestDto transaction, string username);
        Task<List<TransactionResponseDto>> GetAllTransactionAsync();
        Task<TransactionDetailResponseDto> GetTransactionDetailAsync(int transactionId);
        Task VoidTransactionAsync(int transactionId);
    }
}

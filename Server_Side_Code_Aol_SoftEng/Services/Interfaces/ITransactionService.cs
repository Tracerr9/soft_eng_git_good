using Server_Side_Code_Aol_SoftEng.Models;

namespace Server_Side_Code_Aol_SoftEng.Services.Interfaces
{
    public interface ITransactionService
    {
        Task ProcessAddTransactionAsync(TransactionHeaderCreateDto transaction, string username);

    }
}

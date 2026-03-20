using AFCS.API.Common;
using AFCS.API.DTOs.Transaction;

namespace AFCS.API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Result<List<TransactionDTO>>> GetRecentTransactions(int limit = 20);

        Task<Result<TransactionDTO>> CreateTransaction(CreateTransactionRequestDTO request);
    }
}

using AFCS.API.DTOs.Transaction;

namespace AFCS.API.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<TransactionDTO>> GetRecentTransactions(int limit = 20);
        Task<TransactionDTO> CreateTransaction(CreateTransactionRequestDTO request);

    }
}

using AFCS.API.Common;
using AFCS.API.DTOs.Transaction;
using AFCS.API.Repositories.Interfaces;
using AFCS.API.Services.Interfaces;

namespace AFCS.API.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;

        public TransactionService(ITransactionRepository _transactionRepository)
        {
            transactionRepository = _transactionRepository;
        }

        public async Task<Result<List<TransactionDTO>>> GetRecentTransactions(int limit = 20)
        {
            var transactions = await transactionRepository.GetRecentTransactions(limit);
            
            return Result<List<TransactionDTO>>.Ok(transactions);
        }

        public async Task<Result<TransactionDTO>> CreateTransaction(CreateTransactionRequestDTO request)
        {
            var transaction = await transactionRepository.CreateTransaction(request);

            return Result<TransactionDTO>.Ok(transaction);
        }
    }
}

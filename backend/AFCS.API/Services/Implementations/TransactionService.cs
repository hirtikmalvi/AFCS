using AFCS.API.Common;
using AFCS.API.DTOs.Transaction;
using AFCS.API.Hubs;
using AFCS.API.Repositories.Interfaces;
using AFCS.API.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AFCS.API.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IHubContext<FareHub> hub;

        public TransactionService(ITransactionRepository _transactionRepository, IHubContext<FareHub> _hub)
        {
            transactionRepository = _transactionRepository;
            hub = _hub;
        }

        public async Task<Result<List<TransactionDTO>>> GetRecentTransactions(int limit = 20)
        {
            var transactions = await transactionRepository.GetRecentTransactions(limit);
            
            return Result<List<TransactionDTO>>.Ok(transactions);
        }

        public async Task<Result<TransactionDTO>> CreateTransaction(CreateTransactionRequestDTO request)
        {
            var transaction = await transactionRepository.CreateTransaction(request);

            // Broadcast to ALL connected Angular dashboards
            await hub.Clients.All.SendAsync(FareHub.NewTransaction, transaction);
            

            return Result<TransactionDTO>.Ok(transaction);
        }
    }
}

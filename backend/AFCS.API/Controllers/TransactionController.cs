using AFCS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFCS.API.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService transactionService;
        public TransactionController(ITransactionService _transactionService)
        {
            transactionService = _transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecentTransactions([FromQuery] int limit = 20)
        {
            if (limit < 0)
            {
                limit = 20;
            }
            if (limit > 150)
            {
                limit = 150;
            }

            var result = await transactionService.GetRecentTransactions(limit);
            return Ok(result);
        }
    }
}

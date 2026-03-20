using AFCS.API.DTOs.Stats;
using AFCS.API.Repositories.Interfaces;
using Npgsql;

namespace AFCS.API.Repositories.Implementations
{
    public class StatsRepository : IStatsRepository
    {
        private readonly string dbConnString;
        private readonly IConfiguration configuration;

        public StatsRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            dbConnString = configuration.GetConnectionString("DbConnectionString")!;
        }
        public async Task<StatsDTO> GetSummary()
        {
            await using var conn = new NpgsqlConnection(dbConnString);
            await conn.OpenAsync();

            var statsDTO = new StatsDTO();

            // For Summary TotalTransactions, TotalRevenue, AverageFare
            var summarySQL = @"
                SELECT 
	                COUNT(*) AS total_transaction,
	                COALESCE(SUM(fare_amount), 0)  AS total_revenue,
	                COALESCE(AVG(fare_amount), 0) AS average_fare
                FROM Transactions
                WHERE 
                    DATE(transaction_time) = CURRENT_DATE";

            await using (var cmdSummarySQL = new NpgsqlCommand(summarySQL, conn))
            {
                await using var readerSummary = await cmdSummarySQL.ExecuteReaderAsync();

                if (await readerSummary.ReadAsync())
                {
                    statsDTO.TotalTransactions = readerSummary.GetInt32(readerSummary.GetOrdinal("total_transaction"));
                    statsDTO.TotalRevenue = Math.Round(readerSummary.GetDecimal(readerSummary.GetOrdinal("total_revenue")), 2);
                    statsDTO.AverageFare = Math.Round(readerSummary.GetDecimal(readerSummary.GetOrdinal("average_fare")), 2);
                }
            }

            // For Status of Gates
            var gateSQL = @"
                SELECT 
	                (SELECT COUNT(id) FROM gates WHERE status = 'open') AS open_gate,
	                (SELECT COUNT(id) FROM gates WHERE status = 'closed') AS close_gate,
	                (SELECT COUNT(id) FROM gates WHERE status = 'fault') AS fault_gate;";

            await using (var cmdGateSQL = new NpgsqlCommand(gateSQL, conn))
            {
                await using var readerGate = await cmdGateSQL.ExecuteReaderAsync();

                if (await readerGate.ReadAsync())
                {
                    statsDTO.ActiveGates = readerGate.GetInt32(readerGate.GetOrdinal("open_gate"));
                    statsDTO.ClosedGates = readerGate.GetInt32(readerGate.GetOrdinal("close_gate"));
                    statsDTO.FaultGates = readerGate.GetInt32(readerGate.GetOrdinal("fault_gate"));
                }
            }

            // For Hourly Revenue
            var hourlyRevenueSQL = "SELECT * FROM get_hourly_revenue()";

            await using (var cmdHourlyRevenue = new NpgsqlCommand(hourlyRevenueSQL, conn))
            {
                await using var readerHourlyRevenue = await cmdHourlyRevenue.ExecuteReaderAsync();

                var hourlyRevenueList = new List<HourlyRevenueDTO>();

                while (await readerHourlyRevenue.ReadAsync())
                {
                    var hourlyRevenue = new HourlyRevenueDTO
                    {
                        Hour = readerHourlyRevenue.GetInt32(readerHourlyRevenue.GetOrdinal("hr")),
                        Revenue = Math.Round(readerHourlyRevenue.GetDecimal(readerHourlyRevenue.GetOrdinal("revenue")), 2)
                    };
                    hourlyRevenueList.Add(hourlyRevenue);
                }
                statsDTO.HourlyRevenue = hourlyRevenueList;
            }

            // For PaymentBreakdown
            var paymentBreakdownSQL = @"
                                        SELECT 
	                                        payment_type, COUNT(id) AS count
                                        FROM 
	                                        transactions
                                        WHERE 
	                                        DATE(transaction_time) = CURRENT_DATE
                                        GROUP BY payment_type";

            await using (var cmdPaymentBd = new NpgsqlCommand(paymentBreakdownSQL, conn))
            {
                await using var readerPaymentBd = await cmdPaymentBd.ExecuteReaderAsync();

                var paymentBdList = new List<PaymentBreakdownDTO>();

                while (await readerPaymentBd.ReadAsync())
                {
                    var paymentBd = new PaymentBreakdownDTO
                    {
                        PaymentType = readerPaymentBd.GetString(readerPaymentBd.GetOrdinal("payment_type")),
                        Count = readerPaymentBd.GetInt32(readerPaymentBd.GetOrdinal("count"))
                    };
                    paymentBdList.Add(paymentBd);
                }

                statsDTO.PaymentBreakdown = paymentBdList;
            }

            return statsDTO;
        }
    }
}

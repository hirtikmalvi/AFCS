namespace AFCS.API.DTOs.Stats
{
    public class StatsDTO
    {
        public int TotalTransactions { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageFare { get; set; }
        public int ActiveGates  { get; set; }
        public int ClosedGates { get; set; }
        public int FaultGates { get; set; }
        public List<HourlyRevenueDTO> HourlyRevenue { get; set; } = [];
        public List<PaymentBreakdownDTO> PaymentBreakdown { get; set; } = [];

    }
}

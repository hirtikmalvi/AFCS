namespace AFCS.API.DTOs.Gate
{
    public class GateDTO
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; } = "";
        public string GateNumber { get; set; } = "";
        public string Status { get; set; } = "";
    }
}

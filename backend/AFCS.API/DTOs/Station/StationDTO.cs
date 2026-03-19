namespace AFCS.API.DTOs.Station
{
    public class StationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public bool IsActive { get; set; }
    }
}

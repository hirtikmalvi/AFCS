using System.ComponentModel.DataAnnotations;

namespace AFCS.API.DTOs.Gate
{
    public class UpdateGateStatusRequest
    {
        [Required(ErrorMessage = "GateId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Valid GateId is required.")]
        public int? GateId { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        [AllowedValues(["open", "closed", "fault"], ErrorMessage = "Status ust be: open, closed or fault.")]
        public string? Status { get; set; }
    }
}

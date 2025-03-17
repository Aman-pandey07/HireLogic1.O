using System.ComponentModel.DataAnnotations;

namespace JobPortal1.O.DTOs
{
    public class UpdateStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}

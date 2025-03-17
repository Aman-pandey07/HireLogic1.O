using System.ComponentModel.DataAnnotations;

namespace JobPortal1.O.DTOs.ApplicationDtos;

public class ApplicationDTO
{
    [Required]
    public int JobId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public string ResumeUrl { get; set; } = string.Empty;

    public string? Status { get; set; }
}


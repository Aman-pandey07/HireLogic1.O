using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobPortal1.O.Models;

public class Application
{
    [Key]
    public int Id { get; set; }

    // Foreign Key linking to Job table
    [Required]
    public int JobId { get; set; }

    [ForeignKey("JobId")]
    public Job? Job { get; set; }

    // Foreign Key linking to User table (Job Seeker)
    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Required]
    public string ResumeUrl { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected
}

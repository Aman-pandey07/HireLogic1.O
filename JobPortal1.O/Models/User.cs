using System.ComponentModel.DataAnnotations;

namespace JobPortal1.O.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = "JobSeeker"; // JobSeeker or Employer

    // Navigation Properties
    public ICollection<Job>? Jobs { get; set; }
    public ICollection<Application>? Applications { get; set; }
}

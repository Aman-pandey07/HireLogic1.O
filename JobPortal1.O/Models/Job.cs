using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobPortal1.O.Models;

public class Job
{

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Salary { get; set; }

    // Foreign Key linking to User table (Employer)
    [Required]
    public int EmployerId { get; set; }

    [ForeignKey("EmployerId")]
    public User? Employer { get; set; }

    // Navigation Property
    public ICollection<Application>? Applications { get; set; }

}

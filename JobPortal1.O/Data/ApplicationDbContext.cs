using JobPortal1.O.Models;
using Microsoft.EntityFrameworkCore;



public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Job> Jobs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Application> Applications { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 🔥 User & Job Relationship (1 to Many)
        modelBuilder.Entity<User>()
                .HasMany(u => u.Jobs) // One User → Many Jobs
                .WithOne(j => j.Employer) // One Job → One Employer
                .HasForeignKey(j => j.EmployerId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if referenced

        // 🔥 User & Application Relationship (1 to Many)
        modelBuilder.Entity<User>()
            .HasMany(u=> u.Applications) // one user -> Many Applications
            .WithOne(a=> a.User) //one application -> one user
            .HasForeignKey(a=> a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔥 Job & Application Relationship (1 to Many)
        modelBuilder.Entity<Job>()
            .HasMany(j => j.Applications) // One Job → Many Applications
            .WithOne(a => a.Job) // One Application → One Job
            .HasForeignKey(a => a.JobId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete when Job deleted


        base.OnModelCreating(modelBuilder);
    }
}

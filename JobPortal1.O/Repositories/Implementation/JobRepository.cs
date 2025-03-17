using JobPortal1.O.Models;
using JobPortal1.O.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace JobPortal1.O.Repositories.Implementation;

public class JobRepository : IJobRepository
{
    private readonly ApplicationDbContext _context;

    public JobRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Job>> GetAllJobsAsync()
    {
        return await _context.Jobs.Include(j => j.Employer).ToListAsync();
    }

    public async Task<Job?> GetJobByIdAsync(int id)
    {
        return await _context.Jobs.Include(j => j.Employer)
                                  .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<Job> CreateJobAsync(Job job)
    {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public async Task<Job?> UpdateJobAsync(Job job)
    {
        var existingJob = await _context.Jobs.FindAsync(job.Id);
        if (existingJob == null) return null;

        existingJob.Title = job.Title;
        existingJob.Description = job.Description;
        existingJob.Salary = job.Salary;

        _context.Jobs.Update(existingJob);
        await _context.SaveChangesAsync();

        return existingJob;
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return false;

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();

        return true;
    }
}

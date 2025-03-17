using JobPortal1.O.DTOs.ApplicationDtos;
using JobPortal1.O.Models;
using JobPortal1.O.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace JobPortal1.O.Repositories.Implementation;

public class ApplicationRepository : IApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public ApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ 1. Apply for a Job
    public async Task<bool> ApplyForJobAsync(ApplicationDTO dto)
    {
        var job = await _context.Jobs.FindAsync(dto.JobId);
        if (job == null) return false; // Job not found

        var application = new Application
        {
            JobId = dto.JobId,
            UserId = dto.UserId,
            ResumeUrl = dto.ResumeUrl,
            Status = dto.Status ?? "Pending"
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return true; // Application successful
    }

    public async Task<List<Application>> GetAllApplicationsAsync()
    {
        return await _context.Applications
                             .Include(a => a.Job)
                             .Include(a => a.User)
                             .ToListAsync();
    }

    public async Task<Application?> GetApplicationByIdAsync(int id)
    {
        return await _context.Applications
                             .Include(a => a.Job)
                             .Include(a => a.User)
                             .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Application>> GetApplicationsByJobIdAsync(int jobId)
    {
        return await _context.Applications
                             .Where(a => a.JobId == jobId)
                             .Include(a => a.User)
                             .ToListAsync();
    }

    public async Task<List<Application>> GetApplicationsByUserIdAsync(int userId)
    {
        return await _context.Applications
                             .Where(a => a.UserId == userId)
                             .Include(a => a.Job)
                             .ToListAsync();
    }

    public async Task<Application> CreateApplicationAsync(Application application)
    {
        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return application;
    }

    public async Task<Application?> UpdateApplicationStatusAsync(int id, string status)
    {
        var application = await _context.Applications.FindAsync(id);
        if (application == null) return null;

        application.Status = status;
        await _context.SaveChangesAsync();

        return application;
    }

    public async Task<bool> DeleteApplicationAsync(int id)
    {
        var application = await _context.Applications.FindAsync(id);
        if (application == null) return false;

        _context.Applications.Remove(application);
        await _context.SaveChangesAsync();

        return true;
    }
}

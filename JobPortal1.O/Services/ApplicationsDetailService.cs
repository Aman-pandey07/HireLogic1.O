using JobPortal1.O.DTOs.ApplicationDtos;
using JobPortal1.O.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace JobPortal1.O.Services
{
    public class ApplicationsDetailService
    {
        private readonly ApplicationDbContext _context;
        public ApplicationsDetailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationListDTO>> GetApplicationsAsync()
        {
            var applications = await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.User)
                .ToListAsync();

            var result = applications
                .Where(a => a.Job != null && a.User != null)
                .Select(a => new ApplicationListDTO
                {
                    Id = a.Id,
                    JobId = a.Job.Id,
                    JobTitle = a.Job.Title,
                    JobDescription = a.Job.Description,
                    Salary = a.Job.Salary,
                    UserId = a.User.Id,
                    UserName = a.User.Name,
                    ResumeUrl = a.ResumeUrl,
                    Status = a.Status
                }).ToList();

            return result;

        }

        public async Task<ApplicationListDTO?> GetApplicationsAsyncById(int id)
        {
            var applications = await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);

            if (applications == null || applications.Job == null || applications.User == null)
                return null;


            var result = new ApplicationListDTO
            {
                Id = applications.Id,
                JobId = applications.Job.Id,
                JobTitle = applications.Job.Title,
                JobDescription = applications.Job.Description,
                Salary = applications.Job.Salary,
                UserId = applications.User.Id,
                UserName = applications.User.Name,
                ResumeUrl = applications.ResumeUrl,
                Status = applications.Status
            };

            return result;

        }
    }
}

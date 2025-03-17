using JobPortal1.O.DTOs.ApplicationDtos;
using JobPortal1.O.Models;

namespace JobPortal1.O.Repositories.Interface
{
    public interface IApplicationRepository
    {
        Task<List<Application>> GetAllApplicationsAsync();
        Task<Application?> GetApplicationByIdAsync(int id);
        Task<List<Application>> GetApplicationsByJobIdAsync(int jobId);
        Task<List<Application>> GetApplicationsByUserIdAsync(int userId);
        Task<Application> CreateApplicationAsync(Application application);
        Task<Application?> UpdateApplicationStatusAsync(int id, string status);
        Task<bool> DeleteApplicationAsync(int id);
        
        // ✅ New Method - Apply for a Job
        Task<bool> ApplyForJobAsync(ApplicationDTO dto);
    }
}

using JobPortal1.O.Models;

namespace JobPortal1.O.Repositories.Interface
{
    public interface IJobRepository
    {
        Task<List<Job>> GetAllJobsAsync();
        Task<Job?> GetJobByIdAsync(int id);
        Task<Job> CreateJobAsync(Job job);
        Task<Job?> UpdateJobAsync(Job job);
        Task<bool> DeleteJobAsync(int id);
    }
}

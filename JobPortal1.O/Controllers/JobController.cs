using JobPortal1.O.DTOs.Common;
using JobPortal1.O.Models;
using JobPortal1.O.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal1.O.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;

        public JobController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        // ✅ 1. Get All Jobs
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _jobRepository.GetAllJobsAsync();

            if (!jobs.Any())
                return NotFound(new ApiResponse<string>(false, "No jobs available", null));

            return Ok(new ApiResponse<List<Job>>(true, "Jobs fetched successfully", jobs));
        }

        // ✅ 2. Get Job by ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);

            if (job == null)
                return NotFound(new ApiResponse<string>(false, "Job not found", null));

            return Ok(new ApiResponse<Job>(true, "Job fetched successfully", job));
        }

        // ✅ 3. Create Job
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateJob(Job job)
        {
            if (job == null)
                return BadRequest(new ApiResponse<string>(false, "Invalid job data", null));

            await _jobRepository.CreateJobAsync(job);
            return CreatedAtAction(nameof(GetJobById), new { id = job.Id },
                new ApiResponse<Job>(true, "Job created successfully", job));
        }

        // ✅ 4. Update Job
        [HttpPut("{id}")]
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> UpdateJob(int id, Job updatedJob)
        {
            if (id != updatedJob.Id)
                return BadRequest(new ApiResponse<string>(false, "Job ID mismatch", null));

            var existingJob = await _jobRepository.GetJobByIdAsync(id);
            if (existingJob == null)
                return NotFound(new ApiResponse<string>(false, "Job not found", null));

            existingJob.Title = updatedJob.Title;
            existingJob.Description = updatedJob.Description;
            existingJob.Salary = updatedJob.Salary;
            existingJob.EmployerId = updatedJob.EmployerId;

            var result = await _jobRepository.UpdateJobAsync(existingJob);
            if (result == null)
                return NotFound(new ApiResponse<string>(false, "Job not found", null));

            return Ok(new ApiResponse<Job>(true, "Job updated successfully", updatedJob));
        }

        // ✅ 5. Delete Job
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var result = await _jobRepository.DeleteJobAsync(id);
            if (!result)
                return NotFound(new ApiResponse<string>(false, "Job not found", null));

            return Ok(new ApiResponse<string>(true, "Job deleted successfully", null));
        }
    }
}

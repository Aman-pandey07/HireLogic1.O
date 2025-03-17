using JobPortal1.O.Models;
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
        private readonly ApplicationDbContext _context;

        public JobController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _context.Jobs.Include(j => j.Employer).ToListAsync();
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _context.Jobs
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(j => j.Id == id);
            return Ok(job);
        }
        
        
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateJob(Job job)
        {
            if (job == null)
                return BadRequest("Invalid job data");

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJobById), new { id = job.Id }, job);
        }

        // ✅ Update Job
        [HttpPut("{id}")]
        [Authorize(Roles = "Employer")] // 🔥 Only Employers can update jobs
        public async Task<IActionResult> UpdateJob(int id, Job updatedJob)
        {
            if (id != updatedJob.Id)
                return BadRequest("Job ID mismatch");

            var existingJob = await _context.Jobs.FindAsync(id);
            if (existingJob == null)
                return NotFound("Job not found");

            existingJob.Title = updatedJob.Title;
            existingJob.Description = updatedJob.Description;
            existingJob.Salary = updatedJob.Salary;

            _context.Jobs.Update(existingJob);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Delete Job
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employer")] // 🔥 Only Employers can delete jobs
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound("Job not found");

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }





    }
}

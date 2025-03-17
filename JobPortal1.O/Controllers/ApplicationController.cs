using JobPortal1.O.DTOs;
using JobPortal1.O.DTOs.ApplicationDtos;
using JobPortal1.O.DTOs.Common;
using JobPortal1.O.Models;
using JobPortal1.O.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal1.O.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationsDetailService _applicationsDetailService;

        public ApplicationController(ApplicationDbContext context,ApplicationsDetailService applicationsDetailService)
        {
            _context = context;
            _applicationsDetailService = applicationsDetailService;
        }

        

        // ✅ 1. Apply for a Job
        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> ApplyForJob([FromBody] ApplicationDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<string>(false, "Invalid input data", null));

            var job = await _context.Jobs.FindAsync(dto.JobId);
            if (job == null) return NotFound(new ApiResponse<string>(false, "Job not found", null));

            var application = new Application
            {
                JobId = dto.JobId,
                UserId = dto.UserId,
                ResumeUrl = dto.ResumeUrl,
                Status = dto.Status ?? "Pending"
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string>(true, "Job Application Submitted Successfully", null));
        }

        // ✅ 2. Get All Applications
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ApplicationListDTO>>> GetApplications()
        {
            var applications = await _applicationsDetailService.GetApplicationsAsync();

            if (applications == null || !applications.Any())
            {
                return NotFound(new ApiResponse<string>(false, "No Data Found" , null)); // 204 if no data found
            }

            return Ok(new ApiResponse<List<ApplicationListDTO>>(true, "Applications fetched successfully", applications));
        }

        // ✅ 3. Get Application by ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetApplication(int id)
        {
            var application = await _applicationsDetailService.GetApplicationsAsyncById(id);

            if (application == null) return
                    NotFound(new ApiResponse<string>(false, "Application not found", null));
            return Ok(new ApiResponse<ApplicationListDTO>(true, "Application fetched successfully", application));
        }

        // ✅ 4. Get Applications by User ID
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> GetApplicationsByUserId(int userId)
        {
            var applications = await _context.Applications
                                             .Where(a => a.UserId == userId)
                                             .Include(a => a.Job)
                                             .ToListAsync();

            return Ok(applications);
        }

        // ✅ 5. Get Applications by Job ID
        [HttpGet("job/{jobId}")]
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> GetApplicationsByJobId(int jobId)
        {
            var applications = await _context.Applications
                                             .Where(a => a.JobId == jobId)
                                             .Include(a => a.User)
                                             .ToListAsync();

            return Ok(applications);
        }

        // ✅ 6. Update Application Status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null) return NotFound(new ApiResponse<String>(false, "Application not Found", null));  //"Application not found"

            application.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string>(true, "Status updated successfully", null));
        }
            
        

        // ✅ 7. Delete Application
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null) return NotFound(new ApiResponse<String>(false, "Application not Found", null));

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

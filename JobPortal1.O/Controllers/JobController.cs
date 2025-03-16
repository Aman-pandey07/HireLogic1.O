using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal1.O.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllJobs()
        {
            return Ok("Jobs fetched successfully");
        }
    }
}

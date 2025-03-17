using JobPortal1.O.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal1.O.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        //this is tee code to get all user only admin access
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        //this is the code to get all user by id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User Not Found");

            return Ok(user);
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id,User updatedUser)
        {
            if (id != updatedUser.Id) return BadRequest("The User Id Mismatch");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found");

            // Only allow user to update their own data
            var loggedInUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value ?? "0");
            if (loggedInUserId != id)
                return Forbid("You can only update your own profile");

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.PasswordHash = updatedUser.PasswordHash;
            user.Role = updatedUser.Role;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // 204 - No Content
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

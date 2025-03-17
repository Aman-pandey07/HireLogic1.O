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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // ✅ 1. Get All Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();

            if (!users.Any())
                return NotFound(new ApiResponse<string>(false, "No users found", null));

            return Ok(new ApiResponse<List<User>>(true, "Users fetched successfully", users));
        }

        // ✅ 2. Get User by ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new ApiResponse<string>(false, "User not found", null));

            return Ok(new ApiResponse<User>(true, "User fetched successfully", user));
        }

        // ✅ 3. Update User
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest(new ApiResponse<string>(false, "User ID mismatch", null));

            var result = await _userRepository.UpdateUserAsync(updatedUser);
            if (result == null)
                return NotFound(new ApiResponse<string>(false, "User not found", null));

            return Ok(new ApiResponse<User>(true, "User updated successfully", updatedUser));
        }

        // ✅ 4. Delete User
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteUserAsync(id);
            if (!result)
                return NotFound(new ApiResponse<string>(false, "User not found", null));

            return Ok(new ApiResponse<string>(true, "User deleted successfully", null));
        }
    }
}
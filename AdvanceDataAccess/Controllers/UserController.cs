using AdvanceDataAccess.IRepository;
using AdvanceDataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdvanceDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] Users user)
        {
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertUsers([FromBody] List<Users> users)
        {
            await _userRepository.BulkInsertUsersAsync(users);
            return Ok("Bulk Insert Successful");
        }
    }
}
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PakaUsers.Model;

namespace PakaUsers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IUserRepository _userRepository;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userRepository.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUser(string id)
        {
            var user = _userRepository.Get(id);
            return Ok(user);
        }
        
        [HttpDelete]
        public IActionResult DeleteUser(User user)
        {
            _userRepository.Delete(user);
            _userRepository.Save();
            return Ok(user);
        }

        [HttpPut]
        public IActionResult UpdateUser(User user)
        {
            _userRepository.Update(user);
            _userRepository.Save();
            return Ok(user);
        }
    }
}
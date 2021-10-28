using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakaUsers.Model;

namespace PakaUsers.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userRepository.GetAll());
        }

        [HttpGet("{id:Guid}")]
        public IActionResult GetUser(Guid id)
        {
            var user = _userRepository.Get(id.ToString());
            return Ok(user);
        }
        
        [HttpDelete]
        public IActionResult DeleteUser(User user)
        {
            _userRepository.Delete(user);
            _userRepository.Save();
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateUser(User user)
        {
            _userRepository.Update(user);
            _userRepository.Save();
            return Ok(user);
        }
        
        [HttpGet]
        [Route("me")]
        public IActionResult Me()
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == "Id").Value;
            return Ok(_userRepository.Get(userId));
        }
    }
}
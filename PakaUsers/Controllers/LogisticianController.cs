using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PakaUsers.Dto.Requests;
using PakaUsers.Dto.Responses;
using PakaUsers.IdentityAuth;
using PakaUsers.Model;

namespace PakaUsers.Controllers
{
    [Route("api/logistician")]
    [ApiController]
    public class LogisticianController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly StatusCodeHelper _statusCodeHelper = new();

        public LogisticianController(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterLogistician([FromBody] RegisterWorkerDto request)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == "Id").Value;
            var currentUser = _userRepository.Get(userId);

            if (currentUser is not Admin)
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            var userEmailExists = await _userManager.FindByEmailAsync(request.Email);
            if (userEmailExists != null)
                return BadRequest(new Response {Message = "Email already exists!"});

            var userNameExists = await _userManager.FindByNameAsync(request.UserName);
            if (userNameExists != null)
                return BadRequest(new Response {Message = "UserName already exists!"});

            var logistician = new Logistician()
            {
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
                Name = request.Name,
                Surname = request.Surname,
                Salary = request.Salary,
                WarehouseId = request?.WarehouseId,
                WarehouseType = request.WarehouseType,
                IsActive = false
            };

            var result = await _userManager.CreateAsync(logistician, request.Password);
            if (!result.Succeeded)
                return BadRequest(new Response {Message = result.ToString()});

            return Ok(UserResponseDto.Of(logistician));
        }
    }
}
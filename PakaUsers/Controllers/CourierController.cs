using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PakaUsers.Dto.Requests;
using PakaUsers.Dto.Responses;
using PakaUsers.IdentityAuth;
using PakaUsers.Model;
using PakaUsers.Services;

namespace PakaUsers.Controllers
{
    [Route("api/courier")]
    [ApiController]
    public class CourierController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly StatusCodeHelper _statusCodeHelper = new();

        public CourierController(UserManager<User> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCourier([FromBody] RegisterWorkerDto request)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }
            var userEmailExists = await _userManager.FindByEmailAsync(request.Email);
            if (userEmailExists != null)
                return BadRequest(new Response {Message = "Email already exists!"});

            var userNameExists = await _userManager.FindByNameAsync(request.UserName);
            if (userNameExists != null)
                return BadRequest(new Response {Message = "UserName already exists!"});

            var courier = new Courier()
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

            var result = await _userManager.CreateAsync(courier, request.Password);
            if (!result.Succeeded)
                return BadRequest(new Response {Message = result.ToString()});

            return Ok(UserResponseDto.Of(courier));
        }
    }
}
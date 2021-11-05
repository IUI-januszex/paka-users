using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PakaUsers.Dto.Requests;
using PakaUsers.Dto.Responses;
using PakaUsers.IdentityAuth;
using PakaUsers.Model;

namespace PakaUsers.Controllers
//pl.com.januszex.paka.users.controllers
{
    [Route("api/business-client")]
    [ApiController]
    public class BusinessClientController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public BusinessClientController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterBusinessClient([FromBody] RegisterBusinessClientDto request)
        {
            var userEmailExists = await _userManager.FindByEmailAsync(request.Email);
            if (userEmailExists != null)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response {Status = "Error", Message = "Email already exists!"});

            var userNameExists = await _userManager.FindByNameAsync(request.UserName);
            if (userNameExists != null)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response {Status = "Error", Message = "UserName already exists!"});

            var business = new BusinessClient()
            {
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
                CompanyName = request.CompanyName,
                Nip = request.Nip,
                IsActive = false
            };

            var result = await _userManager.CreateAsync(business, request.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response {Status = "Error", Message = result.ToString()});

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response {Status = "Error", Message = result.ToString()});

            return Ok(UserResponseDto.Of(business));
        }
    }
}
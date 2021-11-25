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
{
    [Route("api/individual-client")]
    [ApiController]
    public class IndividualClientController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public IndividualClientController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterIndividualClient([FromBody] RegisterIndividualClientDto request)
        {
            var userEmailExists = await _userManager.FindByEmailAsync(request.Email);
            if (userEmailExists != null)
                return BadRequest(new Response {Message = "Email already exists!"});

            var userNameExists = await _userManager.FindByNameAsync(request.UserName);
            if (userNameExists != null)
                return BadRequest(new Response {Message = "UserName already exists!"});

            var client = new Client()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
                IsActive = true,
            };

            var result = await _userManager.CreateAsync(client, request.Password);

            if (!result.Succeeded)
                return BadRequest(new Response {Message = result.ToString()});

            return Ok(UserResponseDto.Of(client));
        }
    }
}
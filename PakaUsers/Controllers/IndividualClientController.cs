using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PakaUsers.Dto.Requests;
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
        public async Task<IActionResult> RegisterIndividualClient([FromBody] RegisterIndividualClientDto model)
        {
            var userEmailExists = await _userManager.FindByEmailAsync(model.Email);
            if (userEmailExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = "Email already exists!"});

            var userNameExists = await _userManager.FindByNameAsync(model.UserName);
            if (userNameExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = "UserName already exists!"});

            var client = new Client()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.Name,
                Surname = model.Surname,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(client);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = result.ToString()});

            return Ok(new Response {Status = "Success", Message = "Individual client created successfully!"});
        }
    }
}
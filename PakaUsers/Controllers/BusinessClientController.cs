using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PakaUsers.Dto.Requests;
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
        public async Task<IActionResult> RegisterBusinessClient([FromBody] RegisterBusinessClientDto model)
        {
            var userEmailExists = await _userManager.FindByEmailAsync(model.Email);
            if (userEmailExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = "Email already exists!"});

            var userNameExists = await _userManager.FindByNameAsync(model.UserName);
            if (userNameExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = "UserName already exists!"});

            var business = new BusinessClient()
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                CompanyName = model.CompanyName,
                Nip = model.Nip,
                IsActive = false
            };

            var result = await _userManager.CreateAsync(business, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = result.ToString()});

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = result.ToString()});

            return Ok(new Response {Status = "Success", Message = "Business client created successfully!"});
        }
    }
}
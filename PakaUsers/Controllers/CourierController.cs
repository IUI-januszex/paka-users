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
    [Route("api/courier")]
    [ApiController]
    public class CourierController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public CourierController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCourier([FromBody] RegisterWorkerDto model)
        {
            var userEmailExists = await _userManager.FindByEmailAsync(model.Email);
            if (userEmailExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = "Email already exists!"});

            var userNameExists = await _userManager.FindByNameAsync(model.UserName);
            if (userNameExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = "UserName already exists!"});

            var courier = new Courier()
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                Name = model.Name,
                Surname = model.Surname,
                Salary = model.Salary,
                WarehouseId = model.Warehouse,
                IsActive = false
            };

            var result = await _userManager.CreateAsync(courier, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = result.ToString()});

            return Ok(new Response {Status = "Success", Message = "Courier created successfully!"});
        }
    }
}
﻿using System;
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

        public LogisticianController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterLogistician([FromBody] RegisterWorkerDto request)
        {
            var userEmailExists = await _userManager.FindByEmailAsync(request.Email);
            if (userEmailExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {Status = "Error", Message = "Email already exists!"});

            var userNameExists = await _userManager.FindByNameAsync(request.UserName);
            if (userNameExists != null)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response {Status = "Error", Message = "UserName already exists!"});

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
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response {Status = "Error", Message = result.ToString()});

            return Ok(UserResponseDto.Of(logistician));
        }
    }
}
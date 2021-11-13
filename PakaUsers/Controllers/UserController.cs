using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakaUsers.Dto.Requests;
using PakaUsers.Dto.Responses;
using PakaUsers.IdentityAuth;
using PakaUsers.Model;

namespace PakaUsers.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAddressRepository _addressRepository;

        public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IAddressRepository addressRepository)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _addressRepository = addressRepository;
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
            return Ok(UserResponseDto.Of(user));
        }

        [HttpGet("{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userRepository.GetAll().FirstOrDefault(user => user.Email == email);
            return Ok(UserResponseDto.Of(user));
        }

        [HttpDelete("{id:Guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            _userRepository.Delete(id.ToString());
            _userRepository.Save();
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateUser(Guid id, [FromBody] User user)
        {
            var userToUpdate = _userRepository.Get(id.ToString());
            _userRepository.Update(user);

            userToUpdate.UserName = user?.UserName;
            userToUpdate.Email = user?.Email;
            
            _userRepository.Save();
            return Ok(UserResponseDto.Of(user));
        }

        [HttpGet]
        [Route("me")]
        public IActionResult Me()
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == "Id").Value;
            return Ok(UserResponseDto.Of(_userRepository.Get(userId)));
        }

        [HttpPost]
        [Route("me/address")]
        public IActionResult Address([FromBody] AddAddressDto request)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == "Id").Value;
            
            var address = new Address
            {
                AddressName = request.AddressName,
                Email = request.Email,
                City = request.City,
                Street = request.Street,
                Zip = request.Zip,
                BuildingNumber = request.BuildingNumber,
                ApartmentNumber = request?.ApartmentNumber,
                Name = request?.Name,
                Surname = request?.Surname,
                CompanyName = request?.CompanyName
            };

            var user = _userRepository.Get(userId);

            _userRepository.Update(user);
            switch (user)
            {
                case BusinessClient b:
                    b.AddressBook.Add(address);
                    break;
                case Client c:
                    c.AddressBook.Add(address);
                    break;
            }

            _userRepository.Save();
            return Ok(AddressResponseDto.ToDto(user.Id, address));
        }

        [HttpGet]
        [Route("/me/AddressBook")]
        public IActionResult Address()
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == "Id").Value;

            var add = _addressRepository.GetByUserId(userId);
            return Ok(add);
        }
    }
}
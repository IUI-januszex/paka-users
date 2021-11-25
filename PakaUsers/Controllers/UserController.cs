using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakaUsers.Dto.Requests;
using PakaUsers.Dto.Responses;
using PakaUsers.IdentityAuth;
using PakaUsers.Model;
using PakaUsers.Services;

namespace PakaUsers.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUserService _userService;
        private readonly StatusCodeHelper _statusCodeHelper = new();

        public UserController(IUserRepository userRepository,
            IAddressRepository addressRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return !_userService.HasCurrentUserAnyRole(UserType.Admin)
                ? _statusCodeHelper.UnauthorizedErrorResponse()
                : Ok(UserResponseDto.Of(_userRepository.GetAll()));
        }

        [HttpGet("{id:Guid}")]
        public IActionResult GetUser(Guid id)
        {
            return !_userService.HasCurrentUserAnyRole(UserType.Admin, UserType.Courier, UserType.Logistician) 
                ? _statusCodeHelper.UnauthorizedErrorResponse() 
                : Ok(UserResponseDto.Of(_userRepository.Get(id.ToString())));
        }

        [HttpGet]
        [Route("/email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            if (!_userService.IsUserLogged())
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            var user = _userRepository.GetAll().FirstOrDefault(user => user.Email == email);
            return Ok(UserResponseDto.Of(user));
        }

        [HttpDelete("{id:Guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            ///TODO żeby usuwało wszystko poza id
            _userRepository.Delete(id.ToString());
            _userRepository.Save();
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateUser(Guid id, [FromBody] User user)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin)
                || !IsCurrentUserId(id.ToString()))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

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
            return !_userService.IsUserLogged() 
                ? _statusCodeHelper.UnauthorizedErrorResponse() 
                : Ok(UserResponseDto.Of(_userService.GetCurrentUser()));
        }

        [HttpPost]
        [Route("me/address-book")]
        public IActionResult Address([FromBody] AddAddressBookRecordDto request)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.ClientBiz, UserType.ClientInd))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            var user = _userService.GetCurrentUser();

            var address = new AddressBookRecord
            {
                AddressName = request.AddressName,
                Email = request.Email,
                City = request.City,
                Street = request.Street,
                PostalCode = request.PostalCode,
                BuildingNumber = request.BuildingNumber,
                FlatNumber = request?.FlatNumber,
                Personalities = request.Personalities
            };

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
            return Ok(AddresBookRecordResponseDto.ToDto(user.Id, address));
        }

        [HttpGet]
        [Route("/me/address-book")]
        public IActionResult Address()
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.ClientBiz, UserType.ClientInd))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            var user = _userService.GetCurrentUser();

            var userAddressBook = _addressRepository.GetByUserId(user.Id);
            return Ok(userAddressBook);
        }

        [HttpPut]
        [Route("/activate-user/{id:Guid}")]
        public IActionResult Activate(Guid id, [FromBody] ActivationDto request)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            var userToActivate = _userRepository.Get(id.ToString());
            
            if (userToActivate.UserType == UserType.Admin)
            {
                return BadRequest(new Response{Message = "Bad request"});
            }
            _userRepository.Update(userToActivate);
            userToActivate.IsActive = request.IsActive;
            _userRepository.Save();
            return Ok();
        }

        private bool IsCurrentUserId(string id)
        {
            return _userService.GetCurrentUser()?.Id.Equals(id) ?? false;
        }
    }
}
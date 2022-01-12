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
            var user = _userRepository.Get(id.ToString());
            if (user == null)
            {
                return BadRequest(new Response {Message = "No user with this id!"});
            }

            return !_userService.HasCurrentUserAnyRole(UserType.Admin, UserType.Courier, UserType.Logistician)
                ? _statusCodeHelper.UnauthorizedErrorResponse()
                : Ok(UserResponseDto.Of(user));
        }

        [HttpGet]
        [Route("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            if (!_userService.IsUserLogged())
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            var user = _userRepository.GetAll().FirstOrDefault(user => user.Email == email);
            if (user == null)
            {
                return BadRequest(new Response {Message = "No user with this email!"});
            }
            return Ok(UserResponseDto.Of(user));
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }
            var user = _userRepository.Get(id.ToString());
            if (user == null)
            {
                return BadRequest(new Response {Message = "No user with this id!"});
            }
            _userRepository.Delete(id.ToString());
            _userRepository.Save();
            return NoContent();
        }

        [HttpPut]
        [Route("anonymize/{id:guid}")]
        public IActionResult AnonymizeUser(Guid id)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            if (IsCurrentUserId(id.ToString()))
            {
                //admin nie może zanonimizować siebie
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }
            var user = _userRepository.Get(id.ToString());
            if (user == null)
            {
                return BadRequest(new Response {Message = "No user with this id!"});
            }
            
            _userRepository.Anonymize(id.ToString());

            return Ok();
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
            if (userToUpdate == null)
            {
                return BadRequest(new Response {Message = "No user with this id!"});
            }
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
        [Route("me/address-book")]
        public IActionResult Address()
        {
            return !_userService.HasCurrentUserAnyRole(UserType.ClientBiz, UserType.ClientInd)
                ? _statusCodeHelper.UnauthorizedErrorResponse()
                : Ok(_addressRepository.GetByUserId(_userService.GetCurrentUser().Id));
        }

        [HttpDelete]
        [Route("me/address-book-record/{id:long}")]
        public IActionResult DeleteFromAddressBook(long id)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.ClientBiz, UserType.ClientInd))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            var user = _userService.GetCurrentUser();
            var addressBook = _addressRepository.GetByUserId(user.Id);
            var addressToRemove = addressBook.Find(x => x.Id == id);
            if (addressToRemove == null)
            {
                return BadRequest(new Response {Message = "No address book record with this id!"});
            }

            if (addressToRemove.ClientId != user.Id && addressToRemove.BusinessClientId != user.Id)
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            _userRepository.Update(user);
            switch (user)
            {
                case BusinessClient b:
                    b.AddressBook.Remove(addressToRemove);
                    break;
                case Client c:
                    c.AddressBook.Remove(addressToRemove);
                    break;
            }

            _userRepository.Save();
            return NoContent();
        }

        [HttpPut]
        [Route("activate-user/{id}")]
        public IActionResult Activate(Guid id, [FromBody] ActivationDto request)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            var userToActivate = _userRepository.Get(id.ToString());
            if (userToActivate == null)
            {
                return BadRequest(new Response {Message = "No user with this id!"});
            }
            if (userToActivate.UserType == UserType.Admin)
            {
                return BadRequest(new Response {Message = "Bad request"});
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
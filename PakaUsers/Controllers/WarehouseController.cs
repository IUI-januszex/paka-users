using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PakaUsers.Dto.Responses;
using PakaUsers.IdentityAuth;
using PakaUsers.Model;
using PakaUsers.Services;

namespace PakaUsers.Controllers
{
    public class WarehouseController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly StatusCodeHelper _statusCodeHelper = new();

        public WarehouseController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpGet]
        [Route("/logisticians/{id:long}")]
        public IActionResult GetLogisticiansByWarehouseId(long id)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin, UserType.Logistician))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            var logisticians = _userRepository.GetAll().Where(user =>
                user.UserType == UserType.Logistician && ((Logistician) user).WarehouseId == id);
            
            return Ok(UserResponseDto.Of(logisticians));
        }
        
        [HttpGet]
        [Route("/couriers/{id:long}")]
        public IActionResult GetCouriersByWarehouseId(long id)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            var couriers = _userRepository.GetAll().Where(user =>
                user.UserType == UserType.Courier && ((Logistician) user).WarehouseId == id);
            
            return Ok(UserResponseDto.Of(couriers));
        }
    }
}
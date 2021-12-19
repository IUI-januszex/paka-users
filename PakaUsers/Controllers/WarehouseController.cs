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
    [ApiController]
    [Route("api/warehouse")]
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
        [Route("logisticians/{id:long}")]
        public IActionResult GetLogisticiansByWarehouseId(long id)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin, UserType.Logistician))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            return Ok(
                _userRepository.GetAll()
                    .Where(user => user.UserType == UserType.Logistician)
                    .Cast<Logistician>()
                    .Where(logistician => logistician.WarehouseId == id)
                    .Select(WorkerResponseDto.Of));
        }

        [HttpGet]
        [Route("couriers/{id:long}")]
        public IActionResult GetCouriersByWarehouseId(long id)
        {
            if (!_userService.HasCurrentUserAnyRole(UserType.Admin))
            {
                return _statusCodeHelper.UnauthorizedErrorResponse();
            }

            return Ok(
                _userRepository.GetAll()
                    .Where(user => user.UserType == UserType.Courier)
                    .Cast<Courier>()
                    .Where(courier => courier.WarehouseId == id)
                    .Select(WorkerResponseDto.Of));
        }
    }
}
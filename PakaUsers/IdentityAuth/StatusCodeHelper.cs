using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PakaUsers.IdentityAuth
{
    public class StatusCodeHelper : ControllerBase
    {
        public ObjectResult UnauthorizedErrorResponse()
        {
            return Unauthorized(new Response {Message = "Unauthorized"});
        }
        
    }
}
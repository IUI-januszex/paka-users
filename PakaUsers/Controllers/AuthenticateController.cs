using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PakaUsers.IdentityAuth;
using PakaUsers.Model;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PakaUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response { Status = "Error", Message = "User already exists!" });

            var user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            user.UserName = Guid.NewGuid().ToString();
            var result = await userManager.CreateAsync(user, model.Password);
            return !result.Succeeded ? 
                StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." }) 
                : Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response { Status = "Error", Message = "User already exists!" });

            var user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            user.UserName = Guid.NewGuid().ToString();
            var result = await userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserType.Admin.ToString()))
                await roleManager.CreateAsync(new IdentityRole(UserType.Admin.ToString()));
            
            /*if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));*/

            if (await roleManager.RoleExistsAsync(UserType.Admin.ToString()))
                await userManager.AddToRoleAsync(user, UserType.Admin.ToString());
            

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.EmailAddress);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                //czy to potrzebne, jeden użytkownik ma jedną rolę
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, user.UserType.ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

                var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
    }
}
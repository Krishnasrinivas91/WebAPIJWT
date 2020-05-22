using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiJWTs_GitHub.Models;

namespace WebApiJWTs_GitHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config { get; }
        public LoginController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]UserModel userModel)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(userModel);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }
            return response;
        }

        private string GenerateJSONWebToken(object user)
        {
            var securityKey = JwtSecurity.JwtSecurityKey.Create(_config["Jwt:Key"]);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel userModel)
        {
            if (userModel != null && userModel.UserName == "TestUser" && userModel.Password == "TestPassword")
            {
                return new UserModel { UserName = "TestUser", UserEmail = "Test@Test.com" };
            }
            return null;
        }
    }
}
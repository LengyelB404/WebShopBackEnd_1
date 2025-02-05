using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using WebShopBackEnd_1.Dtos;
using WebShopBackEnd_1.Models;

namespace WebShopBackEnd_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly int expire_day = 1;


        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("Registry")]
        public IActionResult Registry(RegistryDto registryDto)
        {
            using(var context = new Webshop1Context())
            {
                try
                {
                    if (context.Users.ToList().Find(x => x.UserName == registryDto.Username) == null)
                    {
                        User user = new User();
                        user.UserName = registryDto.Username;
                        user.Password = BCrypt.Net.BCrypt.HashPassword(registryDto.password, 4);
                        user.LastLogin = DateTime.Now;
                        user.PermissionId = 2;
                        context.Users.Update(user);
                        context.SaveChanges();
                        return Ok("User registred!");
                    }
                    else
                    {
                        return BadRequest("Username already in use!");
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            using (var context = new Webshop1Context())
            {
                /*
                try
                {
                */
                    User user = context.Users.ToList().First(x => x.UserName == loginDto.Username)!;
                    if (user == null)
                    {
                        return BadRequest("Incorrect password or username1");
                    }
                    if (!BCrypt.Net.BCrypt.Verify(loginDto.password,user.Password))
                    {
                        return BadRequest("Incorrect password or username2");
                    }
                    user.LastLogin = DateTime.Now;
                    string token = CreateToken(user);
                    context.Tokens.Add(new Token { UserId = user.Id, Token1 = token, ExpireDate = DateTime.Now.AddDays(expire_day) });
                    context.SaveChanges();
                    return Ok(token);
                /*
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                */
            }
        }

        private string CreateToken(User user)
        {
            using (var context = new Webshop1Context())
            {
                string permission = context.Permissions.First(x => x.Id == user.PermissionId).Name;
                
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("Name",user.UserName),
                    new Claim("role",permission),
                    new Claim("id",user.Id.ToString())

                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AuthSettings:JwtOptions:Token").Value!));

                var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken
                (
                    claims: claims,
                    expires: DateTime.Now.AddDays(expire_day),
                    signingCredentials: creds,
                    audience: _configuration.GetSection("AuthSettings:JwtOptions:Audience").Value,
                    issuer: _configuration.GetSection("AuthSettings:JwtOptions:Issuer").Value
                );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
        }


    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebAPI_DotNet_6_WithJWT.DBContext;
using WebAPI_DotNet_6_WithJWT.Models;
using System.Security.Claims;

namespace WebAPI_DotNet_6_WithJWT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JWT_DBContext _context;
        private readonly JWTSetting _JwtSetting;
        public UserController( JWT_DBContext context, IOptions<JWTSetting> options)
        {
            _context = context;
            _JwtSetting = options.Value;
        }

        [Route("Authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var _user = _context.TblUsers.FirstOrDefault(o => o.Userid == userCred.UserName && o.Password == userCred.Password);
            if (_user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_JwtSetting.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim(ClaimTypes.Name, _user.Userid),
                            }
                    ),
                Expires=DateTime.Now.AddMinutes(2),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string finalToken=tokenHandler.WriteToken(token);

            return Ok(finalToken);
        }
    }
}

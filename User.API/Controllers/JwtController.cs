using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using User.API.Models;
using User.API.Models.ViewModels;
using User.API.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    public class JwtController : Controller
    {
        private readonly IRepository<AppUser, int> _repository;

        private readonly JwtSettings _jwtSettings;

        public JwtController(IRepository<AppUser, int> repository, IOptions<JwtSettings> jwtSettingsAccessor)
        {
            _repository = repository;
            _jwtSettings = jwtSettingsAccessor.Value;
        }

        [HttpPost("makeToken")]
        public async Task<IActionResult> MakeToken([FromBody]LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _repository.SingleAsync(a => a.Name == login.UserName && a.Phone == login.Phone);
            if (user == null)
            {
                return BadRequest();
            }
            var claims = new Claim[] {
                new Claim(ClaimTypes.Name,user.Name),
                new Claim("Phone",user.Phone),
                new Claim(ClaimTypes.Role,"admin")
            };
            //get sectet key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, DateTime.Now, DateTime.Now.AddMinutes(30), creds);
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(jwtToken) });
        }
    }
}

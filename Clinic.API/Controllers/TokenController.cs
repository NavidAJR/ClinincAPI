using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Repositories.Interfaces;
using Clinic.Domain.Context;
using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Clinic.API.Controllers
{
    public class TokenController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public TokenController(UserManager<User> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }


        [Route("/token")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(string userName, string password, string grantType)
        {
            if (await IsValidUsernameAndPasswordAsync(userName, password))
            {
                return new ObjectResult(await GenerateTokenAsync(userName));
            }


            return BadRequest(0);
        }




        private async Task<bool> IsValidUsernameAndPasswordAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return await _userManager.CheckPasswordAsync(user, password);
        }




        private async Task<dynamic> GenerateTokenAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _tokenRepository.ReadAllUserRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            };

            var allClaims = User.Claims.ToList();

            claims.ForEach(c=> allClaims.Add(c));

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyIsSecretSoDoNotTellToAnyone")),
                        SecurityAlgorithms.HmacSha256)),
                        new JwtPayload(allClaims));

            var output = new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = userName
            };

            return output;
        }
    }
}

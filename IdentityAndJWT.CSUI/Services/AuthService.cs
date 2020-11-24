using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityAndJWT.CSUI.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAndJWT.CSUI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Register(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<SignInResult> Login(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        public string GetToken(User user)
        {
            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Email)
                };

            return GenerateJwtToken(claims);
        }

        private string GenerateJwtToken(List<Claim> claimList)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("eee9b1be342b4e7991810cde7c18c27095116baa26245f00b249867cdf48832602caa79677032d7ee0b7bd71787731cd90e9d1ef34d818c6b5133b3cb15dd34c");

            var claimsIdentity = new ClaimsIdentity(claimList);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

    }
}
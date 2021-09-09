using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library_Management_System.Models;
using Library_Management_System.Repositories;
using Library_Management_System.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Library_Management_System.Services
{
    public interface IUserServices
    {
        Task<object> CreateAsync(RegisterModel model);
        Task<object> AuthAsync(RegisterModel model);
    }
    public class UserServices : IUserServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<UserServices> _logger;
        private readonly IRoleRepository _roleRepository;
        private readonly AppSetting _appSetting;
        public UserServices(ILogger<UserServices> logger, IOptions<AppSetting> appSetting, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IRoleRepository roleRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleRepository = roleRepository;
            _appSetting = appSetting.Value;
        }

        public async Task<object> AuthAsync(RegisterModel model)
        {
            if (string.IsNullOrEmpty(model.EmailAddress) || string.IsNullOrEmpty(model.Password))
                return null;

            //check if username exists
            var user = await _userManager.FindByEmailAsync(model.EmailAddress);


            if (user == null)
                return null;

            //sign user in
            var result = await _signInManager.PasswordSignInAsync(model.EmailAddress, model.Password, false, false);
            if (!result.Succeeded)
            {
                throw new Exception("Cannot sign in User");
            }
            
            var token = await GenerateTokenAsync(user);
     
            //authentication successful
            return token;
        }

        public async Task<object> CreateAsync(RegisterModel model)
        {
            //check if user is existing
            var existingUser = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (existingUser != null)
                throw new Exception($"Email {model.EmailAddress} already exist");

            //create user role
            var userRole = await _roleRepository.SeedRoleAsync("User");

            //create user
            var user = new IdentityUser
            {
                Email = model.EmailAddress,
                UserName = model.EmailAddress,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new Exception("Unable to register, please try again later.");

            //assign created user to role
            var addToRoleResult = await _roleRepository.AddUserToRole(user, "User");
            if (!addToRoleResult)
                throw new Exception("Cannot assign user to the specified role");

            var token = await GenerateTokenAsync(await _roleRepository.GetApplicationUser(user.Email));
            return token;
        }

        private async Task<string> GenerateTokenAsync(IdentityUser user)
        {
            var a = _appSetting.Audience;
            var b = _appSetting.Issuer;
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSetting.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Subject = new ClaimsIdentity(new[]
                //{
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //    new Claim(ClaimTypes.Name, user.Id.ToString()),
                //    new Claim(ClaimTypes.Email, user.Email),
                    
                //}),
                Subject = await getClaimsIdentityAsync(user),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _appSetting.Audience,
                Issuer = _appSetting.Issuer
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
        async Task<ClaimsIdentity> getClaimsIdentityAsync(IdentityUser user)
        {

            return new ClaimsIdentity(await getClaimsAsync(user));

            async Task<Claim[]> getClaimsAsync(IdentityUser user)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                foreach (var item in userRole)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }
                return claims.ToArray();
            }

        }
        
    }
}

using System;
using System.Threading.Tasks;
using Library_Management_System.Entity;
using Microsoft.AspNetCore.Identity;

namespace Library_Management_System.Repositories
{
    public interface IRoleRepository : IGenericRepository<IdentityRole>
    {
        Task<bool> SeedRoleAsync(string role);
        Task<bool> AddUserToRole(IdentityUser user, string role);
        Task<IdentityUser> GetApplicationUser(string email);
    }
    public class RoleRepository : GenericRepository<IdentityRole>, IRoleRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
            : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> SeedRoleAsync(string role)
        {
            var checkIfRoleExist = await _roleManager.FindByNameAsync(role);
            if (checkIfRoleExist == null)
            {
                //create role
                var roleCreated = await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = role
                });
                if (roleCreated.Succeeded)
                    return true;
                else
                    return false;
            }
            return true;
        }
        public async Task<bool> AddUserToRole(IdentityUser user, string role)
        {
            var checkIfRoleExist = await SeedRoleAsync(role);
            if (checkIfRoleExist)
            {
                var addUserToRoleResult = await _userManager.AddToRoleAsync(user, role);
                if (addUserToRoleResult.Succeeded)
                {
                    return true;
                }
                else
                    return false;
            }
            return false;
        }
        public async Task<IdentityUser> GetApplicationUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SMAK_AJWTAuthNetCore_API.Commons;
using SMAK_AJWTAuthNetCore_API.ViewModels;
using SMAK_AJWTAuthNetCore_Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SMAK_AJWTAuthNetCore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<RegisterRequestModel> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly SignInManager<RegisterRequestModel> SignInManager;
        private readonly JsonWebTokenKeys JsonWebTokenKeys;
        public AuthController
        (
            UserManager<RegisterRequestModel> userManager,
            SignInManager<RegisterRequestModel> signInManager,
            RoleManager<IdentityRole> roleManager,
            JsonWebTokenKeys jsonWebTokenKeys)
        {
            this.UserManager = userManager;
            this.RoleManager = roleManager;
            this.SignInManager = signInManager;
            this.JsonWebTokenKeys = jsonWebTokenKeys;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserModelcs registerVM)
        {
            var IsExist = await UserManager.FindByNameAsync(registerVM.Name);
            if (IsExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            RegisterRequestModel appUser = new RegisterRequestModel
            {
                UserName = registerVM.Name,
                AccountType = registerVM.AccountType,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNo,
                Password = registerVM.Password,
                UserRole = registerVM.UserRole,
                IsDeleted = registerVM.IsDeleted
            };
            var result = await UserManager.CreateAsync(appUser, registerVM.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            if (!await RoleManager.RoleExistsAsync(registerVM.UserRole))
                await RoleManager.CreateAsync(new IdentityRole(registerVM.UserRole));
            if (await RoleManager.RoleExistsAsync(registerVM.UserRole))
            {
                await UserManager.AddToRoleAsync(appUser, registerVM.UserRole);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {
            //var user1 = await userManager.FindByIdAsync(loginVM.Id);
            var user = await UserManager.FindByNameAsync(loginVM.UserName);
            if (user != null && await UserManager.CheckPasswordAsync(user, loginVM.Password))
            {
                var userRoles = await UserManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JsonWebTokenKeys.IssuerSigningKey));

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    api_key = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = user,
                    Role = userRoles,
                    status = "User Login Successfully"
                });
            }
            return Unauthorized();
        }
    }
}

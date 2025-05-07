using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMAK_AJWTAuthNetCore_API.Commons;
using SMAK_AJWTAuthNetCore_API.ViewModels;
using SMAK_AJWTAuthNetCore_Core.Entities;
using SMAK_AJWTAuthNetCore_Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SMAK_AJWTAuthNetCore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly JsonWebTokenKeys JsonWebTokenKeys;
        private readonly ITokenService TokenService;
        public readonly IUsersRepository<ApplicationUser> UsersRepository;

        public AuthController
        (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            JsonWebTokenKeys jsonWebTokenKeys,
            ITokenService tokenService,
            IUsersRepository<ApplicationUser> usersRepository)
        {
            this.UserManager = userManager;
            this.RoleManager = roleManager;
            this.SignInManager = signInManager;
            this.JsonWebTokenKeys = jsonWebTokenKeys;
            this.TokenService = tokenService;
            UsersRepository = usersRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM registerVM)
        {
            var IsExist = UsersRepository.GetByEmail(registerVM.Email);
            if (IsExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            ApplicationUser appUser = new ApplicationUser
            {
                Name = registerVM.Name,
                UserName = registerVM.Name,
                AccountType = registerVM.AccountType,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNo,
                Password = registerVM.Password,
                UserRole = registerVM.UserRole,
                RefreshToken = TokenService.GenerateRefreshToken(),
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
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
            var user = UsersRepository.GetByEmail(loginVM.Email);
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

                var accessToken = TokenService.GenerateAccessToken(authClaims);
                var refreshToken = TokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                UsersRepository.Update(user);

                return Ok(new
                {
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    user = user,
                    Role = userRoles,
                    status = "User Login Successfully"
                });
            }
            return Unauthorized();
        }
    }
}

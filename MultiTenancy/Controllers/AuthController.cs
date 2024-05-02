using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MultiTenancy.Model;
using MultiTenancy.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace MultiTenancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private HttpContext _httpContext;
        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _configuration = configuration;
            _httpContext = contextAccessor.HttpContext;
        }

        /// <summary>
        /// add user into the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("register-user")]
        public async Task<IActionResult> RegisterUser(UserRegisterViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel { Status = "Error", Message = "Email already exists!" });
            }

            ApplicationUser applicationUser = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedDateTime = DateTime.Now,
            };
            var result = await _userManager.CreateAsync(applicationUser, model.Password);
            if (result.Succeeded != true)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel { Status = "Error", Message = "Account creation failed! Please check the details and try again." });
            }
            return Ok(new ResponseViewModel { Status = "Success", Message = "Account create successfully!", Data = applicationUser.Id });
        }
        /// <summary>
        /// Login for  User.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User Token, User Id and user role</returns>
        /// <response code="400">If the item is null</response>       
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new ResponseViewModel { Status = "Error", Message = "The Email is not associated with any account" });
            }
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _httpContext.Request.Headers.TryGetValue("tenant", out var tenantId);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.System, tenantId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                DateTime tokenExpireDateTime = DateTime.Now.AddHours(3);
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: tokenExpireDateTime,
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userId = user.Id
                });
            }
            else
            {
                return Unauthorized(new ResponseViewModel { Status = "Error", Message = "The password that you've entered is incorrect" });
            }
        }
    }
}

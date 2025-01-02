using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using aspDotNetCore.Data;
using Microsoft.Extensions.Options;

namespace aspDotNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IOptions<JwtOptions> _jwtOptions;
        private readonly ApplicationDbContext _dbContext;

        public UsersController(IOptions<JwtOptions> jwtOptions, ApplicationDbContext dbContext)
        {
            _jwtOptions = jwtOptions ?? throw new ArgumentNullException(nameof(jwtOptions));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpPost("auth")]
        public IActionResult AuthenticateUser([FromBody] AuthenticationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Invalid authentication request.");
            }

            var user = _dbContext.Set<User>().FirstOrDefault(x => x.Name == request.UserName && x.Password == request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtOptions = _jwtOptions.Value;

            var key = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
            if (key.Length < 16) // Ensure sufficient key strength
            {
                throw new InvalidOperationException("The signing key is too short.");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                }),
                Expires = DateTime.UtcNow.AddHours(1)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Ok(new { AccessToken = accessToken });
        }
    }
}

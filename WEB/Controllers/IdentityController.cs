using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEB.Data;
using WEB.Models;
using WEB.ViewModels;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly string SecretKey;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityController(IConfiguration config, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            SecretKey = config.GetSection("Settings").GetSection("SecretKey").ToString();
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        // GET: api/<IdentityController>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUsuario credenciales)
        {
            var user = await _userManager.FindByEmailAsync(credenciales.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, credenciales.Password))
            {
                return Unauthorized(new LocalStorage{ Success = false});
            }

            var keyBytes = Encoding.ASCII.GetBytes(SecretKey);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Email));
            var expiration = DateTime.Now.AddDays(1);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature),

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string Token = tokenHandler.WriteToken(tokenConfig);

            var rol = _userManager.GetRolesAsync(user);

            LocalStorage localS = new LocalStorage{IdUsuario=user.IdColaborador, Correo=user.Email, Token=Token, Expiration=expiration, Rol=rol.Result.First(), Success=true};
            return Ok(localS);
        }

    }
}

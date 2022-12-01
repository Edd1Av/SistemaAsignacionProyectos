using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEB.Data;
using WEB.Models;
using WEB.ViewModels;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using WEB.Services.Mails;
using Microsoft.Extensions.Options;

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
        private readonly IEmailSender _emailSender;
        private EmailSenderOptions _options { get; }

        public IdentityController(IConfiguration config, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IOptions<EmailSenderOptions> options)
        {
            SecretKey = config.GetSection("Settings").GetSection("SecretKey").ToString();
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _options = options.Value;
        }


        // GET: api/<IdentityController>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUsuario credenciales)
        {
            var user = await _userManager.FindByEmailAsync(credenciales.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, credenciales.Password))
            {
                return Ok(new LocalStorage{ Success = false});
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


        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> changePassword(NewPassword model)
        {
            if(model.NPassword != model.NPasswordConfirm)
            {
                return Ok(new Response { success = false, response = "Las contraseñas no coinciden"});
            }
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return Ok(new Response { success = false, response = "Usuario no encontrado" });
                }

                var colaborador = _context.Colaboradores.Include(x => x.IdentityUser).Where(x => x.IdentityUser.NormalizedEmail == model.Email.Trim().ToUpper()).First();

                var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NPassword);

                if (result.Succeeded)
                {
                    string message = "<p>Hemos recibido una solicitud para cambiar su contraseña.</p><br><p>Su nueva contraseña: " + model.NPassword + "</p>";
                    string name = colaborador.Nombres + " " + colaborador.Apellidos;
                    try
                    {
                        await _emailSender.SendEmailAsync(user.Email, "Cambio de contraseña - SAP Plenumsoft", name, message, "").ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        return Ok(new Response { success = false, response = "Error: Correo"});
                    }
                    return Ok(new Response { success = true, response = "La contraseña se ha cambiado correctamente" });

                }
                else{
                    return Ok(new Response { success = false, response = "Contraseñas invalidas" });
                }
                    
                }
                catch (Exception ex)
                {
                    return Ok(new Response{ success = false, response = "Error al cambiar la contraseña." });
                }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> resetPassword(ResetPassword model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email.Trim());

                if (user == null)
                {
                    return Ok(new Response { success = false, response = "Usuario no encontrado" });
                }

                var colaborador = _context.Colaboradores.Include(x => x.IdentityUser).Where(x => x.IdentityUser.NormalizedEmail == model.Email.Trim().ToUpper()).First();

               

                var code = await _userManager.RemovePasswordAsync(user);
                if (code.Succeeded)
                {
                    string password = "Pa$word2";
                    var result = await _userManager.AddPasswordAsync(user, password);

                    if (result.Succeeded)
                    {
                        string message = "<p>Hemos recibido una solicitud para restablecer su contraseña.</p><br><p> Su nueva contraseña: " + password + "</p>";
                        string name = colaborador.Nombres + " " + colaborador.Apellidos;
                        try
                        {
                            await _emailSender.SendEmailAsync(user.Email, "Restablecer contraseña - SAP Plenumsoft", name, message, "").ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            return Ok(new Response { success = false, response = "Error: Email"});
                        }

                        return Ok(new Response { success = true, response = "La contraseña se ha restablecido correctamente" });
                    }
                }
                return Ok(new Response { success = true, response = "Error" });
            }
            catch (Exception ex)
            {
                return Ok(new Response { success = false, response = "Error al cambiar la contraseña." });
            }
        }



        private string GeneratePassword()
        {
            var options = _userManager.Options.Password;

            int length = options.RequiredLength;

            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            while (password.Length <= 9)
            {
                char c = (char)random.Next(65, 91);

                password.Append(c);
            }

            return password.ToString();
        }

    }
}

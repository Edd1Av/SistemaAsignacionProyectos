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
using Microsoft.AspNetCore.Authorization;
using WEB.Services;

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

            LocalStorage localS = new LocalStorage{IdUsuario= (int)(user.IdColaborador!= null ? user.IdColaborador: 0), Correo=user.Email, Token=Token, Expiration=expiration, Rol=rol.Result.First(), Success=true};
            return Ok(localS);
        }

        [HttpPost]
        [Authorize]
        [Route("AddAdmin")]
        public async Task<IActionResult> PostAdministrador(AddAdmin admin)
        {
            Response response = new Response();
            
            var c = await _userManager.FindByEmailAsync(admin.Email.Trim());

            if (c != null)
            {
                response.success = false;
                response.response = $"Ya existe un administrador con ese Email";
                return Ok(response);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    ApplicationUser user = new ApplicationUser();
                    user.Email = admin.Email.Trim();
                    //user.NormalizedEmail = colaborador.Email.Trim().ToUpper();
                    user.EmailConfirmed = true;
                    user.UserName = admin.Email.Trim();

                    string password = Password.GeneratePassword(_userManager);

                    var x = await _userManager.CreateAsync(user, password);
                    await _context.SaveChangesAsync();
                    if (x.Succeeded)
                    {
                            var y = await _userManager.AddToRoleAsync(user, "Administrador");
                            if (y.Succeeded)
                            {

                            }
                            else
                            {
                                transaction.Rollback();
                                response.success = false;
                                response.response = $"No se pudo asignar el rol Administrador";
                                return Ok(response);
                            }
                    }
                    else
                    {
                        transaction.Rollback();
                        response.success = false;
                        response.response = $"No se pudo crear el usuario";
                        return Ok(response);
                    }
                   
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    //var Link = Url.PageLink().Split('/');
                    //string url = Link[0] + "//" + Link[2];
                    string url = _options.Url;
                    string name = $"";
                    string message = "<p>Por este medio confirmamos su registro al sistema Plenumsoft y " +
                               $"compartimos con usted sus claves de acceso:</p><p>Usuario: <span>{user.Email}" +
                               $"</span></p><p>Contraseña: <span>{password}</span></p><p>Para ingresar " +
                               "al sistema, dar clic en el siguiente enlace:</p><a style='color: #1B57A6' " +
                               $"href='{url}'>{url}</a>";
                    try
                    {
                        await _emailSender.SendEmailAsync(user.Email, "Nuevo Administrador - SAP Plenumsoft", name, message, "").ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        return Ok(new Response { success = true, response = "Error: Email" });
                    }

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    response.success = false;
                    response.response = $"Error al registrar";
                    return Ok(response);
                }

            }
            response.success = true;
            response.response = "Registrado con éxito";
            return Ok(response);
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

                //var colaborador = _context.Colaboradores.Include(x => x.IdentityUser).Where(x => x.IdentityUser.NormalizedEmail == model.Email.Trim().ToUpper()).First();

                var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NPassword);

                if (result.Succeeded)
                {
                    string message = "<p>Hemos recibido una solicitud para cambiar su contraseña.</p><p>Su nueva contraseña: " + model.NPassword + "</p>";
                    //string name = colaborador.Nombres + " " + colaborador.Apellidos;
                    try
                    {
                        await _emailSender.SendEmailAsync(user.Email, "Cambio de contraseña - SAP Plenumsoft", "", message, "").ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        return Ok(new Response { success = true, response = "Error: Email"});
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

                //var colaborador = _context.Colaboradores.Include(x => x.IdentityUser).Where(x => x.IdentityUser.NormalizedEmail == model.Email.Trim().ToUpper()).First();

               

                var code = await _userManager.RemovePasswordAsync(user);
                if (code.Succeeded)
                {
                    string password = Password.GeneratePassword(_userManager);
                    var result = await _userManager.AddPasswordAsync(user, password);

                    if (result.Succeeded)
                    {
                        string message = "<p>Hemos recibido una solicitud para restablecer su contraseña.</p><p> Su nueva contraseña: " + password + "</p>";
                        //string name = colaborador.Nombres + " " + colaborador.Apellidos;
                        try
                        {
                            await _emailSender.SendEmailAsync(user.Email, "Restablecer contraseña - SAP Plenumsoft", "", message, "").ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            return Ok(new Response { success = true, response = "Error: Email"});
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

    }
}

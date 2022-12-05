using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB.Data;
using WEB.Models;
using Newtonsoft.Json;
using WEB.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Data.Entities.Enum;
using WEB.Services.Mails;
using Microsoft.Extensions.Options;

namespace WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private EmailSenderOptions _options { get; }


        public ColaboradoresController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IOptions<EmailSenderOptions> options)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _options = options.Value;
        }

        // GET: api/Colaboradores
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ColaboradorPost>>> GetColaboradores()
        {
            var x = await _userManager.GetUsersInRoleAsync("Desarrollador");

            var colaboradores = await _context.Colaboradores.Include(x=>x.IdentityUser).Where(x => x.Id != 1).ToListAsync();
            List<ColaboradorPost> IdentityColaborador = new List<ColaboradorPost>();
            foreach (var element in colaboradores)
            {
                List<ProyectosPost> Proyectos = new List<ProyectosPost>();
                var temp = _context.Distribucion.Include(x => x.Asignacion).ThenInclude(x => x.Colaborador).Include(x => x.Proyecto)
                .Where(x => x.Asignacion.IdColaborador == element.Id && x.Fecha_Final >= DateTime.Now.Date).ToList();
                foreach (var i in temp)
                {
                    Proyectos.Add(new ProyectosPost { Titulo = i.Proyecto.Titulo, Clave = i.Proyecto.Clave });
                }
                ColaboradorPost colaborador = new ColaboradorPost();
                colaborador.Nombres = element.Nombres;
                colaborador.Apellidos = element.Apellidos;
                colaborador.Email = element.IdentityUser.Email;
                colaborador.CURP = element.CURP;
                colaborador.Id_Odoo = element.Id_Odoo;
                colaborador.Id = element.Id;
                colaborador.IsAdmin = element.IsAdmin;
                colaborador.Proyectos = Proyectos.Count > 0 ? Proyectos : null;
                IdentityColaborador.Add(colaborador);
            }
            //var rolDesarrollador = _roleManager.FindByNameAsync("Desarrollador");
            //var colaboradores = await _context.Colaboradores.Where(x =>x.IdentityUser.Roles.Any(y => y.RoleId == rolDesarrollador.Result.Id)).OrderBy(x => x.Id_Odoo).ToListAsync();
            return IdentityColaborador;
        }

        // GET: api/Colaboradores/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Colaborador>> GetColaborador(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);

            if (colaborador == null)
            {
                return NotFound();
            }

            return Ok(colaborador);
        }


        [HttpGet]
        [Authorize]
        [Route("Desarrolladores")]
        public async Task<ActionResult<IEnumerable<ColaboradorPost>>> GetDesarrolladores()
        {
            //var x = await _userManager.GetUsersInRoleAsync("Desarrollador");

            var colaboradores = await _context.Colaboradores.Include(x => x.IdentityUser).Where(x => x.IsAdmin == false).ToListAsync();
            List<ColaboradorPost> IdentityColaborador = new List<ColaboradorPost>();
            foreach (var element in colaboradores)
            {
                ColaboradorPost colaborador = new ColaboradorPost();
                colaborador.Nombres = element.Nombres;
                colaborador.Apellidos = element.Apellidos;
                colaborador.Email = element.IdentityUser.Email;
                colaborador.CURP = element.CURP;
                colaborador.Id_Odoo = element.Id_Odoo;
                colaborador.Id = element.Id;

                IdentityColaborador.Add(colaborador);
            }
            //var rolDesarrollador = _roleManager.FindByNameAsync("Desarrollador");
            //var colaboradores = await _context.Colaboradores.Where(x =>x.IdentityUser.Roles.Any(y => y.RoleId == rolDesarrollador.Result.Id)).OrderBy(x => x.Id_Odoo).ToListAsync();
            return IdentityColaborador;
        }

        // PUT: api/Colaboradores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutColaborador(int id, ColaboradorPost colaborador)
        {
            Response response = new Response();
            if (id != colaborador.Id)
            {
                response.success = false;
                response.response = "Los ID no coinciden";
                return Ok(response);
            }

            try
            {
                if (_context.Colaboradores.Any(x => x.CURP.ToUpper().Trim() == colaborador.CURP.ToUpper().Trim() && x.Id != colaborador.Id))
                {
                    response.success = false;
                    response.response = $"Ya existe un contacto con ese CURP";
                    return Ok(response);
                }
                if (_context.Colaboradores.Any(x => x.Id_Odoo.ToUpper().Trim() == colaborador.Id_Odoo.ToUpper().Trim() && x.Id != colaborador.Id))
                {
                    response.success = false;
                    response.response = $"Ya existe un contacto con esa clave Odoo";
                    return Ok(response);
                }

                Colaborador updateColaborador = new Colaborador();
                updateColaborador.Id = colaborador.Id;
                updateColaborador.Nombres = colaborador.Nombres.Trim();
                updateColaborador.Apellidos = colaborador.Apellidos.Trim();
                updateColaborador.CURP = colaborador.CURP.ToUpper().Trim();
                updateColaborador.Id_Odoo = colaborador.Id_Odoo.Trim();


                _context.Entry(updateColaborador).State = EntityState.Modified;

                _context.Logger.Add(new Log()
                {
                    Created = DateTime.Now,
                    User = "ADMIN",
                    Id_User = 1.ToString(),
                    Accion = ETipoAccionS.GetString(ETipoAccion.UPDATECOLABORADOR),
                    Description=ETipoAccionS.GetString(ETipoAccion.UPDATECOLABORADOR) + " Con CURP:" + updateColaborador.CURP+" Por ADMIN",
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!ColaboradorExists(id))
                {
                   
                    response.success = false;
                    response.response = "No existe un colaborador con ese ID";
                    return Ok(response);
                }
                else
                {
                    response.success = false;
                    response.response = "Error al editar el registro";
                    return Ok(response);
                }
            }
            response.success = true;
            response.response = "Registro editado con éxito";
            return Ok(response);

        }

        // POST: api/Colaboradores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostColaborador(ColaboradorPost colaborador)
        {
            Response response = new Response();
            if (_context.Colaboradores.Any(x => x.CURP.ToUpper().Trim() == colaborador.CURP.ToUpper().Trim()))
            {
                response.success = false;
                response.response = $"Ya existe un contacto con ese CURP";
                return Ok(response);
            }
            if (_context.Colaboradores.Any(x => x.Id_Odoo.ToUpper().Trim() == colaborador.Id_Odoo.ToUpper().Trim()))
            {
                response.success = false;
                response.response = $"Ya existe un contacto con esa clave Odoo";
                return Ok(response);
            }

            var c = await _userManager.FindByEmailAsync(colaborador.Email.Trim());

            if (c!=null)
            {
                response.success = false;
                response.response = $"Ya existe un contacto con ese Email";
                return Ok(response);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                
                try
                {
                    
                    Colaborador addColaborador = new Colaborador();
                    addColaborador.Nombres = colaborador.Nombres.Trim();
                    addColaborador.Apellidos = colaborador.Apellidos.Trim();
                    addColaborador.CURP = colaborador.CURP.ToUpper().Trim();
                    addColaborador.Id_Odoo = colaborador.Id_Odoo.Trim();
                    addColaborador.IsAdmin = colaborador.IsAdmin;
                    _context.Colaboradores.Add(addColaborador);


                    ApplicationUser user = new ApplicationUser();
                    user.Email = colaborador.Email.Trim();
                    //user.NormalizedEmail = colaborador.Email.Trim().ToUpper();
                    user.Colaborador = addColaborador;
                    user.EmailConfirmed = true;
                    user.UserName = colaborador.Id_Odoo;

                    string password = "Pa$word1"; 

                    var x = await _userManager.CreateAsync(user,password);
                    await _context.SaveChangesAsync();
                    if (x.Succeeded)
                    {
                        if(colaborador.IsAdmin == true)
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
                            var y = await _userManager.AddToRoleAsync(user, "Desarrollador");
                            if (y.Succeeded)
                            {
                                //Enviar Correo
                            }
                            else
                            {
                                transaction.Rollback();
                                response.success = false;
                                response.response = $"No se pudo asignar un rol";
                                return Ok(response);
                            }
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        response.success = false;
                        response.response = $"No se pudo crear el usuario";
                        return Ok(response);
                    }
                    _context.Logger.Add(new Log()
                    {
                        Created = DateTime.Now,
                        User = "ADMIN",
                        Id_User = 1.ToString(),
                        Accion = ETipoAccionS.GetString(ETipoAccion.ADDCOLABORADOR),
                        Description=ETipoAccionS.GetString(ETipoAccion.ADDCOLABORADOR) + " Con CURP:" + colaborador.CURP+" Por ADMIN",
                    });
                    await _context.SaveChangesAsync();
                   
                    transaction.Commit();
                    var Link = Url.PageLink().Split('/');
                    string url = Link[0] + "//" + Link[2];
                    string name = $"{colaborador.Nombres} {colaborador.Apellidos}";
                    string message = "<p>Por este medio confirmamos su registro al sistema Plenumsoft y " +
                               $"compartimos con usted sus claves de acceso:</p><p>Usuario: <span>{user.Email}" +
                               $"</span></p><p>Contraseña: <span>{password}</span></p><p>Para ingresar " +
                               "al sistema, dar clic en el siguiente enlace:</p><a style='color: #1B57A6' " +
                               $"href='{url}'>{url}</a>";
                    try
                    {
                        await _emailSender.SendEmailAsync(user.Email, "Nueva cuenta de usuario - SAP Plenumsoft", name, message, "").ConfigureAwait(false);
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

        // DELETE: api/Colaboradores/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteColaborador(int id)
        {
            Response response = new Response();
            using (var transaction = _context.Database.BeginTransaction())
            {
                var colaborador = await _context.Colaboradores.Include(x=>x.IdentityUser).Where(x=>x.Id == id).FirstOrDefaultAsync();
                if (colaborador == null)
                {
                    transaction.Rollback();
                    response.success = false;
                    response.response = "El registro no existe";
                    return Ok(response);
                }
                try
                {
                    var IUser = await _userManager.FindByEmailAsync(colaborador.IdentityUser.Email);
                    string emailU = colaborador.IdentityUser.Email;
                    string nombreUsuarioU = $"{colaborador.Nombres} {colaborador.Apellidos}";
                    var Delete = await _userManager.DeleteAsync(IUser);
                    if (Delete.Succeeded)
                    {


                        _context.Colaboradores.Remove(colaborador);
                        _context.Logger.Add(new Log()
                        {
                            Created = DateTime.Now,
                            User = "ADMIN",
                            Id_User = 1.ToString(),
                            Accion = ETipoAccionS.GetString(ETipoAccion.DELETECOLABORADOR),
                            Description = ETipoAccionS.GetString(ETipoAccion.DELETECOLABORADOR) + " Con CURP:" + colaborador.CURP + " Por ADMIN",
                        });
                    }
                    else
                    {
                        transaction.Rollback();
                        response.success = false;
                        response.response = $"Error al eliminar el usuario";
                        return Ok(response);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    string messageU = $"<p>Por este medio confirmamos la eliminación de su cuenta del sistema Plenumsoft</p>";

                    //var Link = Url.PageLink().Split('/');
                    //string url = Link[0] + "//" + Link[2];
                    try { 
                        await _emailSender.SendEmailAsync(emailU, "Baja de usuario - SAP Plenumsoft", nombreUsuarioU, messageU, "").ConfigureAwait(false);
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
                    response.response = $"Error al eliminar el registro";
                    return Ok(response);
                }
            }
            response.success = true;
            response.response = "Registro eliminado con éxito";
            return Ok(response);
        }

        private bool ColaboradorExists(int id)
        {
            return _context.Colaboradores.Any(e => e.Id == id);
        }
    }
}

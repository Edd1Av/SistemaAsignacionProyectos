using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB.Data;
using WEB.Models;
using WEB.ViewModels;

namespace WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProyectosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Proyectos
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Proyecto>>> GetProyectos()
        {
            return await _context.Proyectos.OrderBy(x=>x.Titulo).ToListAsync();
        }

        [HttpGet]
        [Authorize]
        [Route("proyectosColaborador/{id}")]
        public async Task<ActionResult<IEnumerable<Proyecto>>> GetProyectos(int id)
        {
            //var x = _context.Asignacion.Select(x => x.Distribuciones.Where(y => y.Asignacion.IdColaborador == id).Select(x=>x.Proyecto)).ToList();

            var y = await _context.Distribucion.Where(x => x.Asignacion.IdColaborador == id).Select(y=>y.Proyecto).ToListAsync();
            
            return y; 
        }

        // GET: api/Proyectos/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Proyecto>> GetProyecto(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);

            if (proyecto == null)
            {
                return NotFound();
            }

            return Ok(proyecto);
        }

        // PUT: api/Proyectos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProyecto(int id, Proyecto proyecto)
        {
            Response response = new Response();
            if (id != proyecto.Id)
            {
                response.success = false;
                response.response = "Los ID no coinciden";
                return Ok(response);
            }

            try
            {
                if (_context.Proyectos.Any(x => x.Clave.ToUpper().Trim() == proyecto.Clave.ToUpper().Trim() && x.Id != proyecto.Id))
                {
                    response.success = false;
                    response.response = $"Ya existe un proyecto con esa clave Odoo";
                    return Ok(response);
                }

                Proyecto addProyecto = new Proyecto();
                addProyecto.Id = proyecto.Id;
                addProyecto.Titulo = proyecto.Titulo.Trim();
                addProyecto.Clave = proyecto.Clave.ToUpper().Trim();

                _context.Entry(addProyecto).State = EntityState.Modified;

                _context.Logger.Add(new Log()
                {
                    Created = DateTime.Now,
                    User = "ADMIN",
                    Id_User = 1.ToString(),
                    Accion = ETipoAccionS.GetString(ETipoAccion.UPDATEPROYECTO),
                    Description = ETipoAccionS.GetString(ETipoAccion.UPDATEPROYECTO) + " " + proyecto.Titulo + " Por ADMIN",
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!ProyectoExists(id))
                {
                    response.success = false;
                    response.response = "No existe un proyecto con ese ID";
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

        // POST: api/Proyectos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Proyecto>> PostProyecto(Proyecto proyecto)
        {
            Response response = new Response();
            try
            {
                if (_context.Proyectos.Any(x => x.Clave.ToUpper().Trim() == proyecto.Clave.ToUpper().Trim()))
                {
                    response.success = false;
                    response.response = $"Ya existe un proyecto con esa clave Odoo";
                    return Ok(response);
                }

                Proyecto addProyecto = new Proyecto();
                addProyecto.Titulo = proyecto.Titulo.Trim();
                addProyecto.Clave = proyecto.Clave.ToUpper().Trim();

                _context.Proyectos.Add(addProyecto);

                _context.Logger.Add(new Log()
                {
                    Created = DateTime.Now,
                    User = "ADMIN",
                    Id_User = 1.ToString(),
                    Accion = ETipoAccionS.GetString(ETipoAccion.ADDPROYECTO),
                    Description= ETipoAccionS.GetString(ETipoAccion.ADDPROYECTO) + " " + proyecto.Titulo + " Por ADMIN",
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                response.success = false;
                response.response = $"Error al registrar";
                return Ok(response);
            }

            response.success = true;
            response.response = "Registrado con éxito";
            return Ok(response);
        }

        // DELETE: api/Proyectos/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProyecto(int id)
        {
            Response response = new Response();
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null)
            {
                response.success = false;
                response.response = "El registro no existe";
                return Ok(response);
            }
            try
            {
                _context.Proyectos.Remove(proyecto);

                _context.Logger.Add(new Log()
                {
                    Created = DateTime.Now,
                    User = "ADMIN",
                    Id_User = 1.ToString(),
                    Accion = ETipoAccionS.GetString(ETipoAccion.DELETEPROYECTO),
                    Description = ETipoAccionS.GetString(ETipoAccion.DELETEPROYECTO) + " " + proyecto.Titulo+" Por ADMIN",
                });
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                response.success = false;
                response.response = $"Error al eliminar el registro";
                return Ok(response);
            }
            response.success = true;
            response.response = "Registro eliminado con éxito";
            return Ok(response);
        }

        private bool ProyectoExists(int id)
        {
            return _context.Proyectos.Any(e => e.Id == id);
        }
    }
}

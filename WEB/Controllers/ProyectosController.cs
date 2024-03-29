﻿using System;
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
        public async Task<ActionResult<IEnumerable<ProyectosR>>> GetProyectos()
        {
            List<Proyecto> proyectos = await _context.Proyectos.Where(x=>x.IsActive==true).OrderBy(x => x.Titulo).ToListAsync();
            List<ProyectosR> result = new List<ProyectosR>();
            foreach(var item in proyectos)
            {
                List<ColaboradoresAsignados> CA=new List<ColaboradoresAsignados>();
                List<Distribucion> Dis=_context.Distribucion.Include(x=>x.Asignacion).ThenInclude(x=>x.Colaborador).
                    Where(x=>x.Fecha_Final.Date>=DateTime.Now.Date&&x.Fecha_Inicio<=DateTime.Now.Date&&x.IdProyecto==item.Id).ToList();
                foreach(var i in Dis)
                {
                    CA.Add(new ColaboradoresAsignados
                    {
                        Nombres=i.Asignacion.Colaborador.Nombres,
                        Apellidos=i.Asignacion.Colaborador.Apellidos,
                        ClaveOdoo=i.Asignacion.Colaborador.Id_Odoo
                    }
                    );
                }
                result.Add(new ProyectosR 
                { 
                    id=item.Id,
                    titulo = item.Titulo,
                    clave = item.Clave,
                    ColaboradoresAsignados=CA.Count>0?CA:null
                }
                );

            }
            return result;
        }

        [HttpPost]
        [Authorize]
        [Route("proyectosColaborador/{id}")]
        public async Task<ActionResult<IEnumerable<Proyecto>>> GetProyectos(int id, IntervaloFechas intervalo)
        {
            //var x = _context.Asignacion.Select(x => x.Distribuciones.Where(y => y.Asignacion.IdColaborador == id).Select(x=>x.Proyecto)).ToList();

            var y = await _context.Distribucion.Include(x => x.Proyecto).Where(x => x.Asignacion.IdColaborador == id &&x.Proyecto.IsActive==true && 
            (((x.Fecha_Final.Date >= intervalo.FechaInicio.Date && x.Fecha_Final.Date <= intervalo.FechaFin.Date) ||
             (x.Fecha_Inicio.Date <= intervalo.FechaFin.Date && x.Fecha_Inicio.Date >= intervalo.FechaInicio.Date)) ||
             ((intervalo.FechaFin.Date >= x.Fecha_Inicio.Date && intervalo.FechaFin.Date <= x.Fecha_Final.Date) ||
             (intervalo.FechaInicio.Date <= x.Fecha_Final.Date && intervalo.FechaInicio.Date >= x.Fecha_Inicio.Date)))).Select(y => y.Proyecto).ToListAsync();
            //(intervalo.FechaInicio.Date.CompareTo(x.Fecha_Inicio.Date) >= 0 && intervalo.FechaInicio.Date.CompareTo(x.Fecha_Final.Date) <= 0) && 
            //(intervalo.FechaFin.Date.CompareTo(x.Fecha_Inicio.Date) >= 0 && intervalo.FechaFin.Date.CompareTo(x.Fecha_Final.Date) <= 0)).Select(y=>y.Proyecto).ToListAsync();



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
        public async Task<IActionResult> PutProyecto(int id, ProyectosPost proyecto)
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
                addProyecto.Id = (int)proyecto.Id;
                addProyecto.Titulo = proyecto.Titulo.Trim();
                addProyecto.Clave = proyecto.Clave.ToUpper().Trim();
                addProyecto.IsActive = true;
                _context.Entry(addProyecto).State = EntityState.Modified;

                _context.Logger.Add(new Log()
                {
                    Created = DateTime.Now,
                    User = proyecto.User,
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
        public async Task<ActionResult<Proyecto>> PostProyecto(ProyectosPost proyecto)
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
                addProyecto.IsActive = true;
                _context.Proyectos.Add(addProyecto);

                _context.Logger.Add(new Log()
                {
                    Created = DateTime.Now,
                    User = proyecto.User,
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
        [HttpPost]
        [Route("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteProyecto(Delete postModel)
        {
            Response response = new Response();
            
            try
            {
                var proyecto = await _context.Proyectos.FindAsync(postModel.Id);
                if (proyecto == null)
                {
                    response.success = false;
                    response.response = "El registro no existe";
                    return Ok(response);
                }
                else
                {
                    proyecto.IsActive = false;

                    _context.Entry(proyecto).State = EntityState.Modified;
                    //_context.Proyectos.Remove(proyecto);

                    _context.Logger.Add(new Log()
                    {
                        Created = DateTime.Now,
                        User = postModel.User,
                        Accion = ETipoAccionS.GetString(ETipoAccion.DELETEPROYECTO),
                        Description = ETipoAccionS.GetString(ETipoAccion.DELETEPROYECTO) + " " + proyecto.Titulo + " Por ADMIN",
                    });
                    await _context.SaveChangesAsync();
                }

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

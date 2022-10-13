using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class AsignacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AsignacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Asignaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AsignacionHistorico>>> GetAsignacion()
        {
            var asignaciones =  _context.Asignacion.Include(x=>x.Colaborador).Include(x=>x.Distribuciones).ToList();
            List <AsignacionHistorico> asignacionHistorico = new List<AsignacionHistorico>();
            foreach(var element in asignaciones)
            {
                List<Proyecto> proyectos = new List<Proyecto>();
                foreach(var item in element.Distribuciones)
                {
                    proyectos.Add(_context.Proyectos.Where(x => x.Id == item.IdProyecto).FirstOrDefault());
                }
                asignacionHistorico.Add(new AsignacionHistorico
                {
                    Id = element.Id,
                    Colaborador = element.Colaborador,
                    Fecha_inicio = element.Fecha_Inicio,
                    Fecha_final = element.Fecha_Final,
                    Fecha_inicio_s = element.Fecha_Inicio.ToShortDateString(),
                    Fecha_final_s = element.Fecha_Final.ToShortDateString(),
                    Distribucion = element.Distribuciones,
                    Proyectos = string.Join(",", proyectos.Select(x => x.Titulo).ToList())
                }) ;
            }
            return (asignacionHistorico);
        }

        // GET: api/Asignaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Asignacion>> GetAsignacion(int id)
        {
            var asignacion = await _context.Asignacion.FindAsync(id);

            if (asignacion == null)
            {
                return NotFound();
            }

            return asignacion;
        }

        // PUT: api/Asignaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsignacion(int id, AsignacionPost postModel)
        {
            Response response = new Response();
            try
            {

                Asignacion asignacion = _context.Asignacion.Where(x => x.Id == id).FirstOrDefault();
                var colaborador = new Colaborador();
                colaborador = _context.Colaboradores.Where(x => x.Id == postModel.Id_Colaborador).FirstOrDefault();

                var distribucion = new List<Distribucion>();
                asignacion.Fecha_Inicio = postModel.Fecha_Inicio;
                asignacion.Fecha_Final = postModel.Fecha_Final;


                foreach (var item in postModel.Proyectos)
                {
                    var proyecto = new Proyecto();
                    proyecto = _context.Proyectos.Where(x => x.Id == item.Id).FirstOrDefault();

                    distribucion.Add(new Distribucion()
                    {
                        Proyecto = proyecto,
                        Porcentaje = item.Porcentaje
                    });
                }

                _context.Distribucion.RemoveRange(asignacion.Distribuciones);
                asignacion.Distribuciones = distribucion;

                _context.Entry(asignacion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.response = $"Error al modificar";
                return Ok(response);
            }

            response.success = true;
            response.response = "Modificado con éxito";
            return Ok(response);
        }

        // POST: api/Asignaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Asignacion>> PostAsignacion(AsignacionPost postModel)
        {
            Response response = new Response();
            try
            {
                var colaborador = new Colaborador();
                colaborador = _context.Colaboradores.Where(x => x.Id == postModel.Id_Colaborador).FirstOrDefault();

                var asignacion = new Asignacion();
                var distribucion = new List<Distribucion>();
                asignacion.Fecha_Inicio = postModel.Fecha_Inicio;
                asignacion.Fecha_Final = postModel.Fecha_Final;


                foreach (var item in postModel.Proyectos)
                {
                    var proyecto = new Proyecto();
                    proyecto= _context.Proyectos.Where(x => x.Id==item.Id).FirstOrDefault();

                    distribucion.Add(new Distribucion()
                    {
                        Proyecto = proyecto,
                        Porcentaje = item.Porcentaje
                    }); ;
                }

                asignacion.Distribuciones = distribucion;

                asignacion.Colaborador = colaborador;
                _context.Asignacion.Add(asignacion);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                response.success = false;
                response.response = $"Error al registrar";
                return Ok(response);
            }

            response.success = true;
            response.response = "Registrado con éxito";
            return Ok(response);
        }

        // DELETE: api/Asignaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsignacion(int id)
        {
            Response response = new Response();
            try
            {

                var asignacion = await _context.Asignacion.FindAsync(id);
                if (asignacion == null)
                {
                    response.success = false;
                    response.response = $"Error al eliminar";
                    return Ok(response);
                }

                _context.Asignacion.Remove(asignacion);
                await _context.SaveChangesAsync();

                response.success = true;
                response.response = "Eliminado con éxito";
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.response = $"Error al eliminar";
                return Ok(response);
            }

        }

        private bool AsignacionExists(int id)
        {
            return _context.Asignacion.Any(e => e.Id == id);
        }
    }
}

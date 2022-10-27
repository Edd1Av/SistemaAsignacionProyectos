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
    public class AsignacionRealController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AsignacionRealController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Asignaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AsignacionHistoricoReal>>> GetAsignacion()
        {
            //var asignaciones =  _context.Asignacion.Include(x=>x.Colaborador).Include(x=>x.Distribuciones).ToList();
            var asignaciones = _context.AsignacionReal
                                           .Include(x => x.Asignacion)
                                               .ThenInclude(z=>z.Colaborador)
                                           .Include(i => i.DistribucionesReales)
                                               .ThenInclude(y => y.Proyecto)
                                           .ToList();

            List<AsignacionHistoricoReal> asignacionHistoricoReal = new List<AsignacionHistoricoReal>();
            foreach (var element in asignaciones)
            {
                asignacionHistoricoReal.Add(new AsignacionHistoricoReal
                {
                    Id = element.Id,
                    Colaborador = element.Asignacion.Colaborador,
                    Fecha_inicio = element.Fecha_Inicio,
                    Fecha_final = element.Fecha_Final,
                    Fecha_inicio_s = element.Fecha_Inicio.ToShortDateString(),
                    Fecha_final_s = element.Fecha_Final.ToShortDateString(),
                    Distribucion = element.DistribucionesReales,
                    Proyectos = string.Join(", ", element.DistribucionesReales.Select(x => x.Proyecto.Titulo).ToList())
                });
            }
            return (asignacionHistoricoReal);
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

        [Route("ByColaborador/{id}")]
        [HttpGet]
        public async Task<ActionResult<AsignacionReal>> GetAsignacionByColaborador(int id)
        {
            var asignacion = _context.AsignacionReal
                                               .Include(x => x.Asignacion)
                                               .ThenInclude(i=>i.Colaborador)
                                                .Include(z => z.DistribucionesReales)
                                               .ThenInclude(y => y.Proyecto).Where(x => x.Asignacion.IdColaborador == id).FirstOrDefault();

            if (asignacion == null)
            {
                return NotFound();
            }

            return asignacion;
        }

        // PUT: api/Asignaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsignacion(int id, AsignacionPostReal postModel)
        {
            Response response = new Response();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
 
                    var asignacionPlaneada = _context.Asignacion.Include(z => z.Colaborador)
                    .Include(i => i.AsignacionReal)
                    .ThenInclude(c => c.DistribucionesReales)
                    .ThenInclude(v => v.Proyecto)
                    .Where(x => x.IdColaborador == postModel.Id_Colaborador).First();

                    var asignacionReal = asignacionPlaneada.AsignacionReal.Where(x=>x.Id == id).First();

                    _context.DistribucionReal.RemoveRange(asignacionReal.DistribucionesReales);

                    asignacionReal.DistribucionesReales.Clear();


                    foreach (var item in postModel.Proyectos)
                    {
                        var proyecto = new Proyecto();
                        proyecto = _context.Proyectos.Where(x => x.Id == item.Id).First();

                        asignacionReal.DistribucionesReales.Add(new DistribucionReal()
                        {
                            //Fecha_Inicio = item.Fecha_inicio,
                            //Fecha_Final = item.Fecha_final,
                            Proyecto = proyecto,
                            Porcentaje = item.Porcentaje
                        });
                    }

                    asignacionReal.Fecha_Inicio = postModel.Fecha_Inicio.ToLocalTime();
                    asignacionReal.Fecha_Final = postModel.Fecha_Final.ToLocalTime();

                    _context.AsignacionReal.Attach(asignacionReal);
                    _context.Entry(asignacionReal).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.success = false;
                    response.response = $"Error al modificar";
                    return Ok(response);
                }
            }

            response.success = true;
            response.response = "Modificado con éxito";
            return Ok(response);
        }

        // POST: api/Asignaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AsignacionReal>> PostAsignacion(AsignacionPostReal postModel)
        {
            Response response = new Response();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var asignacionPlaneada = _context.Asignacion.Include(z => z.Colaborador)
                        .Include(i => i.AsignacionReal)
                        .ThenInclude(c => c.DistribucionesReales)
                        .ThenInclude(v => v.Proyecto)
                        .Where(x => x.IdColaborador == postModel.Id_Colaborador).First();
                    var asignacionReal = new AsignacionReal();
                    var distribucionReal = new List<DistribucionReal>();



                    foreach (var item in postModel.Proyectos)
                    {
                        var proyecto = new Proyecto();
                        proyecto = _context.Proyectos.Where(x => x.Id == item.Id).First();

                        distribucionReal.Add(new DistribucionReal()
                        {
                            //Fecha_Inicio = item.Fecha_inicio,
                            //Fecha_Final = item.Fecha_final,
                            Proyecto = proyecto,
                            Porcentaje = item.Porcentaje
                        });
                    }

                    asignacionReal.DistribucionesReales = distribucionReal;
                    asignacionReal.Fecha_Inicio = postModel.Fecha_Inicio.ToLocalTime();
                    asignacionReal.Fecha_Final = postModel.Fecha_Final.ToLocalTime();
                    asignacionPlaneada.AsignacionReal = new List<AsignacionReal>();
                    asignacionPlaneada.AsignacionReal.Add(asignacionReal);


                    _context.Asignacion.Attach(asignacionPlaneada);
                    _context.Entry(asignacionPlaneada).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
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

        // DELETE: api/Asignaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsignacion(int id)
        {
            Response response = new Response();
            try
            {

                var asignacion = await _context.AsignacionReal.FindAsync(id);
                if (asignacion == null)
                {
                    response.success = false;
                    response.response = $"Error al eliminar";
                    return Ok(response);
                }

                _context.AsignacionReal.Remove(asignacion);
                await _context.SaveChangesAsync();

                response.success = true;
                response.response = "Eliminado con éxito";
                return Ok(response);
            }
            catch (Exception ex)
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

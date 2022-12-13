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
    public class AsignacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AsignacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Asignaciones
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AsignacionHistorico>>> GetAsignacion()
        {
            //var asignaciones =  _context.Asignacion.Include(x=>x.Colaborador).Include(x=>x.Distribuciones).ToList();
            var asignaciones = _context.Asignacion
                                           .Include(x => x.Colaborador)
                                           .Include(x => x.Distribuciones)
                                               .ThenInclude(y => y.Proyecto)
                                           .ToList();

            List <AsignacionHistorico> asignacionHistorico = new List<AsignacionHistorico>();
            foreach(var element in asignaciones)
            {
                asignacionHistorico.Add(new AsignacionHistorico
                {
                    Id = element.Id,
                    Colaborador = element.Colaborador,
                    //Fecha_inicio = element.Fecha_Inicio,
                    //Fecha_final = element.Fecha_Final,
                    //Fecha_inicio_s = element.Fecha_Inicio.ToShortDateString(),
                    //Fecha_final_s = element.Fecha_Final.ToShortDateString(),
                    Distribucion = element.Distribuciones,
                    Proyectos = string.Join(", ", element.Distribuciones.Select(x=>x.Proyecto.Titulo).ToList())
                }) ;
            }
            return (asignacionHistorico);
        }

        // GET: api/Asignaciones/5
        [HttpGet("{id}")]
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<Asignacion>> GetAsignacionByColaborador(int id)
        {
            var asignacion = _context.Asignacion.Include(x => x.Colaborador)
                                           .Include(x => x.Distribuciones)
                                               .ThenInclude(y => y.Proyecto).Where(x => x.IdColaborador == id).FirstOrDefault();

            if (asignacion == null)
            {
                return NotFound();
            }

            return asignacion;
        }

        // PUT: api/Asignaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAsignacion(int id, AsignacionPost postModel)
        {
            Response response = new Response();
            if (postModel.Proyectos.Count == 0)
            {
                response.success = false;
                response.response = $"Seleccione algún proyecto";
                return Ok(response);
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Asignacion asignacion = _context.Asignacion
                                            .Include(x=>x.Colaborador)
                                            .Include(x=>x.Distribuciones)
                                                .ThenInclude(y=>y.Proyecto)
                                            .Where(x => x.Id == id).First();

                    //asignacion.Fecha_Inicio = postModel.Fecha_Inicio;
                    //asignacion.Fecha_Final = postModel.Fecha_Final;
                    //var Asignaciones = new List<Asignacion>();
                    //Asignaciones = _context.Asignacion.Include(c => c.Distribuciones).Where(x => x.IdColaborador == postModel.Id_Colaborador).ToList();
                    //foreach (var item in postModel.Proyectos)
                    //{
                    //    var proyecto = new Proyecto();
                    //    proyecto = _context.Proyectos.Where(x => x.Id == item.Id).First();
                    //    foreach (var z in Asignaciones)
                    //    {
                    //        foreach (var y in z.Distribuciones)
                    //        {
                    //            if (y.IdProyecto == item.Id && ((y.Fecha_Final.ToLocalTime().Date >= item.Fecha_final.ToLocalTime().Date && y.Fecha_Inicio.ToLocalTime().Date <= item.Fecha_inicio.ToLocalTime().Date) ||
                    //                (y.Fecha_Final.ToLocalTime().Date >= item.Fecha_inicio.ToLocalTime().Date || y.Fecha_Inicio.ToLocalTime().Date >= item.Fecha_final.ToLocalTime().Date)))
                    //            {
                    //                response.success = false;

                    //                transaction.Rollback();
                    //                response.response = $"Error al modificar, existe un registro de proyecto en ese intervalo de fechas";
                    //                return Ok(response);
                    //            }
                    //        }
                    //    }
                    //}

                    _context.Distribucion.RemoveRange(asignacion.Distribuciones);

                    asignacion.Distribuciones.Clear();


                    foreach (var item in postModel.Proyectos)
                    {
                        var proyecto = new Proyecto();
                        proyecto = _context.Proyectos.Where(x => x.Id == item.Id).First();

                        asignacion.Distribuciones.Add(new Distribucion()
                        {
                            Fecha_Inicio = item.Fecha_inicio,
                            Fecha_Final = item.Fecha_final,
                            Proyecto = proyecto,
                            //Porcentaje = item.Porcentaje
                        });
                    }

                    _context.Asignacion.Attach(asignacion);
                    _context.Entry(asignacion).State = EntityState.Modified;
                    _context.Logger.Add(new Log()
                    {
                        Created = DateTime.Now,
                        User = postModel.User,
                        Accion = ETipoAccionS.GetString(ETipoAccion.UPDATEASIGNACIONPLANEADA),
                        Description= ETipoAccionS.GetString(ETipoAccion.UPDATEASIGNACIONPLANEADA)+ " Con ID=" + asignacion.Id+ " De Colaborador con CURP " +asignacion.Colaborador.CURP,
                    });
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
        [Authorize]
        public async Task<ActionResult<Asignacion>> PostAsignacion(AsignacionPost postModel)
        {
            Response response = new Response();
            if (postModel.Proyectos.Count == 0)
            {
                response.success = false;
                response.response = $"Seleccione algún proyecto";
                return Ok(response);
            }
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var colaborador = new Colaborador();
                    colaborador = _context.Colaboradores.Include(x=>x.Asignacion).Where(x => x.Id == postModel.Id_Colaborador).First();

                    if(colaborador.Asignacion != null)
                    {
                        transaction.Rollback();
                        response.success = false;
                        response.response = $"El colaborador ya cuenta con una asignación";
                        return Ok(response);
                    }

                    var asignacion = new Asignacion();
                    var distribucion = new List<Distribucion>();
                    //asignacion.Fecha_Inicio = postModel.Fecha_Inicio;
                    //asignacion.Fecha_Final = postModel.Fecha_Final;


                    foreach (var item in postModel.Proyectos)
                    {
                        var proyecto = new Proyecto();
                        proyecto = _context.Proyectos.Where(x => x.Id == item.Id).FirstOrDefault();
                        //var Asignaciones = new List<Asignacion>();
                        //Asignaciones = _context.Asignacion.Include(c=>c.Distribuciones).Where(x => x.IdColaborador == postModel.Id_Colaborador).ToList();
                        //foreach(var z in Asignaciones)
                        //{
                        //    foreach(var y in z.Distribuciones)
                        //    {
                        //        if (y.IdProyecto == item.Id && ((y.Fecha_Final.ToLocalTime().Date >= item.Fecha_final.ToLocalTime().Date && y.Fecha_Inicio.ToLocalTime().Date <= item.Fecha_inicio.ToLocalTime().Date)||
                        //            (y.Fecha_Final.ToLocalTime().Date >= item.Fecha_inicio.ToLocalTime().Date || y.Fecha_Inicio.ToLocalTime().Date >= item.Fecha_final.ToLocalTime().Date)))
                        //        {
                        //            response.success = false;

                        //            transaction.Rollback();
                        //            response.response = $"Error al registrar, existe un registro de proyecto en ese intervalo de fechas";
                        //            return Ok(response);
                        //        }
                        //    }
                        //}


                        distribucion.Add(new Distribucion()
                        {
                            Fecha_Inicio = item.Fecha_inicio,
                            Fecha_Final = item.Fecha_final,
                            Proyecto = proyecto,
                            //Porcentaje = item.Porcentaje
                        }); ;
                    }

                    asignacion.Distribuciones = distribucion;

                    asignacion.Colaborador = colaborador;
                    _context.Asignacion.Add(asignacion);
                    _context.Logger.Add(new Log()
                    {
                        Created = DateTime.Now,
                        User = postModel.User,
                        Accion = ETipoAccionS.GetString(ETipoAccion.ADDASIGNACIONPLANEADA),
                        Description= ETipoAccionS.GetString(ETipoAccion.ADDASIGNACIONPLANEADA) + " De Colaborador con CURP " + asignacion.Colaborador.CURP
                    });
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

        [HttpPost]
        [Route("delete")]
        [Authorize]
        public async Task<ActionResult<Asignacion>> DeleteAsignacion(Delete postModel)
        {
            Response response = new Response();
            try
            {

                var asignacion = _context.Asignacion.Include(x => x.Colaborador).Where(y => y.Id == postModel.Id).FirstOrDefault();
                if (asignacion == null)
                {
                    response.success = false;
                    response.response = $"Error al eliminar";
                    return Ok(response);
                }

                _context.Asignacion.Remove(asignacion);
                _context.Logger.Add(new Log()
                {
                    Created = DateTime.Now,
                    User = postModel.User,
                    Accion = ETipoAccionS.GetString(ETipoAccion.DELETEASIGNACIONPLANEADA),
                    Description= ETipoAccionS.GetString(ETipoAccion.DELETEASIGNACIONPLANEADA) + " Con ID=" + asignacion.Id + " De Colaborador con CURP " + asignacion.Colaborador.CURP
                });
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

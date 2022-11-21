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
    //[Authorize]
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

        //[Route("ByColaborador/{id}")]
        //[HttpGet]
        //public async Task<ActionResult<AsignacionReal>> GetAsignacionByColaborador(int id)
        //{
        //    var asignacion = _context.AsignacionReal
        //                                       .Include(x => x.Asignacion)
        //                                       .ThenInclude(i=>i.Colaborador)
        //                                        .Include(z => z.DistribucionesReales)
        //                                       .ThenInclude(y => y.Proyecto).Where(x => x.Asignacion.IdColaborador == id).FirstOrDefault();

        //    if (asignacion == null)
        //    {
        //        return NotFound();
        //    }

        //    return asignacion;
        //}

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

                    asignacionReal.Fecha_Inicio = postModel.Fecha_Inicio.ToLocalTime().Date;
                    asignacionReal.Fecha_Final = postModel.Fecha_Final.ToLocalTime().Date;

                    _context.AsignacionReal.Attach(asignacionReal);
                    _context.Entry(asignacionReal).State = EntityState.Modified;
                    _context.Logger.Add(new Log()
                    {
                        Created = DateTime.Now,
                        User = asignacionPlaneada.Colaborador.Nombres+" "+asignacionPlaneada.Colaborador.Apellidos,
                        Id_User = asignacionPlaneada.IdColaborador.ToString(),
                        Accion = ETipoAccionS.GetString(ETipoAccion.UPDATEASIGNACIONPLANEADA),
                        Description= ETipoAccionS.GetString(ETipoAccion.UPDATEASIGNACIONPLANEADA) + " Con ID=" + asignacionReal.Id + " De Colaborador " + asignacionPlaneada.Colaborador.CURP,
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

                    var AsignacionesReales = new List<AsignacionReal>();
                    AsignacionesReales = _context.AsignacionReal.Include(c => c.DistribucionesReales).Include(p=>p.Asignacion).Where(x => x.Asignacion.IdColaborador == postModel.Id_Colaborador).ToList();
                    
                    foreach(var item in postModel.Proyectos)
                    {
                        foreach (var x in AsignacionesReales)
                        {
                            foreach (var y in x.DistribucionesReales)
                            {
                                if ((x.Fecha_Final.Date >= postModel.Fecha_Inicio.Date &&
                                                     x.Fecha_Final.Date <= postModel.Fecha_Final.Date)
                                                        || (x.Fecha_Inicio.Date <= postModel.Fecha_Final.Date &&
                                                        x.Fecha_Inicio.Date >= postModel.Fecha_Inicio.Date))
                                {
                                    response.success = false;


                                    response.response = $"Error al registrar, existe un registro de proyecto en ese intervalo de fechas";
                                    return Ok(response);
                                }
                            }
                        }
                    }


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
                    asignacionReal.Fecha_Inicio = postModel.Fecha_Inicio.ToLocalTime().Date;
                    asignacionReal.Fecha_Final = postModel.Fecha_Final.ToLocalTime().Date;
                    //asignacionPlaneada.AsignacionReal = new List<AsignacionReal>();
                    asignacionPlaneada.AsignacionReal.Add(asignacionReal);


                    _context.Asignacion.Attach(asignacionPlaneada);
                    _context.Entry(asignacionPlaneada).State = EntityState.Modified;
                    _context.Logger.Add(new Log()
                    {
                        Created = DateTime.Now,
                        User = asignacionPlaneada.Colaborador.Nombres + " " + asignacionPlaneada.Colaborador.Apellidos,
                        Id_User = asignacionPlaneada.IdColaborador.ToString(),
                        Accion = ETipoAccionS.GetString(ETipoAccion.ADDASIGNACIONREAL),
                        Description=ETipoAccionS.GetString(ETipoAccion.ADDASIGNACIONREAL) + " Con ID=" + asignacionReal.Id + " De Colaborador " + asignacionPlaneada.Colaborador.CURP,
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsignacion(int id)
        {
            Response response = new Response();
            try
            {

                var asignacionReal =  _context.AsignacionReal.Include(x=>x.Asignacion).ThenInclude(y=>y.Colaborador).Where(z=>z.Id==id).FirstOrDefault();
                if (asignacionReal == null)
                {
                    response.success = false;
                    response.response = $"Error al eliminar";
                    return Ok(response);
                }

                _context.AsignacionReal.Remove(asignacionReal);

                _context.Logger.Add(new Log()
                {
                    Created = DateTime.Now,
                    User = asignacionReal.Asignacion.Colaborador.Nombres + " " + asignacionReal.Asignacion.Colaborador.Apellidos,
                    Id_User = asignacionReal.Asignacion.IdColaborador.ToString(),
                    Accion = ETipoAccionS.GetString(ETipoAccion.DELETEASIGNACIONREAL) + " Con ID=" + asignacionReal.Id + " De Colaborador " + asignacionReal.Asignacion.Colaborador.CURP,
                    Description = ""
                }) ;
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


        public static int DaysLeft(DateTime startDate, DateTime endDate, Boolean excludeWeekends, List<DateTime> excludeDates)
        {
            int count = 0;
            for (DateTime index = startDate; index < endDate; index = index.AddDays(1))
            {
                if (excludeWeekends && index.DayOfWeek != DayOfWeek.Sunday && index.DayOfWeek != DayOfWeek.Saturday)
                {
                    bool excluded = false; ;
                    for (int i = 0; i < excludeDates.Count; i++)
                    {
                        if (index.Date.CompareTo(excludeDates[i].Date) == 0)
                        {
                            excluded = true;
                            break;
                        }
                    }

                    if (!excluded)
                    {
                        count++;
                    }
                }
            }

            return count;
        }



        [HttpPost]
        [Route("Historico")]
        public async Task<ActionResult<AsignacionReal>> Historico(Fechas postModel)
        {
            Response response = new Response();
            try
            {
                var Asignaciones = new List<AsignacionReal>();
                if (postModel.Fecha_Inicio != null&postModel.Fecha_Final!=null)
                {
                    Asignaciones = _context.AsignacionReal
                                                   .Include(x => x.Asignacion)
                                                       .ThenInclude(z => z.Colaborador)
                                                   .Include(i => i.DistribucionesReales)
                                                       .ThenInclude(y => y.Proyecto).
                                                     Where(x => (x.Fecha_Final.Date >= postModel.Fecha_Inicio.Date&&
                                                     x.Fecha_Final.Date <= postModel.Fecha_Final.Date)
                                                        || (x.Fecha_Inicio.Date <= postModel.Fecha_Final.Date&&
                                                        x.Fecha_Inicio.Date >= postModel.Fecha_Inicio.Date))
                                                   .ToList();
                }
                else
                {
                    Asignaciones = _context.AsignacionReal
                                                   .Include(x => x.Asignacion)
                                                       .ThenInclude(z => z.Colaborador)
                                                   .Include(i => i.DistribucionesReales)
                                                       .ThenInclude(y => y.Proyecto)
                                                   .ToList();
            }

                var rest = new List<dynamic>();
                var Colaboradores = _context.Colaboradores.Where(x=>x.Id!=1).ToList();
                foreach (var colaborador in Colaboradores)
                {
                    var proyectos = new List<HistoricoResponse>();
                    var Lista = Asignaciones.Where(x => x.Asignacion.IdColaborador == colaborador.Id).ToList();
                    foreach(var L in Lista)
                    {
                        foreach (var P in L.DistribucionesReales)
                        {

                            int difFechas = DaysLeft(L.Fecha_Inicio.Date < postModel.Fecha_Inicio.Date ? postModel.Fecha_Inicio.Date : L.Fecha_Inicio.Date,
                                L.Fecha_Final.Date.AddDays(1)>postModel.Fecha_Final.Date? postModel.Fecha_Final.Date.AddDays(1) : L.Fecha_Final.Date.AddDays(1), true,new List<DateTime>()); 
                        if (proyectos.Where(x => x.id == P.IdProyecto).ToList().Count > 0)
                            {
                                proyectos.Where(x => x.id == P.IdProyecto).FirstOrDefault().value += P.Porcentaje * (difFechas);
                                proyectos.Where(x => x.id == P.IdProyecto).FirstOrDefault().dias += (difFechas)*(P.Porcentaje/100);
                            }
                            else
                            {

                                proyectos.Add(new HistoricoResponse
                                {
                                    id = P.IdProyecto,
                                    titulo=P.Proyecto.Titulo,
                                    value = P.Porcentaje *(difFechas),
                                    dias= ((double)P.Porcentaje/(double)100) * (double)difFechas
                                });
                            }
                        }

                    }
                var sum = proyectos.Sum(x => x.value);
                var diferencia=0.0;
                for (int i = 0; i < proyectos.Count; i++)
                {
                    var porcentaje = (int)(((double)proyectos[i].value / (double)sum) * 100);
                    diferencia += (((double)proyectos[i].value / (double)sum) * 100)- (int)(((double)proyectos[i].value / (double)sum) * 100);
                    if (i == proyectos.Count - 1)
                    {
                        proyectos[i].porcentaje = Math.Ceiling(porcentaje+diferencia);
                    }
                    else
                    {
                        proyectos[i].porcentaje = Math.Ceiling((double)porcentaje);
                    }

                }

                    rest.Add(new
                    {
                        colaborador = colaborador.Nombres + " " + colaborador.Apellidos,
                        asignaciones = proyectos,
                        diasTrabajados=proyectos.Sum(x=>x.dias),
                        complete= ((double)proyectos.Sum(x => x.dias)/ (double)DaysLeft(postModel.Fecha_Inicio.Date, postModel.Fecha_Final.Date.AddDays(1), true, new List<DateTime>()))*100
                    });

                }

                dynamic datos = new System.Dynamic.ExpandoObject();
                datos.diastotales = DaysLeft(postModel.Fecha_Inicio.Date,postModel.Fecha_Final.Date.AddDays(1), true,new List<DateTime>());
                datos.rest = rest;
                response.success = true;
                response.response = datos;
                return Ok(response);
        }
            catch (Exception ex)
            {
                response.success = false;
                response.response = $"Error al eliminar";
                return Ok(response);
    }
}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AsignacionHistoricoReal>>> GetAsignacion(int id)
        {
            //var asignaciones =  _context.Asignacion.Include(x=>x.Colaborador).Include(x=>x.Distribuciones).ToList();
            var asignaciones = _context.AsignacionReal
                                           .Include(x => x.Asignacion)
                                               .ThenInclude(z=>z.Colaborador)
                                           .Include(i => i.DistribucionesReales)
                                               .ThenInclude(y => y.Proyecto)
                                           .Where(x=>x.Asignacion.IdColaborador == id).ToList();

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
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Asignacion>> GetAsignacion(int id)
        //{
        //    var asignacion = await _context.Asignacion.FindAsync(id);

        //    if (asignacion == null)
        //    {
        //        return NotFound();
        //    }

        //    return asignacion;
        //}

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
 
                    var asignacionPlaneada = _context.Asignacion.Include(z => z.Colaborador)
                    .Include(i => i.AsignacionReal)
                    .ThenInclude(c => c.DistribucionesReales)
                    .ThenInclude(v => v.Proyecto)
                    .Where(x => x.IdColaborador == postModel.Id_Colaborador).First();

                    var asignacionReal = asignacionPlaneada.AsignacionReal.Where(x=>x.Id == id).First();

                    _context.DistribucionReal.RemoveRange(asignacionReal.DistribucionesReales);

                    asignacionReal.DistribucionesReales.Clear();

                    var AsignacionesReales = new List<AsignacionReal>();
                    AsignacionesReales = _context.AsignacionReal.Include(c => c.DistribucionesReales).Include(p => p.Asignacion).Where(x => x.Asignacion.IdColaborador == postModel.Id_Colaborador).ToList();

                
                        foreach (var x in AsignacionesReales)
                        {
                            foreach (var y in x.DistribucionesReales)
                            {
                                if ((x.Fecha_Final.Date >= postModel.Fecha_Inicio.Date && x.Fecha_Final.Date <= postModel.Fecha_Final.Date) ||
                                    (x.Fecha_Inicio.Date <= postModel.Fecha_Final.Date && x.Fecha_Inicio.Date >= postModel.Fecha_Inicio.Date) ||
                                    (postModel.Fecha_Final.Date >= x.Fecha_Inicio.Date && postModel.Fecha_Final.Date <= x.Fecha_Final.Date) ||
                                    (postModel.Fecha_Inicio.Date <= x.Fecha_Final.Date && postModel.Fecha_Inicio.Date >= x.Fecha_Inicio.Date))
                                {
                                    response.success = false;


                                    response.response = $"Error al registrar, existe un registro de proyecto en ese intervalo de fechas";
                                    transaction.Rollback();
                                    return Ok(response);
                                }
                            }
                        }
                    

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

                    asignacionReal.Fecha_Inicio = postModel.Fecha_Inicio;
                    asignacionReal.Fecha_Final = postModel.Fecha_Final;

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

                    var asignacionPlaneada = _context.Asignacion.Include(z => z.Colaborador)
                        .Include(i => i.AsignacionReal)
                        .ThenInclude(c => c.DistribucionesReales)
                        .ThenInclude(v => v.Proyecto)
                        .Where(x => x.IdColaborador == postModel.Id_Colaborador).First();
                    var asignacionReal = new AsignacionReal();
                    var distribucionReal = new List<DistribucionReal>();

                    var AsignacionesReales = new List<AsignacionReal>();
                    AsignacionesReales = _context.AsignacionReal.Include(c => c.DistribucionesReales).Include(p=>p.Asignacion).Where(x => x.Asignacion.IdColaborador == postModel.Id_Colaborador).ToList();
                    
                   
                        foreach (var x in AsignacionesReales)
                        {
                            foreach (var y in x.DistribucionesReales)
                            {
                                if ((x.Fecha_Final.Date >= postModel.Fecha_Inicio.Date && x.Fecha_Final.Date <= postModel.Fecha_Final.Date) || 
                                    (x.Fecha_Inicio.Date <= postModel.Fecha_Final.Date && x.Fecha_Inicio.Date >= postModel.Fecha_Inicio.Date) ||
                                    (postModel.Fecha_Final.Date >= x.Fecha_Inicio.Date && postModel.Fecha_Final.Date <= x.Fecha_Final.Date) ||
                                    (postModel.Fecha_Inicio.Date <= x.Fecha_Final.Date && postModel.Fecha_Inicio.Date >= x.Fecha_Inicio.Date))
                                {
                                    response.success = false;


                                    response.response = $"Error al registrar, existe un registro de proyecto en ese intervalo de fechas";
                                    transaction.Rollback();
                                    return Ok(response);
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
                    asignacionReal.Fecha_Inicio = postModel.Fecha_Inicio;
                    asignacionReal.Fecha_Final = postModel.Fecha_Final;
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
        [Route("FechasFaltantes")]
        public async Task<ActionResult<AsignacionReal>> FechasFaltantes(FiltroFechasFaltantes postModel)
        {
            Response response = new Response();
            try
            {
                List<DateTime> Fechas = new List<DateTime>();

                var Asignacion = _context.Asignacion.Include(x => x.Colaborador).Include(x => x.Distribuciones).Include(x => x.AsignacionReal).
                    Where(x => x.IdColaborador == postModel.Id_Colaborador).ToList();


                DateTime Fecha_Inicial = DateTime.Now.ToLocalTime().Date.AddMonths(-1);
                //DateTime Fecha_Inicial = Asignacion.Min(x => x.Distribuciones.Min(y => y.Fecha_Inicio)).Date;
                DateTime Fecha_Final = DateTime.Now.ToLocalTime().Date;

                var asignacionesReales = _context.AsignacionReal.Include(x => x.Asignacion)
                                                           .ThenInclude(z => z.Colaborador)
                                                       .Include(i => i.DistribucionesReales)
                                                           .ThenInclude(y => y.Proyecto).
                                                           Where(x => x.Asignacion.IdColaborador == postModel.Id_Colaborador).ToList();

                foreach (var item in asignacionesReales)
                {
                    for (DateTime i = Fecha_Inicial.Date; i < Fecha_Final.Date.AddDays(1); i = i.AddDays(1))
                    {
                        if (i.DayOfWeek != DayOfWeek.Sunday && i.DayOfWeek != DayOfWeek.Saturday && !(i.Date <= item.Fecha_Final.Date && i.Date >= item.Fecha_Inicio.Date))
                        {
                            Fechas.Add(i);
                        }
                    }
                }

                response.success = true;
                response.response = Fechas.GroupBy(x => x)
                            .Where(g => g.Count() == asignacionesReales.Count)
                            .Select(x => x.Key)
                            .ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.response = $"Error al consultar";
                return Ok(response);
            }

        }

            private static double Truncate(double value, int decimales)
        {
            double aux_value = Math.Pow(10, decimales);
            return (Math.Truncate(value * aux_value) / aux_value);
        }

        public static string FechaToString(DateTime value)
        {
            string[] meses = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic" };
            return (value.Day<10? "0"+value.Day.ToString():value.Day.ToString()) + "-" + meses[value.Month-1]+"-"+(value.Year-2000).ToString();
        }



        [HttpPost]
        [Route("Historico")]
        public async Task<ActionResult<AsignacionReal>> Historico(FiltroReporte postModel)
        {
            Response response = new Response();
            try
            {
                var AsignacionesReales = new List<AsignacionReal>();
                if (postModel.Fecha_Inicio != null&postModel.Fecha_Final!=null)
                {
                    AsignacionesReales = _context.AsignacionReal
                                                   .Include(x => x.Asignacion)
                                                       .ThenInclude(z => z.Colaborador)
                                                   .Include(i => i.DistribucionesReales)
                                                       .ThenInclude(y => y.Proyecto).
                                                       Where(postModel.Id_Colaborador!=0&&postModel.Id_Colaborador!=1? x=>x.Asignacion.IdColaborador==postModel.Id_Colaborador:x=>x.Id==x.Id).
                                                     Where(x =>
                                                     ((x.Fecha_Final.Date >= postModel.Fecha_Inicio.Date && x.Fecha_Final.Date <= postModel.Fecha_Final.Date) ||
                                                     (x.Fecha_Inicio.Date <= postModel.Fecha_Final.Date && x.Fecha_Inicio.Date >= postModel.Fecha_Inicio.Date)) ||
                                                      ((postModel.Fecha_Final.Date >= x.Fecha_Inicio.Date && postModel.Fecha_Final.Date <= x.Fecha_Final.Date) ||
                                                      (postModel.Fecha_Inicio.Date <= x.Fecha_Final.Date && postModel.Fecha_Inicio.Date >= x.Fecha_Inicio.Date))).ToList();

                }
                else
                {
                    AsignacionesReales = _context.AsignacionReal
                                                   .Include(x => x.Asignacion)
                                                       .ThenInclude(z => z.Colaborador)
                                                   .Include(i => i.DistribucionesReales)
                                                       .ThenInclude(y => y.Proyecto)
                                                   .ToList();
            }

                var rest = new List<rest>();
                var Colaboradores = _context.Colaboradores.Where(postModel.Id_Colaborador != 0 && postModel.Id_Colaborador != 1 ? x=>x.Id==postModel.Id_Colaborador:x =>x.Id!=1).ToList();
                foreach (var colaborador in Colaboradores)
                {
                    var proyectos = new List<HistoricoResponse>();
                    var AsignacionR = AsignacionesReales.Where(x => x.Asignacion.IdColaborador == colaborador.Id).ToList();
                    List<DateTime> FechasFaltantes = new List<DateTime>();
                    foreach (var item in AsignacionR)
                    {
                        for (DateTime i = postModel.Fecha_Inicio.Date; i < postModel.Fecha_Final.Date.AddDays(1); i = i.AddDays(1))
                        {
                            if (i.DayOfWeek != DayOfWeek.Sunday && i.DayOfWeek != DayOfWeek.Saturday && !(i.Date <= item.Fecha_Final.Date && i.Date >= item.Fecha_Inicio.Date))
                            {
                                FechasFaltantes.Add(i);
                            }
                        }
                    }

                    foreach (var L in AsignacionR)
                    {
                        foreach (var P in L.DistribucionesReales)
                        {

                            //int difFechas = DaysLeft(L.Fecha_Inicio.Date < postModel.Fecha_Inicio.Date ? postModel.Fecha_Inicio.Date : L.Fecha_Inicio.Date,
                            //    L.Fecha_Final.Date.AddDays(1) > postModel.Fecha_Final.Date ? postModel.Fecha_Final.Date.AddDays(1) : L.Fecha_Final.Date.AddDays(1), true, new List<DateTime>());

                            int difFechas = DaysLeft(L.Fecha_Inicio.Date < postModel.Fecha_Inicio.Date ? postModel.Fecha_Inicio.Date : L.Fecha_Inicio.Date,
                                L.Fecha_Final.Date.AddDays(1) > postModel.Fecha_Final.Date ? postModel.Fecha_Final.Date.AddDays(1) : L.Fecha_Final.Date.AddDays(1), true, new List<DateTime>());
                            if (proyectos.Where(x => x.id == P.IdProyecto).ToList().Count > 0)
                            {
                                proyectos.Where(x => x.id == P.IdProyecto).FirstOrDefault().value += P.Porcentaje * (difFechas);
                                proyectos.Where(x => x.id == P.IdProyecto).FirstOrDefault().dias += (difFechas) * (P.Porcentaje / 100);
                            }
                            else
                            {

                                proyectos.Add(new HistoricoResponse
                                {
                                    id = P.IdProyecto,
                                    clave = P.Proyecto.Clave,
                                    titulo = P.Proyecto.Titulo,
                                    value = P.Porcentaje * (difFechas),
                                    dias = ((double)P.Porcentaje / (double)100) * (double)difFechas
                                });
                            }
                        }

                    }


                    var sum = proyectos.Sum(x => x.value);
                var diferencia=0.0;
                for (int i = 0; i < proyectos.Count; i++)
                {
                    var porcentaje = (int)(((double)proyectos[i].value / (double)sum) * 100);
                    diferencia += Truncate(((((double)proyectos[i].value / (double)sum) * 100)- (int)(((double)proyectos[i].value / (double)sum) * 100)),5);
                    if (i == proyectos.Count - 1)
                    {
                        proyectos[i].porcentaje = Math.Ceiling(porcentaje+diferencia);
                    }
                    else
                    {
                        proyectos[i].porcentaje = Math.Floor((double)porcentaje);
                    }

                }
                if(proyectos.Count > 0)
                {
                        if (proyectos.Sum(x => x.porcentaje) < 100)
                        {
                            proyectos.FirstOrDefault().porcentaje += 100 - proyectos.Sum(x => x.porcentaje);
                        }
                 }
                    List<DateTime> faltantes = FechasFaltantes.GroupBy(x => x)
                            .Where(g => g.Count() == AsignacionR.Count)
                            .Select(x => x.Key)
                            .ToList();
                    List<DiasFaltantes> list = new List<DiasFaltantes>();
                    foreach(DateTime d in faltantes)
                    {
                        if (list.Where(x => x.final >= d.Date && x.inicio <= d.Date).ToList().Count == 0)
                        {
                            DateTime final = new DateTime();

                            var temp = list.Where(x => x.inicio <= d.Date && x.final >= d.Date).ToList();
                            Console.WriteLine(temp);
                            for (var i = 0; i < faltantes.Count; i++)
                            {
                                if (list.Where(x => x.final == faltantes[i]).ToList().Count == 0)
                                {
                                    if ((i + 1) == faltantes.Count)
                                    {
                                        final = faltantes[i].Date;
                                    }
                                    else
                                    {
                                        var d1 = faltantes[i].Date.AddDays(1);
                                        if (faltantes[i + 1].Date > d1 && faltantes[i].Date >= d)
                                        {
                                            final = faltantes[i].Date;
                                            break;
                                        }
                                    }
                                }
                            }
                            list.Add(new DiasFaltantes
                            {
                                inicio = d.Date,
                                final = d.Date== final? null:final,
                            }
                            );
                        }

                        
                    }

                    rest.Add(new rest
                    {
                        id_odoo = colaborador.Id_Odoo.Length > 20 ? colaborador.Id_Odoo.Substring(20) : colaborador.Id_Odoo,
                        colaborador = colaborador.Nombres + " " + colaborador.Apellidos,
                        asignaciones = proyectos,
                        diasfaltantes = list,
                        diasTrabajados =proyectos.Sum(x=>x.dias),
                        complete= ((double)proyectos.Sum(x => x.dias)/ (double)DaysLeft(postModel.Fecha_Inicio.Date, postModel.Fecha_Final.Date.AddDays(1), true, new List<DateTime>()))*100
                    });
                    if(rest.LastOrDefault().complete < 100)
                    {
                        foreach(var item in rest.LastOrDefault().asignaciones)
                        {
                            item.porcentaje=item.porcentaje* (rest.LastOrDefault().complete/100);
                        }
                    }

                }
                List<Excel> excel = new List<Excel>();
                excel.Add(new Excel { A = "fecha_inicio", B = FechaToString(postModel.Fecha_Inicio), C = "", D = "", E = "" });
                excel.Add(new Excel {A= "fecha_final", B= FechaToString(postModel.Fecha_Final), C="",D="",E=""});
                excel.Add(new Excel { A = "", B = "", C = "", D = "", E = "" });
                excel.Add(new Excel { A = "id_recurso", B = "recurso", C = "id_proyecto", D = "proyecto", E = "porcentaje" });
                foreach (var item in rest)
                {
                    foreach(var item2 in item.asignaciones)
                    {
                        if (item2.id == item.asignaciones.FirstOrDefault().id)
                        {
                            excel.Add(new Excel { A = item.id_odoo, B = item.colaborador, C = item2.clave, D = item2.titulo, E =  Truncate(item2.porcentaje,3).ToString() });
                        }
                        else
                        {
                            excel.Add(new Excel { A = "", B = "", C = item2.clave, D = item2.titulo, E = Truncate(item2.porcentaje, 3).ToString() });
                        }
                    }
                }
                dynamic datos = new System.Dynamic.ExpandoObject();
                datos.diastotales = DaysLeft(postModel.Fecha_Inicio.Date,postModel.Fecha_Final.Date.AddDays(1), true,new List<DateTime>());
                datos.porcentaje = ((rest.Sum(x => x.diasTrabajados)/rest.Count)
                    / DaysLeft(postModel.Fecha_Inicio.Date, postModel.Fecha_Final.Date.AddDays(1), true, new List<DateTime>()))*100;
                datos.rest = rest;
                datos.excel = excel;
                response.success = true;
                response.response = datos;
                return Ok(response);
        }
            catch (Exception ex)
            {
                response.success = false;
                response.response = $"Error al consultar";
                return Ok(response);
    }
}
    }
}

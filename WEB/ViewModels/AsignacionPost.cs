﻿namespace WEB.ViewModels
{
    public class AsignacionPost
    {
        public int Id_Colaborador { get; set; }
        //public DateTime Fecha_Inicio { get; set; }
        //public DateTime Fecha_Final { get; set; }
        public string User { get; set; }
        public int idAsignacion { get; set; }
        public List<ProyectosAsignados> Proyectos { get; set; }
    }

    public class ProyectosAsignados
    {
        public int Id { get; set; }
        public DateTime Fecha_inicio { get; set; }
        public DateTime Fecha_final { get; set; }
        //public int Porcentaje { get; set; }
    }



    public class AsignacionPostReal
    {
        public int Id_Colaborador { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Final { get; set; }
        public List<ProyectosAsignadosReal> Proyectos { get; set; }

    }

    public class ProyectosAsignadosReal
    {
        public int Id { get; set; }
        //public DateTime Fecha_inicio { get; set; }
        //public DateTime Fecha_final { get; set; }
        public int Porcentaje { get; set; }
    }

    public class FiltroReporte
    {
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Final { get; set; }
        public int Id_Colaborador { get; set; }
    }
    public class FiltroFechasFaltantes
    {
        public int Id_Colaborador { get; set; }
    }

    public class DiasFaltantes
    {
        public DateTime inicio { get; set; }
        public DateTime? final { get; set; }
    }

    public class HistoricoResponse
    {
        public int id { get; set; }
        public string clave { get; set; }
        public string? titulo { get; set; }
        public int value { get; set; }
        public double porcentaje { get; set; }
        public double dias { get; set; }

    }

    public class AsignacionesResponse
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string clave { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_final { get; set; }
    }

    public class rest
    {
        public double Sum(IEnumerable<double> source)
        {
            double sum = 0;
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }
        public string id_odoo { get; set; }
        public string colaborador { get; set; }
        public List<HistoricoResponse> asignaciones { get; set; }
        public List<DiasFaltantes> diasfaltantes { get; set; }
        public double diasTrabajados { get; set; }
        public double complete { get; set; }
    }
    public class Excel 
    {
        public dynamic A { get; set; }
        public dynamic B { get; set; }
        public dynamic C { get; set; }
        public dynamic D { get; set; }
        public dynamic E { get; set; }
    }

}

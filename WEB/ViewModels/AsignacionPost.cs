﻿namespace WEB.ViewModels
{
    public class AsignacionPost
    {
        public int Id_Colaborador { get; set; }
        //public DateTime Fecha_Inicio { get; set; }
        //public DateTime Fecha_Final { get; set; }
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

    public class Fechas
    {
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Final { get; set; }
    }

    public class HistoricoResponse
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public int value { get; set; }
        public double porcentaje { get; set; }
        public double dias { get; set; }
    }

}

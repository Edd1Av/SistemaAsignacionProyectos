using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace Data.Entities.Enum
{
    public enum ETipoAccion
    {
        ADDCOLABORADOR = 1,
        DELETECOLABORADOR = 2,
        UPDATECOLABORADOR = 3,
        ADDPROYECTO = 4,
        DELETEPROYECTO = 5,
        UPDATEPROYECTO = 6,
        ADDASIGNACIONPLANEADA = 7,
        DELETEASIGNACIONPLANEADA = 8,
        UPDATEASIGNACIONPLANEADA = 9,
        ADDASIGNACIONREAL = 10,
        DELETEASIGNACIONREAL = 11,
        UPDATEASIGNACIONREAL = 12,
    }
    public static class ETipoAccionS
    {
        public static string GetString(ETipoAccion item)
        {
            switch (item)
            {
                case  ETipoAccion.ADDCOLABORADOR:
                    return "Agregar colaborador";
                case ETipoAccion.DELETECOLABORADOR:
                    return "Eliminar Colaborador";
                case ETipoAccion.UPDATECOLABORADOR:
                    return "Actualizar Colaborador";
                case ETipoAccion.ADDPROYECTO:
                    return "Agregar Proyecto";
                case ETipoAccion.DELETEPROYECTO:
                    return "Eliminar Proyecto";
                case ETipoAccion.UPDATEPROYECTO:
                    return "Actualizar Proyecto";
                case ETipoAccion.ADDASIGNACIONPLANEADA:
                    return "Agregar Asignacion Planeada";
                case ETipoAccion.UPDATEASIGNACIONPLANEADA:
                    return "Actualizar Asignacion Planeada";
                case ETipoAccion.DELETEASIGNACIONPLANEADA:
                    return "Eliminar Asignacion Planeada";
                case ETipoAccion.ADDASIGNACIONREAL:
                    return "Agregar Asignacion Real";
                case ETipoAccion.DELETEASIGNACIONREAL:
                    return "Eliminar Asignacion Real";
                case ETipoAccion.UPDATEASIGNACIONREAL:
                    return "Actualizar Asignacion Real";
                default:
                    return "NO VALUE GIVEN";
            }
        }
    }


}
import { IAsignacion } from "./iasignacion";
import { IProyecto } from "./IProyectos";

export interface IDistribucion {
    id:Number;
    proyecto:IProyecto;
    asignacion:IAsignacion;
    fecha_Inicio:Date;
    fecha_Final:Date;
}

export interface IDistribucionReal {
    id:Number;
    proyecto:IProyecto;
    asignacion:IAsignacion;
    porcentaje:number;
}

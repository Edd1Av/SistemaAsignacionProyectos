import { IAsignacion } from "./iasignacion";
import { IProyecto } from "./IProyectos";

export interface IDistribucion {
    id:Number;
    proyecto:IProyecto;
    asignacion:IAsignacion;
    porcentaje:number;
}

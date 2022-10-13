import { IColaborador } from "./Icolaboradores";
import { IDistribucion } from "./idistribucion";

export interface IAsignacion {
    id:Number;
    fecha_inicio:Date;
    fecha_final:Date;
    colaborador:IColaborador;
    distribucion:IDistribucion[];
    proyectos:string;
}



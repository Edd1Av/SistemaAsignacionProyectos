import { IColaborador } from "./Icolaboradores";
import { IDistribucion } from "./idistribucion";

export interface IAsignacion {
    id:Number;
    fecha_inicio:Date;
    fecha_final:Date;
    fecha_inicio_s:string;
    fecha_final_s:string;
    colaborador:IColaborador;
    distribucion:IDistribucion[];
    proyectos:string;
}

export interface IAsignacionGet {
    id:Number;
    fecha_Inicio:Date;
    fecha_Final:Date;
    fecha_inicio_s:string;
    fecha_final_s:string;
    colaborador:IColaborador;
    distribuciones:IDistribucion[];
    proyectos:string;
}



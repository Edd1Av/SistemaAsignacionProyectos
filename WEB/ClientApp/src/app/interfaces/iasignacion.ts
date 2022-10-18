import { IColaborador } from "./Icolaboradores";
import { IDistribucion, IDistribucionReal } from "./idistribucion";

export interface IAsignacion {
    id:Number;
    colaborador:IColaborador;
    distribucion:IDistribucion[];
    proyectos:string;
}

export interface IAsignacionGet {
    id:Number;
    colaborador:IColaborador;
    distribuciones:IDistribucion[];
    proyectos:string;
}

export interface IAsignacionReal {
    id:Number;
    colaborador:IColaborador;
    distribucion:IDistribucionReal[];
    proyectos:string;
    fecha_inicio:Date;
    fecha_final:Date;
    fecha_inicio_s:string;
    fecha_final_s:string;
}

export interface IAsignacionGetReal {
    id:Number;
    colaborador:IColaborador;
    distribuciones:IDistribucionReal[];
    proyectos:string;
    fecha_inicio:Date;
    fecha_final:Date;
}



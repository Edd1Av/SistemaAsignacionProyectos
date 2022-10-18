import { IColaborador } from "./Icolaboradores";
import { IDistribucion } from "./idistribucion";

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
    distribucion:IDistribucion[];
    proyectos:string;
    fecha_inicio:Date;
    fecha_final:Date;
}

export interface IAsignacionGetReal {
    id:Number;
    colaborador:IColaborador;
    distribuciones:IDistribucion[];
    proyectos:string;
    fecha_inicio:Date;
    fecha_final:Date;
}



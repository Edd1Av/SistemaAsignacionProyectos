import { IProyectoAsignado } from "./iproyecto-asignado";

export interface IAsignacionPost {
    idColaborador:number;
    fecha_inicio: Date;
    fecha_final: Date;
    proyectos:IProyectoAsignado[]
}

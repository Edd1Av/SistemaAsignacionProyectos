import { IProyectoAsignado, IProyectoAsignadoReal } from "./iproyecto-asignado";

export interface IAsignacionPost {
    id_colaborador:number;
    // fecha_inicio:Date;
    // fecha_final:Date;
    proyectos:IProyectoAsignado[];
}

export interface IAsignacionPostReal {
    id_colaborador:number;
    fecha_inicio:Date;
    fecha_final:Date;
    proyectos:IProyectoAsignadoReal[];
}
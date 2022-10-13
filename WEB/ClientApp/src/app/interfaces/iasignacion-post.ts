import { IProyectoAsignado } from "./iproyecto-asignado";

export interface IAsignacionPost {
    id_colaborador:number;
    fecha_inicio:Date;
    fecha_final:Date;
    proyectos:IProyectoAsignado[];
}

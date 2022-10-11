import { IColaborador } from "./Icolaboradores";

export interface IAsignacion {
    id:Number;
    fecha_inicio:Date;
    fecha_final:Date;
    colaborador:IColaborador;
    proyectos:string;
}

export interface IProyectoAsignado {
    id?:Number;
    clave?:String;
    titulo?:String;
    fecha_inicio?:Date|null;
    fecha_final?:Date|null;

    fechaInicioMin?:Date|null;
    fechaInicioMax?:Date|null;
    fechaFinalMin?:Date|null;
    fechaFinalMax?:Date|null;
}

export interface IProyectoAsignadoReal{
    id?:Number;
    clave?:String;
    titulo?:String;
    // fecha_inicio:Date;
    // fecha_final:Date;
    porcentaje?:number;
}

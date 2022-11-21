export interface IProyectoAsignado {
    id:Number;
    clave:String;
    titulo:String;
    fecha_inicio:Date|undefined;
    fecha_final:Date|undefined;

    fechaInicioMin:Date|undefined;
    fechaInicioMax:Date|undefined;
    fechaFinalMin:Date|undefined;
    fechaFinalMax:Date|undefined;
}

export interface IProyectoAsignadoReal{
    id:Number;
    clave:String;
    titulo:String;
    // fecha_inicio:Date;
    // fecha_final:Date;
    porcentaje:number;
}

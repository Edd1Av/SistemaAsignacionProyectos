export interface IProyectoAsignado {
    id:Number;
    clave:String;
    titulo:String;
    fecha_inicio:Date;
    fecha_final:Date;
}

export interface IProyectoAsignadoReal{
    id:Number;
    clave:String;
    titulo:String;
    // fecha_inicio:Date;
    // fecha_final:Date;
    porcentaje:number;
}

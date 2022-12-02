export interface IReporte {
    success: boolean;
    response: Datos;
  }

export interface Datos {
    diastotales:number;
    rest:Ihistorico[];
    porcentaje:number;
    excel:Iexcel[];
}

export interface Iexcel {
    a:string;
    b:string;    
    c:string;    
    d:string;    
    e:string;        
}

export interface Ihistorico {
    id_odoo:String;
    colaborador:String;
    asignaciones:IProyectosHistorico[];
    diasTrabajados:number;
    complete:number;
    diasfaltantes:diasFaltantes[];
}

export interface diasFaltantes{
    inicio:Date;
    final:Date;
}

export interface IProyectosHistorico {
    clave:String;
    titulo:String;
    value:number;
    porcentaje:number;
    dias:number;
}


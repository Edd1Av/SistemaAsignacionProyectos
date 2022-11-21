export interface IReporte {
    success: boolean;
    response: Datos;
  }

export interface Datos {
    diastotales:number;
    rest:Ihistorico[];
    porcentaje:number;
}

export interface Ihistorico {
    colaborador:String;
    asignaciones:IProyectosHistorico[];
    diasTrabajados:number;
    complete:number;
    
}

export interface IProyectosHistorico {
    id:number;
    titulo:String;
    value:number;
    porcentaje:number;
    dias:number;
}


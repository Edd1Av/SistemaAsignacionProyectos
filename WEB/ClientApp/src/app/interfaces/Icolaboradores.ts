export interface IColaborador {
    id?:Number;
    nombres?:String;
    apellidos?:String;
    curp?:String;
    id_Odoo?:String;
    email?:string;
    proyectos?:ProyectosPost[];
    user?:string;
}

export interface IDelete{
    id:Number;
    user:string;
}

export interface ProyectosPost {
    titulo:String;
    clave:String;
}


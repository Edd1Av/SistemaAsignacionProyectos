export interface IProyecto{
    id:Number;
    clave:String;
    titulo:String;
    colaboradoresasignados:ColaboradoresAsignados[];
    user:string;
}


export interface ColaboradoresAsignados{
    nombres:String;
    apellidos:String;
    claveodoo:String;
}

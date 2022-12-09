export interface IProyecto{
    id:Number;
    clave:String;
    titulo:String;
    colaboradoresasignados:ColaboradoresAsignados[];
}

export interface ColaboradoresAsignados{
    nombres:String;
    apellidos:String;
    claveodoo:String;
}

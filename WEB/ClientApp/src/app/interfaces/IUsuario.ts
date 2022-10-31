export interface IUsuario{
    success:boolean;
    idUsuario:number;
    correo:string;
    rol:string;
    token:string;
    expiration:Date;
}
export interface IUsuario{
    success:boolean;
    id:number;
    email:string;
    rol:string;
    token:string;
    expiration:Date;
}
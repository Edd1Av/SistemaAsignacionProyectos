import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable} from 'rxjs';
import { map} from 'rxjs/operators';
import { IChangePassword, ILogin, IPostAdmin, IResetPassword } from 'src/app/interfaces/ILogin';
import { IResponse } from 'src/app/interfaces/IResponse';
import { IUsuario } from 'src/app/interfaces/IUsuario';


@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {

  usuario: IUsuario|null;
  public urlBase: string;

  public changeLoginStatusSubject;
  public changeLoginStatus;

  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string, private router: Router) {
    this.urlBase = baseUrl;

    let local = localStorage.getItem('Sesion')
    let usuario:string;
  
    local!=null?usuario=local:usuario="null";
    this.changeLoginStatusSubject = new BehaviorSubject<IUsuario|null>(JSON.parse(usuario));
    this.changeLoginStatus = this.changeLoginStatusSubject.asObservable();
    
    this.validarExp();
    
    
  }

  public validarExp(){
    if(this.changeLoginStatusSubject.value !=null){
      let user:IUsuario = this.changeLoginStatusSubject.value;
      let now:Date = new Date();
      let exp:Date = new Date(user.expiration);
      console.log("Ahora",now);
      console.log("Expiración", exp);
      if(now > exp){
        console.log("DEBE SALIR")
        this.logout();
      }
    }
  }

  public get usuarioData(){
    return this.changeLoginStatusSubject.value;
  }

  
  //x:IUsuario=new{success=false, token="", };
 
  // By default pop ups are disabled because they don't work properly on Edge.
  // If you want to enable pop up authentication simply set this flag to false.

  public Login(credenciales: ILogin):Observable<IUsuario>{
    return this.http.post<IUsuario>(this.urlBase + "api/Identity/Login", credenciales).pipe(map(result=>{

      if(result.success==true){
        localStorage.setItem("Sesion", JSON.stringify(result));
        this.changeLoginStatusSubject.next(result);
        //this.router.navigate(["/home"]);
      }
       return result;
    }));
  }

  public ChangePassword(credenciales: IChangePassword):Observable<IResponse>{
    return this.http.post<IResponse>(this.urlBase + "api/Identity/ChangePassword", credenciales);
  }

  public ResetPassword(credenciales: IResetPassword):Observable<IResponse>{
    return this.http.post<IResponse>(this.urlBase + "api/Identity/ResetPassword", credenciales);
  }

  public AddAdmin(credenciales: IPostAdmin):Observable<IResponse>{
    return this.http.post<IResponse>(this.urlBase + "api/Identity/AddAdmin", credenciales);
  }

  public DeleteAdmin(credenciales: ILogin):Observable<IResponse>{
    return this.http.post<IResponse>(this.urlBase + "api/Identity/DeleteAdmin", credenciales);
  }

  public logout() {
    localStorage.removeItem("Sesion");
    this.changeLoginStatusSubject.next(null);
    this.router.navigate(['/login'])
  }

  public isLoggedIn(){
    this.usuario = this.changeLoginStatusSubject.value;
    //this.usuario = JSON.parse(localStorage.getItem('Sesion')||JSON.stringify(false));
    if(this.usuario){
      return this.usuario;
    }
    return false;
  }

  public isLogged(){
    this.usuario = this.changeLoginStatusSubject.value;
    //this.usuario = JSON.parse(localStorage.getItem('Sesion')||JSON.stringify(false));
    if(this.usuario){
      return true;
    }
    return false;
  }

  public getToken(){
    this.usuario = this.changeLoginStatusSubject.value;
    if(this.usuario){
      return this.usuario.token;
    }
    return "";
  }

  public getRol(){
    if(this.usuario){
      return this.usuario.rol;
    }
    else{
      return false;
      //this.usuario = JSON.parse(localStorage.getItem('Sesion')||JSON.stringify(false));
    }
  }
}

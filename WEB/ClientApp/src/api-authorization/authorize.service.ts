import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { User, UserManager } from 'oidc-client';
import { BehaviorSubject, concat, from, Observable, Subject } from 'rxjs';
import { filter, map, mergeMap, take, tap } from 'rxjs/operators';
import { ILogin } from 'src/app/interfaces/ILogin';
import { IResponse } from 'src/app/interfaces/IResponse';
import { IUsuario } from 'src/app/interfaces/IUsuario';
import { ApplicationPaths, ApplicationName } from './api-authorization.constants';


@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {
  public urlBase: string;
  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.urlBase = baseUrl;
  }

  usuario: IUsuario;
  //x:IUsuario=new{success=false, token="", };
  public changeLoginStatusSubject = new Subject<IUsuario>();
  public changeLoginStatus = this.changeLoginStatusSubject.asObservable();
  // By default pop ups are disabled because they don't work properly on Edge.
  // If you want to enable pop up authentication simply set this flag to false.

  public Login(credenciales: ILogin):Observable<IUsuario>{
    return this.http.post<IUsuario>(this.urlBase + "api/Identity/Login", credenciales);
  }

  public logout() {
    localStorage.removeItem("Sesion");

    this.changeLoginStatusSubject.next(undefined);
  }

  public isLoggedIn(){
    //this.x.success=false;
    this.usuario = JSON.parse(localStorage.getItem('Sesion')||JSON.stringify(false));
    if(this.usuario){
      return this.usuario;
    }
    return false;
  }

  public getRol(){
    this.usuario = JSON.parse(localStorage.getItem('Sesion')||JSON.stringify(false));
  }
}

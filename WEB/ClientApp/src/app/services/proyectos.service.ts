import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IResponse } from '../interfaces/IResponse';
import { IProyecto } from '../interfaces/IProyectos';
import { map } from 'rxjs/operators';
import { IntervaloFecha } from '../interfaces/IntervaloFechas';

@Injectable({
  providedIn: 'root'
})
export class ProyectosService {

  public urlBase: string;

  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.urlBase = baseUrl;
}


  getProyectoById(id: number): Observable<IProyecto> {
    return this.http.get<IProyecto>(this.urlBase + "api/proyectos/" + id);
  }

  getProyectos(): Observable<IProyecto[]> {
    return this.http.get<IProyecto[]>(this.urlBase + "api/proyectos");
  }

  getProyectosColaborador(id:number, intervaloFecha:IntervaloFecha): Observable<IProyecto[]> {
    return this.http.post<IProyecto[]>(this.urlBase + "api/proyectos/proyectosColaborador/"+id,intervaloFecha);
  }

  getProyectosColaboradorUp(id:number, intervaloFecha:IntervaloFecha): Observable<IProyecto[]> {
    return this.http.post<IProyecto[]>(this.urlBase + "api/proyectos/proyectosColaboradorUp/"+id,intervaloFecha);
  }

  UpdateProyecto(id:number, Proyecto:IProyecto,User:string)  {
    Proyecto.user=User;
    return this.http.put<IResponse>(this.urlBase + "api/proyectos/"+id, Proyecto);
  }

  SetProyecto(Proyecto:IProyecto,User:string)  {
    Proyecto.user=User;
      return this.http.post<IResponse>(this.urlBase + "api/proyectos",Proyecto);
    }

  DeleteProyecto(Proyecto:IProyecto): Observable<IResponse> {
      return this.http.post<IResponse>(this.urlBase + "api/proyectos/delete",Proyecto);
  }
}

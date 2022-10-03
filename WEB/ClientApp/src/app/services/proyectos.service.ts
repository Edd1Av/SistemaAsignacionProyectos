import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IResponse } from '../interfaces/IResponse';
import { IProyectos } from '../interfaces/IProyectos';

@Injectable({
  providedIn: 'root'
})
export class ProyectosService {

  public urlBase: string;

  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.urlBase = baseUrl;
}


  getProyectoById(id: number): Observable<IProyectos> {
    return this.http.get<IProyectos>(this.urlBase + "api/proyectos" + id);
  }

  getProyectos(): Observable<IProyectos[]> {
    return this.http.get<IProyectos[]>(this.urlBase + "api/proyectos");
  }

  UpdateProyecto(Proyecto:IProyectos)  {
    return this.http.put<IResponse>(this.urlBase + "api/proyectos",Proyecto);
  }

  SetProyecto(Proyecto:IProyectos)  {
      return this.http.post<IResponse>(this.urlBase + "api/proyectos",Proyecto);
    }

  DeleteProyecto(id: number): Observable<IResponse> {
      return this.http.delete<IResponse>(this.urlBase + "api/proyectos"+id);
  }
}

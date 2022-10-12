import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IAsignacion } from '../interfaces/iasignacion';
import { IAsignacionPost } from '../interfaces/iasignacion-post';
import { IResponse } from '../interfaces/iResponse';

@Injectable({
  providedIn: 'root'
})
export class AsignacionesService {
  public urlBase: string;

  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.urlBase = baseUrl;
  }

  getAsignacionById(id: number): Observable<IAsignacion> {
    return this.http.get<IAsignacion>(this.urlBase + "api/Asignaciones" + id);
  }

  getAsignaciones(): Observable<IAsignacion[]> {
    return this.http.get<IAsignacion[]>(this.urlBase + "api/Asignaciones");
  }

  UpdateAsignacion(id:number, Asignacion:IAsignacion)  {
    return this.http.put<IResponse>(this.urlBase + "api/Asignaciones/"+id, Asignacion);
  }

  SetAsignacion(Asignacion:IAsignacionPost)  {
    console.log(Asignacion);
      return this.http.post<IResponse>(this.urlBase + "api/Asignaciones",Asignacion);
    }

  DeleteAsignacion(id: number): Observable<IResponse> {
      return this.http.delete<IResponse>(this.urlBase + "api/Asignaciones/"+id);
  }
}

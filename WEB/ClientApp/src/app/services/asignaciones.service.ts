import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IAsignacionReal, IAsignacionGet, IAsignacion } from '../interfaces/iasignacion';
import { IAsignacionPost, IAsignacionPostReal } from '../interfaces/iasignacion-post';
import { IDelete } from '../interfaces/Icolaboradores';
import { IntervaloFecha } from '../interfaces/IntervaloFechas';
import { IResponse } from '../interfaces/IResponse';


@Injectable({
  providedIn: 'root'
})
export class AsignacionesService {
  public urlBase: string;

  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.urlBase = baseUrl;
  }


  //Asignaciones
  getAsignacionById(id: number): Observable<IAsignacion> {
    return this.http.get<IAsignacion>(this.urlBase + "api/Asignaciones" + id);
  }

  getAsignacionByIdColaborador(id: number): Observable<IAsignacionGet> {
    return this.http.get<IAsignacionGet>(this.urlBase + "api/Asignaciones/ByColaborador/" + id);
  }


  getAsignaciones(): Observable<IAsignacion[]> {
    return this.http.get<IAsignacion[]>(this.urlBase + "api/Asignaciones");
  }

  UpdateAsignacion(id:number, Asignacion:IAsignacionPost):Observable<IResponse>{
    return this.http.put<IResponse>(this.urlBase + "api/Asignaciones/"+id, Asignacion);
  }

  SetAsignacion(Asignacion:IAsignacionPost):Observable<IResponse>{
      return this.http.post<IResponse>(this.urlBase + "api/Asignaciones",Asignacion);
    }

  DeleteAsignacion(Asignacion:IDelete): Observable<IResponse> {
      return this.http.post<IResponse>(this.urlBase + "api/Asignaciones/delete",Asignacion,);
  }




 
    //AsignacionesReales
    // getAsignacionRealById(id: number): Observable<IAsignacionReal> {
    //   return this.http.get<IAsignacionReal>(this.urlBase + "api/AsignacionReal" + id);
    // }
  
    // getAsignacionRealByIdColaborador(id: number): Observable<IAsignacionGet> {
    //   return this.http.get<IAsignacionGet>(this.urlBase + "api/AsignacionReal/ByColaborador/" + id);
    // }
  
    //Historico
    getAsignacionesReal(id: number): Observable<IAsignacionReal[]> {
      return this.http.get<IAsignacionReal[]>(this.urlBase + "api/AsignacionReal/"+id);
    }
  
    UpdateAsignacionReal(id:number, Asignacion:IAsignacionPostReal)  {
      return this.http.put<IResponse>(this.urlBase + "api/AsignacionReal/"+id, Asignacion);
    }
  
    SetAsignacionReal(Asignacion:IAsignacionPostReal)  {
      console.log(Asignacion);
        return this.http.post<IResponse>(this.urlBase + "api/AsignacionReal",Asignacion);
      }

      
  
    DeleteAsignacionReal(id: number): Observable<IResponse> {
        return this.http.delete<IResponse>(this.urlBase + "api/AsignacionReal/"+id);
    }

    GetFechasFaltantes(postModel:any)  {
      console.log(Response);
        return this.http.post<IResponse>(this.urlBase + "api/AsignacionReal/FechasFaltantes",postModel);
    }

    GetFechasLimite(id:number){
      return this.http.get<IntervaloFecha>(this.urlBase+"api/AsignacionReal/FechasLimite/"+id);
    }
}

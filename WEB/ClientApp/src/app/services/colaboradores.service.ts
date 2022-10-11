import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {IColaborador} from 'src/app/interfaces/Icolaboradores';
import {IResponse} from 'src/app/interfaces/iResponse';
@Injectable({
  providedIn: 'root'
}) 



export class ColaboradoresService {
  public urlBase: string;

  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.urlBase = baseUrl;
  }

  getColaboradorById(id: number): Observable<IColaborador> {
    return this.http.get<IColaborador>(this.urlBase + "api/Colaboradores" + id);
  }

  getColaboradores(): Observable<IColaborador[]> {
    return this.http.get<IColaborador[]>(this.urlBase + "api/Colaboradores");
  }

  UpdateColaborador(id:number, Colaborador:IColaborador)  {
    return this.http.put<IResponse>(this.urlBase + "api/Colaboradores/"+id, Colaborador);
  }

  SetColaborador(Colaborador:IColaborador)  {
    console.log(Colaborador);
      return this.http.post<IResponse>(this.urlBase + "api/Colaboradores",Colaborador);
    }

  DeleteColaborador(id: number): Observable<IResponse> {
      return this.http.delete<IResponse>(this.urlBase + "api/Colaboradores/"+id);
  }
}



import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {Icolaboradores} from 'src/app/interfaces/Icolaboradores';
import {IResponse} from 'src/app/interfaces/iresponse';
@Injectable({
  providedIn: 'root'
})



export class ColaboradoresService {
  public urlBase: string;

  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.urlBase = baseUrl;
  }

  getColaboradorById(id: number): Observable<Icolaboradores> {
    return this.http.get<Icolaboradores>(this.urlBase + "api/Colaboradores" + id);
  }

  getColaboradores(): Observable<Icolaboradores[]> {
    return this.http.get<Icolaboradores[]>(this.urlBase + "api/Colaboradores");
  }

  UpdateColaborador(Colaborador:Icolaboradores)  {
    return this.http.put<IResponse>(this.urlBase + "api/Colaboradores",Colaborador);
  }

  SetColaborador(Colaborador:Icolaboradores)  {
      return this.http.post<IResponse>(this.urlBase + "api/Colaboradores",Colaborador);
    }

  DeleteColaborador(id: number): Observable<IResponse> {
      return this.http.delete<IResponse>(this.urlBase + "api/Colaboradores"+id);
  }
}



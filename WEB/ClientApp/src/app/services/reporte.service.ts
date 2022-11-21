import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { IReporte } from '../interfaces/ireporte';

@Injectable({
  providedIn: 'root'
})
export class ReporteService {
  public urlBase: string;
  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.urlBase = baseUrl;
  }

  GetReporte(postModel:any)  {
    console.log(Response);
      return this.http.post<IReporte>(this.urlBase + "api/AsignacionReal/Historico",postModel);
    }
}



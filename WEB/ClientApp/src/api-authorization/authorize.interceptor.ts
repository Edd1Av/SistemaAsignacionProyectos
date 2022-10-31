import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpSentEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthorizeService } from './authorize.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeInterceptor implements HttpInterceptor {
  constructor(private authorize: AuthorizeService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let token = this.authorize.getToken();
    req = req.clone({
      setHeaders:{Authorization: "bearer " + token}
    });
    return next.handle(req)
  }
}

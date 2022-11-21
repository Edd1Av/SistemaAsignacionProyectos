import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpSentEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthorizeService } from './authorize.service';
import { LoaderService } from 'src/app/loader/loader.service';
import { finalize } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeInterceptor implements HttpInterceptor {
  constructor(private authorize: AuthorizeService,public loaderService:LoaderService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.loaderService.isLoading.next(true);
    let token = this.authorize.getToken();
    req = req.clone({
      setHeaders:{Authorization: "bearer " + token}
    });
    return next.handle(req).pipe(
      finalize(
        ()=>{
          this.loaderService.isLoading.next(false);
        }
      )
    );
  }
}

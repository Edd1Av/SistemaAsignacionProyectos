import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizeService } from './authorize.service';
import { tap } from 'rxjs/operators';
import { ApplicationPaths, QueryParameterNames } from './api-authorization.constants';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeGuardDesarrollador implements CanActivate {
  constructor(private authorize: AuthorizeService, private router: Router) {
  }
  canActivate(
    _next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    
      let user = this.authorize.isLoggedIn()
      console.log("can activate desarrollador", user)
      if(user){
        if (user.rol=="Desarrollador"){
          return true;
        }
        else{
          this.router.navigate(['/home']);
          return false;
        }
      }
      this.router.navigate(['/login']);
      return false;
  }

}

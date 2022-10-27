import { Component, OnInit } from '@angular/core';
import { AuthorizeService} from '../authorize.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Subject } from 'rxjs';
import { LoginActions, QueryParameterNames, ApplicationPaths, ReturnUrlType } from '../api-authorization.constants';
import { ILogin } from 'src/app/interfaces/ILogin';
import { IResponse } from 'src/app/interfaces/IResponse';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IUsuario } from 'src/app/interfaces/IUsuario';

// The main responsibility of this component is to handle the user's login process.
// This is the starting point for the login process. Any component that needs to authenticate
// a user can simply perform a redirect to this component with a returnUrl query parameter and
// let the component perform the login and return back to the return url.
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(
    private authorizeService: AuthorizeService,
    private activatedRoute: ActivatedRoute,
    private router: Router, private _snackBar: MatSnackBar) { }

    public changeLoginStatusSubject = new Subject<IUsuario>();
    public changeLoginStatus = this.changeLoginStatusSubject.asObservable();

    datos:ILogin;
  async ngOnInit() {
    
  }

  Login(){
    this.authorizeService.Login(this.datos).subscribe(result=> {
      if (result.success == true){
      
        localStorage.setItem("Sesion", JSON.stringify(result));
        this.changeLoginStatusSubject.next(result);
        this.router.navigate(["/home"]);
      }
      else{
        this.openSnackBar("Datos incorrectos");
      }
    });
  }

  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }
}



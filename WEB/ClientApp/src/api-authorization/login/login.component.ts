import { Component, OnInit } from '@angular/core';
import { AuthorizeService} from '../authorize.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ILogin } from 'src/app/interfaces/ILogin';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

// The main responsibility of this component is to handle the user's login process.
// This is the starting point for the login process. Any component that needs to authenticate
// a user can simply perform a redirect to this component with a returnUrl query parameter and
// let the component perform the login and return back to the return url.
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css', './login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(
    private authorizeService: AuthorizeService,
    private activatedRoute: ActivatedRoute,
    private router: Router, private _snackBar: MatSnackBar,
    private formBuilder: FormBuilder,) { }


    datos:ILogin;
    formGroup!: FormGroup;
  async ngOnInit() {
    this.buildForm();

    
  }

  buildForm(){
    this.formGroup = this.formBuilder.group({
      email: new FormControl("", Validators.required),
      password: new FormControl("", Validators.required),
    });
  }

  onSubmit(){
    if(this.formGroup.valid){
      this.authorizeService.Login(this.formGroup.value).subscribe(result=> {
        console.log(result);
        if (result.success == true){
          if(result.rol=="Administrador"){
            console.log("es admin ir a home");
            this.router.navigate(["/home"]);
          }
          if(result.rol == "Desarrollador"){
            console.log("es desarrollador ir a asignaciones reales");
            this.router.navigate(["/asignacionReal"])
          }
          
        }
        else{
          this.openSnackBar("Datos incorrectos");
        }
      });
    }
    else{
      this.openSnackBar("Rellene todos los campos");
    }
    
  }

  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }
}



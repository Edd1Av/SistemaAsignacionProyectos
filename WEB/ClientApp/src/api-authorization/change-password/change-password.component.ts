import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { IResponse } from 'src/app/interfaces/IResponse';
import { IUsuario } from 'src/app/interfaces/IUsuario';
import { AuthorizeService } from '../authorize.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  constructor(private authorizeService: AuthorizeService,
    private activatedRoute: ActivatedRoute,
    private router: Router, private _snackBar: MatSnackBar,
    private formBuilder: FormBuilder,) { }

  formGroup!: FormGroup;
  Usuario:IUsuario;
  async ngOnInit() {
    if(this.authorizeService.usuarioData!=null){
      this.Usuario=this.authorizeService.usuarioData;
    }
    this.buildForm();
  }

  buildForm(){
    this.formGroup = this.formBuilder.group({
      email: new FormControl(this.Usuario.correo, Validators.required),
      password: new FormControl("", Validators.required),
      npassword: new FormControl("", Validators.required),
    });
  }

  onSubmit(){
    if(this.formGroup.valid){
      this.authorizeService.ChangePassword(this.formGroup.value).pipe(tap((result: IResponse)=>{
        this.openSnackBar(result.response);
        if(result.success){
          this.router.navigate(["/home"]);
        }
      })).subscribe();
    }
    else{
      this.openSnackBar("Ingrese todos los datos");
    }
    
  }

  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }

}

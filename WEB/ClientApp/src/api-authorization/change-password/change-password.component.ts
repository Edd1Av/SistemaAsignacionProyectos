import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
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

  constructor(
    @Inject(MAT_DIALOG_DATA) private data:any,
    private matDialogref: MatDialogRef<ChangePasswordComponent>,
    private authorizeService: AuthorizeService,
    private _snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
) { }

  formGroup!: FormGroup;
  Usuario:IUsuario;

  ngOnInit() {
    if(this.authorizeService.usuarioData!=null){
      this.Usuario=this.authorizeService.usuarioData;
    }
    this.buildForm();
  }

  buildForm(){
    this.formGroup = this.formBuilder.group({
      email: new FormControl(this.Usuario.correo, Validators.required),
      password: new FormControl("", Validators.required),
      nPassword: new FormControl("", Validators.required),
      nPasswordConfirm: new FormControl("", Validators.required),
    });
  }

  onSubmit(){
    if(this.formGroup.valid){
      this.authorizeService.ChangePassword(this.formGroup.value).pipe(tap((result: IResponse)=>{
        this.openSnackBar(result.response);
        if(result.success){
          this.matDialogref.close();
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

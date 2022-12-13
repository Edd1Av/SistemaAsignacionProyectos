import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { User } from 'oidc-client';
import { tap } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { IResponse } from 'src/app/interfaces/IResponse';
import { ColaboradoresService } from 'src/app/services/colaboradores.service';

@Component({
  selector: 'app-colaborador-insert',
  templateUrl: './colaborador-insert.component.html',
  styleUrls: ['./colaborador-insert.component.css']
})
export class ColaboradorInsertComponent implements OnInit {

  formGroup!: FormGroup;
 
  constructor(
    @Inject(MAT_DIALOG_DATA) private data:any,
    private matDialogref: MatDialogRef<ColaboradorInsertComponent>,
    private formBuilder: FormBuilder,
    private authService: AuthorizeService,
    private _colaboradorService: ColaboradoresService,
    private _snackBar: MatSnackBar
  ) { }
  User:string;
  ngOnInit(): void {
    
  this.buildForm();
  this.authService.changeLoginStatus.subscribe((value)=>{
    if(value){
      this.User=value.correo;
      
    }
  })
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      nombres: new FormControl("", Validators.required),
      apellidos: new FormControl("", Validators.required),
      curp: new FormControl("", Validators.required),
      id_odoo: new FormControl("res_partner_contact_", Validators.required),
      email: new FormControl("", Validators.required)
    });
  }

  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }

  onSubmit() {
    if(this.formGroup.valid){
        this._colaboradorService
          .SetColaborador(this.formGroup.value,this.User)
          .pipe(
            tap((result: IResponse) => {
              this.openSnackBar(result.response);
              if (result.success) {
                this.matDialogref.close();
              }
            })
          )
          .subscribe(); 
    }else{
      this.openSnackBar("Introduzca los campos faltantes");
    }
  }

}

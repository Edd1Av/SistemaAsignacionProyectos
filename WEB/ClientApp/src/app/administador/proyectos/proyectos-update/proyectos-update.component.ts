import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { ProyectosService } from 'src/app/services/proyectos.service';

@Component({
  selector: 'app-proyectos-update',
  templateUrl: './proyectos-update.component.html',
  styleUrls: ['./proyectos-update.component.css']
})
export class ProyectosUpdateComponent implements OnInit {

  formGroup!: FormGroup;
 
  constructor(
    @Inject(MAT_DIALOG_DATA) private data:any,
    private matDialogref: MatDialogRef<ProyectosUpdateComponent>,
    private formBuilder: FormBuilder,
    private authService: AuthorizeService,
    private _proyectosService: ProyectosService,
    private _snackBar: MatSnackBar
  ) { }
    User:string;
  ngOnInit(): void {
  this.buildForm();
  this.formGroup.controls.id.setValue(this.data.proyecto.id);
  this.formGroup.controls.titulo.setValue(this.data.proyecto.titulo);
  this.formGroup.controls.clave.setValue(this.data.proyecto.clave);
  this.authService.changeLoginStatus.subscribe((value)=>{
    if(value){
      this.User=value.correo;
      
    }
  })
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      id: new FormControl("", Validators.required),
      titulo: new FormControl("", Validators.required),
      clave: new FormControl("", Validators.required)
    });
  }

  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }

  onSubmit() {
    if(this.formGroup.valid){
        
        this._proyectosService.UpdateProyecto( this.data.proyecto.id, this.formGroup.value,this.User).subscribe((result) => {
          if (result.success) {
            //this.formGroup.reset();
            //this.initializeFormGroup();
            this.matDialogref.close();
          }
          this.openSnackBar(result.response);
        });
    }else{
      this.openSnackBar("Introduzca los campos faltantes");
    }
  }

}

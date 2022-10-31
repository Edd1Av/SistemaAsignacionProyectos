import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { tap } from 'rxjs/operators';
import { IResponse } from 'src/app/interfaces/IResponse';
import { ProyectosService } from 'src/app/services/proyectos.service';

@Component({
  selector: 'app-proyectos-insert',
  templateUrl: './proyectos-insert.component.html',
  styleUrls: ['./proyectos-insert.component.css']
})
export class ProyectosInsertComponent implements OnInit {

  formGroup!: FormGroup;
 
  constructor(
    @Inject(MAT_DIALOG_DATA) private data:any,
    private matDialogref: MatDialogRef<ProyectosInsertComponent>,
    private formBuilder: FormBuilder,
    private _proyectosService: ProyectosService,
    private _snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    
  this.buildForm();
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      titulo: new FormControl("", Validators.required),
      clave: new FormControl("", Validators.required),
    });
  }

  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }

  onSubmit() {
    if(this.formGroup.valid){
        this._proyectosService
          .SetProyecto(this.formGroup.value)
          .pipe(
            tap((result: IResponse) => {
              console.log(result);
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

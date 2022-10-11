import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { tap } from 'rxjs/operators';
import { IColaborador } from 'src/app/interfaces/Icolaboradores';
import { IProyecto } from 'src/app/interfaces/IProyectos';
import { IResponse } from 'src/app/interfaces/iResponse';
import { AsignacionesService } from 'src/app/services/asignaciones.service';

@Component({
  selector: 'app-asignaciones-insert',
  templateUrl: './asignaciones-insert.component.html',
  styleUrls: ['./asignaciones-insert.component.css']
})
export class AsignacionesInsertComponent implements OnInit {
  formGroup!: FormGroup;
 
  constructor(
    @Inject(MAT_DIALOG_DATA) private data:any,
    private matDialogref: MatDialogRef<AsignacionesInsertComponent>,
    private formBuilder: FormBuilder,
    private _asignacionService: AsignacionesService,
    private _snackBar: MatSnackBar
  ) { }

  colaboradores: IColaborador[]=[];
  Proyectos:IProyecto[]=[];
  ngOnInit(): void {
    
  this.buildForm();
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      fecha_inicio: new FormControl("", Validators.required),
      fecha_final: new FormControl("", Validators.required),
      colaborador: new FormControl("", Validators.required),
      proyectos: new FormControl("", Validators.required),
    });
  }

  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }

  onSubmit() {
    if(this.formGroup.valid){
        this._asignacionService
          .SetAsignacion(this.formGroup.value)
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

import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ProyectosService } from 'src/app/services/proyectos.service';
import { ProyectosUpdateComponent } from '../proyectos-update/proyectos-update.component';

@Component({
  selector: 'app-proyectos-details',
  templateUrl: './proyectos-details.component.html',
  styleUrls: ['./proyectos-details.component.css']
})
export class ProyectosDetailsComponent implements OnInit {
  formGroup!: FormGroup;
  displayedColumnsColaboradores: string[] = [
    "Nombres",
    "Apellidos",
    "ClaveOdoo",
  ];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data:any,
    private matDialogref: MatDialogRef<ProyectosUpdateComponent>,
    private formBuilder: FormBuilder,
    private _proyectosService: ProyectosService,
    private _snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
  this.buildForm();
  this.formGroup.controls.id.setValue(this.data.proyecto.id);
  this.formGroup.controls.titulo.setValue(this.data.proyecto.titulo);
  this.formGroup.controls.clave.setValue(this.data.proyecto.clave);
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

}

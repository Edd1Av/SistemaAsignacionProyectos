import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { tap } from 'rxjs/operators';
import { IResponse } from 'src/app/interfaces/iresponse';
import { ColaboradoresService } from 'src/app/services/colaboradores.service';

@Component({
  selector: 'app-colaborador-insert',
  templateUrl: './colaborador-insert.component.html',
  styleUrls: ['./colaborador-insert.component.css']
})
export class ColaboradorInsertComponent implements OnInit {

  formGroup!: FormGroup;
  // GuardarActivo = true;
  _snackBar!: MatSnackBar
  constructor(
    @Inject(MAT_DIALOG_DATA) private data:any,
    private matDialogref: MatDialogRef<ColaboradorInsertComponent>,
    private formBuilder: FormBuilder,
    private _colaboradorService: ColaboradoresService
  ) { }

  ngOnInit(): void {
    
  this.buildForm();
  // if (this.data.type == this.EDIT) {
  //   this.isEdit = true;
  //   this.formGroup.controls.nombre.setValue(this.data.color.nombre);
  // }
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      nombre: new FormControl("", Validators.required)
    });
  }

  initializeFormGroup() {
    this.formGroup = this.formBuilder.group({
      nombre: ""
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
          .SetColaborador(this.formGroup.controls["nombre"].value)
          .pipe(
            tap((result: IResponse) => {
              this.openSnackBar(result.response);
              if (result.success) {
                this.matDialogref.close();
              }
              // this.GuardarActivo = true;
            })
          )
          .subscribe(); 
      // else {
      //   var colorSelected = {
      //     nombre: this.formGroup.controls["nombre"].value,
      //     id: this.data.color.id
      //  };
      //   this._colorService.updateColor(colorSelected).subscribe((result) => {
      //     if (result.success) {
      //       this.formGroup.reset();
      //       this.initializeFormGroup();
      //       this.matDialogref.close();
      //     }
      //     this.openSnackBar(result.response);
      //     // this.GuardarActivo = true;
      //   });
      // }
    }else{
      this.openSnackBar("Introduzca los campos faltantes");
    }
  }

}

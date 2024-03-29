import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { IUsuario } from 'src/app/interfaces/IUsuario';
import { ColaboradoresService } from 'src/app/services/colaboradores.service';

@Component({
  selector: 'app-colaborador-update',
  templateUrl: './colaborador-update.component.html',
  styleUrls: ['./colaborador-update.component.css']
})
export class ColaboradorUpdateComponent implements OnInit {

  formGroup!: FormGroup;
 
  constructor(
    @Inject(MAT_DIALOG_DATA) private data:any,
    private matDialogref: MatDialogRef<ColaboradorUpdateComponent>,
    private formBuilder: FormBuilder,
    private authService: AuthorizeService,
    private _colaboradorService: ColaboradoresService,
    private _snackBar: MatSnackBar
  ) { }
  User:IUsuario;
  ngOnInit(): void {
  console.log(this.data.colaborador.id_odoo, "ID ODOO")
  this.buildForm();
  this.formGroup.controls.id.setValue(this.data.colaborador.id);
  this.formGroup.controls.nombres.setValue(this.data.colaborador.nombres);
  this.formGroup.controls.apellidos.setValue(this.data.colaborador.apellidos);
  this.formGroup.controls.curp.setValue(this.data.colaborador.curp);
  this.formGroup.controls.id_odoo.setValue(this.data.colaborador.id_Odoo);
  this.formGroup.controls.email.setValue(this.data.colaborador.email);
  if(this.authService.usuarioData!=null){
    this.User=this.authService.usuarioData;
  }
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      id: new FormControl("", Validators.required),
      nombres: new FormControl("", Validators.required),
      apellidos: new FormControl("", Validators.required),
      curp: new FormControl("", Validators.required),
      id_odoo: new FormControl("", Validators.required),
      email: new FormControl("", Validators.required),
    });
  }

  // initializeFormGroup() {
  //   this.formGroup = this.formBuilder.group({
  //     nombre: "",
  //     apellidos:"",
  //     curp:"",
  //     claveOdoo:"",
  //   });
  // }

  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }

  onSubmit() {
    if(this.formGroup.valid){
        
        this._colaboradorService.UpdateColaborador( this.data.colaborador.id, this.formGroup.value,this.User.correo).subscribe((result) => {
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

import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { IAsignacionGet } from 'src/app/interfaces/iasignacion';
import { IProyectoAsignado } from 'src/app/interfaces/iproyecto-asignado';
import { AsignacionesService } from 'src/app/services/asignaciones.service';
import { ColaboradoresService } from 'src/app/services/colaboradores.service';
import { ColaboradorUpdateComponent } from '../colaborador-update/colaborador-update.component';

@Component({
  selector: 'app-colaborador-details',
  templateUrl: './colaborador-details.component.html',
  styleUrls: ['./colaborador-details.component.css']
})



export class ColaboradorDetailsComponent implements OnInit {

  @ViewChild(MatPaginator) paginator!: MatPaginator;
displayedColumns: string[] = [
  "Proyecto",
  "Clave_Odoo",
  "Porcentaje",
];
displayedColumnsProyectos: string[] = [
  "Titulo",
  "Clave",
];
  asignacion:IAsignacionGet;
  dataSource!: MatTableDataSource<IProyectoAsignado>;
  ProyectosAsignados:IProyectoAsignado[]=[];
  formGroup!: FormGroup;
  nombre_colaborador:string=this.data.colaborador.nombres;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data:any,
    private matDialogref: MatDialogRef<ColaboradorUpdateComponent>,
    private formBuilder: FormBuilder,
    private _colaboradorService: ColaboradoresService,
    private _snackBar: MatSnackBar,
    private _asignacionService:AsignacionesService
  ) { }

  ngOnInit(): void {
    console.log(this.data.colaborador);
  console.log(this.data.colaborador.id_odoo, "ID ODOO")
  this.buildForm();
  this.formGroup.controls.id.setValue(this.data.colaborador.id);
  this.formGroup.controls.nombres.setValue(this.data.colaborador.nombres);
  this.formGroup.controls.apellidos.setValue(this.data.colaborador.apellidos);
  this.formGroup.controls.curp.setValue(this.data.colaborador.curp);
  this.formGroup.controls.id_odoo.setValue(this.data.colaborador.id_Odoo);
  this.formGroup.controls.email.setValue(this.data.colaborador.email);
  this.formGroup.controls.isAdmin.setValue(this.data.colaborador.isAdmin);
  }

  // private RellenarAsignacion(){
  //   this._asignacionService.getAsignacionByIdColaborador(this.data.colaborador.id)
  //   .pipe(
  //     tap((result:IAsignacionGet)=>{
  //       console.log(result);
  //       this.asignacion=result;

  //       this.asignacion.distribuciones.map(x=>{
  //         let proyecto:IProyectoAsignado={
  //           id:x.proyecto.id,
  //           fecha_inicio:x.fecha_Inicio,
  //           fecha_final:x.fecha_Final,
  //           clave:x.proyecto.clave,
  //           titulo:x.proyecto.titulo
  //         }
  //         this.ProyectosAsignados.push(proyecto);
  //         this.dataSource=new MatTableDataSource<IProyectoAsignado>(this.ProyectosAsignados);
  //         this.dataSource.paginator=this.paginator;
  //         this.formGroup.controls.fecha_Inicio.setValue(this.asignacion.fecha_Inicio);
  //         this.formGroup.controls.fecha_Final.setValue(this.asignacion.fecha_Final);
  //       })
  //     })
  //   ).subscribe();
    
  // }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      id: new FormControl("", Validators.required),
      nombres: new FormControl("", Validators.required),
      apellidos: new FormControl("", Validators.required),
      curp: new FormControl("", Validators.required),
      id_odoo: new FormControl("", Validators.required),
      email: new FormControl("", Validators.required),
      isAdmin: new FormControl(true, Validators.required)
      // fecha_Inicio: new FormControl("", Validators.required),
      // fecha_Final: new FormControl("", Validators.required),
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

}

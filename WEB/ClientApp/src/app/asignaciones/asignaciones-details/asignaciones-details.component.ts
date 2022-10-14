import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { IAsignacion } from 'src/app/interfaces/iasignacion';
import { IAsignacionPost } from 'src/app/interfaces/iasignacion-post';
import { IColaborador } from 'src/app/interfaces/Icolaboradores';
import { IProyectoAsignado } from 'src/app/interfaces/iproyecto-asignado';
import { IProyecto } from 'src/app/interfaces/IProyectos';
import { IResponse } from 'src/app/interfaces/iResponse';
import { AsignacionesService } from 'src/app/services/asignaciones.service';
import { ColaboradoresService } from 'src/app/services/colaboradores.service';
import { ProyectosService } from 'src/app/services/proyectos.service';
import { AsignacionesInsertComponent } from '../asignaciones-insert/asignaciones-insert.component';

@Component({
  selector: 'app-asignaciones-details',
  templateUrl: './asignaciones-details.component.html',
  styleUrls: ['./asignaciones-details.component.css']
})
export class AsignacionesDetailsComponent implements OnInit {

  formGroup!: FormGroup;
 
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = [
    "Proyecto",
    "Clave_Odoo",
    "Porcentaje",
  ];

  constructor(
    @Inject(MAT_DIALOG_DATA) private data:any,
    private matDialogref: MatDialogRef<AsignacionesInsertComponent>,
    private formBuilder: FormBuilder,
    private _asignacionService: AsignacionesService,
    private _snackBar: MatSnackBar,
    private _colaboradoresService:ColaboradoresService,
    private _proyectosService:ProyectosService
  ) { 
  }
  
  dataSource!: MatTableDataSource<IProyectoAsignado>;
  ProyectosAsignados:IProyectoAsignado[]=[];
  colaboradores: IColaborador[]=[];
  Proyectos:IProyecto[]=[];
  ProyectoSeleccionado:IProyecto;
  ProyectoId:number=0;
  select:number;
  ngOnInit(): void {

  this.GetColaboradores();
  this.GetProyectos();
  this.buildForm();
  this.rellenarCampos();
  }


  private rellenarCampos(){
    let Asignacion:IAsignacion=this.data.asignacion;

    Asignacion.distribucion.map(x=>{
      let proyecto:IProyectoAsignado={
        id:x.proyecto.id,
        porcentaje:x.porcentaje,
        clave:x.proyecto.clave,
        titulo:x.proyecto.titulo
      }
      this.ProyectosAsignados.push(proyecto);
      this.dataSource=new MatTableDataSource<IProyectoAsignado>(this.ProyectosAsignados);
      this.dataSource.paginator=this.paginator;
    })

      
    };
  

  private GetColaboradores(){
    this._colaboradoresService.getColaboradores()
    .pipe(
      tap((result:IColaborador[])=>{
        this.colaboradores=result;
      })
    ).subscribe();
  }

  private GetProyectos(){
    this._proyectosService.getProyectos()
    .pipe(
      tap((result:IProyecto[])=>{
        this.Proyectos=result;
      })
    ).subscribe();
  }

  public eliminarProyecto(index:number){
    this.ProyectosAsignados.splice(index,1);
    this.dataSource=new MatTableDataSource<IProyectoAsignado>(this.ProyectosAsignados);
  }
  public agregarProyecto(){
    if(this.ProyectosAsignados.find(x=>x.id==this.formGroup.controls['proyectos'].value)){
      this.openSnackBar("No se pueden repetir los proyectos");
      return;
    }

    this.ProyectoSeleccionado=this.Proyectos.filter(x=>x.id==this.formGroup.controls['proyectos'].value)[0];
    
    console.log(this.ProyectoSeleccionado);
    let proyectoA:IProyectoAsignado={
      clave:this.ProyectoSeleccionado.clave,
      id:this.ProyectoSeleccionado.id,
      titulo:this.ProyectoSeleccionado.titulo,
      porcentaje:0
    }

    console.log(proyectoA);


    this.ProyectosAsignados.push(proyectoA);
    console.log(this.ProyectosAsignados);
    this.dataSource=new MatTableDataSource<IProyectoAsignado>(this.ProyectosAsignados);
    this.dataSource.paginator=this.paginator;
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      fecha_inicio: new FormControl(this.data.asignacion.fecha_inicio, Validators.required),
      fecha_final: new FormControl(this.data.asignacion.fecha_final, Validators.required),
      Colaborador: new FormControl(this.data.asignacion.colaborador.nombres, Validators.required),
    });
  }




  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }


}

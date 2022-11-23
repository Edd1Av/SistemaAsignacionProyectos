import { isNgTemplate } from '@angular/compiler';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
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
import { IResponse } from 'src/app/interfaces/IResponse';
import { AsignacionesService } from 'src/app/services/asignaciones.service';
import { ColaboradoresService } from 'src/app/services/colaboradores.service';
import { ProyectosService } from 'src/app/services/proyectos.service';
import { AsignacionesInsertComponent } from '../asignaciones-insert/asignaciones-insert.component';

@Component({
  selector: 'app-asignaciones-update',
  templateUrl: './asignaciones-update.component.html',
  styleUrls: ['./asignaciones-update.component.css']
})
export class AsignacionesUpdateComponent implements OnInit {
  formGroup!: FormGroup;

  myFilter = (d: Date|null): boolean => {
    const day = (d || new Date()).getDay();
    // Prevent Saturday and Sunday from being selected.
    return day !== 0 && day !== 6;
  };

  fechaInicioMin:Date|null = null;
  fechaInicioMax:Date|null = null;

  fechaFinalMin:Date|null = null;
  fechaFinalMax:Date|null = null;
 
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = [
    "Proyecto",
    "Clave_Odoo",
    "Fecha_Inicio",
    "Fecha_Final", 
    "acciones"
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
        fecha_inicio:x.fecha_Inicio,
        fecha_final:x.fecha_Final,
        clave:x.proyecto.clave,
        titulo:x.proyecto.titulo,
        fechaInicioMin:null,
        fechaInicioMax:x.fecha_Final,
        fechaFinalMin:x.fecha_Inicio,
        fechaFinalMax:null,
      }
      this.ProyectosAsignados.push(proyecto);
      this.dataSource=new MatTableDataSource<IProyectoAsignado>(this.ProyectosAsignados);
      this.dataSource.paginator=this.paginator;
    });

  

      
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
      fecha_inicio:null,
      fecha_final:null,
      fechaInicioMin:null,
      fechaInicioMax:null,
      fechaFinalMin:null,
      fechaFinalMax:null,
      // porcentaje:0
    }

    console.log(proyectoA);


    this.ProyectosAsignados.push(proyectoA);
    console.log(this.ProyectosAsignados);
    this.dataSource=new MatTableDataSource<IProyectoAsignado>(this.ProyectosAsignados);
    this.dataSource.paginator=this.paginator;
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      // fecha_inicio: new FormControl(this.data.asignacion.fecha_inicio, Validators.required),
      // fecha_final: new FormControl(this.data.asignacion.fecha_final, Validators.required),
      colaborador: new FormControl({value:this.data.asignacion.colaborador.id, disabled:true}, Validators.required),
      proyectos: new FormControl(""),
    });
  }




  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }

  onSubmit() {
    // let total:number=0;
    // this.ProyectosAsignados.forEach(element => {
    //   total+=element.porcentaje;
    // });
    // if(total!=100){
    //   this.openSnackBar("El porcentaje de asignación a proyectos debe sumar 100");
    //   return;
    // }
    if(this.formGroup.valid){
      let POST:IAsignacionPost={
        id_colaborador:this.formGroup.controls['colaborador'].value,
        // fecha_inicio:this.formGroup.controls['fecha_inicio'].value,
        // fecha_final:this.formGroup.controls['fecha_final'].value,

        proyectos:this.ProyectosAsignados
      }
        this._asignacionService
          .UpdateAsignacion(this.data.asignacion.id,POST)
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

  updateFechaInicio(type: string, event: MatDatepickerInputEvent<Date>, index:number) {
    if(event.value!=null){
      this.ProyectosAsignados[index].fechaFinalMin = new Date(event.value.toISOString());
    }

  }

  updateFechaFinal(type: string, event: MatDatepickerInputEvent<Date>, index:number) {
    if(event.value!=null){
      this.ProyectosAsignados[index].fechaInicioMax = new Date(event.value.toISOString());
    }
   
  }
}

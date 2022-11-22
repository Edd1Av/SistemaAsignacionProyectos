import { ConditionalExpr } from '@angular/compiler';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { IAsignacionPostReal } from 'src/app/interfaces/iasignacion-post';
import { IColaborador } from 'src/app/interfaces/Icolaboradores';
import { IntervaloFecha } from 'src/app/interfaces/IntervaloFechas';
import { IProyectoAsignadoReal } from 'src/app/interfaces/iproyecto-asignado';
import { IProyecto } from 'src/app/interfaces/IProyectos';
import { IResponse } from 'src/app/interfaces/IResponse';
import { IUsuario } from 'src/app/interfaces/IUsuario';
import { AsignacionesService } from 'src/app/services/asignaciones.service';
import { ColaboradoresService } from 'src/app/services/colaboradores.service';
import { ProyectosService } from 'src/app/services/proyectos.service';

@Component({
  selector: 'app-asignaciones-real-insert',
  templateUrl: './asignaciones-real-insert.component.html',
  styleUrls: ['./asignaciones-real-insert.component.css']
})
export class AsignacionesRealInsertComponent implements OnInit {
  formGroup!: FormGroup;
 
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = [
    "Proyecto", 
    "Clave_Odoo",
    "Porcentaje",
    "acciones"
  ];

  
  fechaInicioMin:Date|undefined = undefined;
  fechaInicioMax:Date|undefined = undefined;
  fechaFinalMin:Date|undefined = undefined;
  fechaFinalMax:Date|undefined = undefined;

  intervaloFecha:IntervaloFecha;

  fechaValida:Boolean = false;


  constructor(
    @Inject(MAT_DIALOG_DATA) private data:any,
    private matDialogref: MatDialogRef<AsignacionesRealInsertComponent>,
    private formBuilder: FormBuilder,
    private _asignacionService: AsignacionesService,
    private _snackBar: MatSnackBar,
    private _colaboradoresService:ColaboradoresService,
    private _proyectosService:ProyectosService,
    private _authService:AuthorizeService
  ) { }
  
  dataSource!: MatTableDataSource<IProyectoAsignadoReal>;
  ProyectosAsignados:IProyectoAsignadoReal[]=[];
  colaboradores: IColaborador[]=[];
  Proyectos:IProyecto[]=[];
  ProyectoSeleccionado:IProyecto;
  ProyectoId:number=0;
  Usuario:IUsuario;
  ngOnInit(): void {

    if(this._authService.usuarioData!=null){
      this.Usuario=this._authService.usuarioData;
    }
  console.log(this.Usuario);
  // this.GetColaboradores();
  // this.GetProyectos();
  this.buildForm();
  }

  // private GetColaboradores(){
  //   this._colaboradoresService.getColaboradores()
  //   .pipe(
  //     tap((result:IColaborador[])=>{
  //       this.colaboradores=result;
  //     })
  //   ).subscribe();
  // }

  // private GetProyectos(){
  //   this._proyectosService.getProyectosColaborador(this.Usuario.idUsuario)
  //   .pipe(
  //     tap((result:IProyecto[])=>{
  //       this.Proyectos=result;
  //     })
  //   ).subscribe();
  // }

  public eliminarProyecto(index:number){
    this.ProyectosAsignados.splice(index,1);
    this.dataSource=new MatTableDataSource<IProyectoAsignadoReal>(this.ProyectosAsignados);
  }
  public agregarProyecto(){
    if(this.ProyectosAsignados.find(x=>x.id==this.formGroup.controls['proyectos'].value)){
      this.openSnackBar("No se pueden repetir los proyectos");
      return;
    }

    this.ProyectoSeleccionado=this.Proyectos.filter(x=>x.id==this.formGroup.controls['proyectos'].value)[0];
    
    console.log(this.ProyectoSeleccionado);
    let proyectoA:IProyectoAsignadoReal={
      clave:this.ProyectoSeleccionado.clave,
      id:this.ProyectoSeleccionado.id,
      titulo:this.ProyectoSeleccionado.titulo,
      porcentaje:0,
      // fecha_inicio:new Date(),
      // fecha_final:new Date(),
    }

    console.log(proyectoA);


    this.ProyectosAsignados.push(proyectoA);
    console.log(this.ProyectosAsignados);
    this.dataSource=new MatTableDataSource<IProyectoAsignadoReal>(this.ProyectosAsignados);
    this.dataSource.paginator=this.paginator;
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      fecha_inicio: new FormControl({value:"", disabled:false}, Validators.required),
      fecha_final: new FormControl("", Validators.required),
      // colaborador: new FormControl({value: this.Usuario.idUsuario, disabled:true}, Validators.required),
      proyectos: new FormControl({value:"", disabled:true}),
    });
  }




  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }

  onSubmit() {
    let total:number=0;
    this.ProyectosAsignados.forEach(element => {
      total+=element.porcentaje;
    });
    if(total!=100){
      this.openSnackBar("El porcentaje de asignación a proyectos debe sumar 100");
      return;
    }
    if(this.formGroup.valid){
      let POST:IAsignacionPostReal={
        id_colaborador:this.Usuario.idUsuario,
        fecha_inicio:this.formGroup.controls['fecha_inicio'].value,
        fecha_final:this.formGroup.controls['fecha_final'].value,
        proyectos:this.ProyectosAsignados
      }
        this._asignacionService
          .SetAsignacionReal(POST)
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

  updateFechaInicio(type: string, event: MatDatepickerInputEvent<Date>) {
    if(event.value!=null){
      this.fechaFinalMin = new Date(event.value.toISOString());
    }

  }

  updateFechaFinal(type: string, event: MatDatepickerInputEvent<Date>) {
    if(event.value!=null){
      this.fechaInicioMax = new Date(event.value.toISOString());
    }
   
  }

  confirmarFecha(){
    if(this.formGroup.controls['fecha_inicio'].value && this.formGroup.controls['fecha_final'].value){

      console.log(this.formGroup.controls['fecha_inicio'].value);
      this.intervaloFecha={fechaInicio:this.formGroup.controls['fecha_inicio'].value, fechaFin:this.formGroup.controls['fecha_final'].value}

      this._proyectosService.getProyectosColaborador(this.Usuario.idUsuario, this.intervaloFecha)
    .pipe(
      tap((result:IProyecto[])=>{
        this.Proyectos=result;
        this.formGroup.controls['fecha_inicio'].disable();
      this.formGroup.controls['fecha_final'].disable();
      this.formGroup.controls['proyectos'].enable();

      this.fechaValida = true;
      })
    ).subscribe();
    }
    else{
      this.openSnackBar("Seleccione un rango de fechas");
    }
  }

  cancelarFecha(){
      this.formGroup.controls['fecha_inicio'].enable();
      this.formGroup.controls['fecha_final'].enable();
      this.formGroup.controls['proyectos'].disable();
      this.formGroup.controls['proyectos'].reset();
      this.ProyectosAsignados=[];
      this.dataSource=new MatTableDataSource<IProyectoAsignadoReal>(this.ProyectosAsignados);
      this.dataSource.paginator=this.paginator;
      this.fechaValida = false;


  }

}

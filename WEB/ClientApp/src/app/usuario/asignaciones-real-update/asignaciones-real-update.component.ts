import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { IAsignacionReal } from 'src/app/interfaces/iasignacion';
import { IAsignacionPostReal } from 'src/app/interfaces/iasignacion-post';
import { IColaborador } from 'src/app/interfaces/Icolaboradores';
import { IProyectoAsignadoReal } from 'src/app/interfaces/iproyecto-asignado';
import { IProyecto } from 'src/app/interfaces/IProyectos';
import { IResponse } from 'src/app/interfaces/IResponse';
import { AsignacionesService } from 'src/app/services/asignaciones.service';
import { ColaboradoresService } from 'src/app/services/colaboradores.service';
import { ProyectosService } from 'src/app/services/proyectos.service';
import { AsignacionesRealInsertComponent } from '../asignaciones-real-insert/asignaciones-real-insert.component';

@Component({
  selector: 'app-asignaciones-real-update',
  templateUrl: './asignaciones-real-update.component.html',
  styleUrls: ['./asignaciones-real-update.component.css']
})
export class AsignacionesRealUpdateComponent implements OnInit {

  formGroup!: FormGroup;
 
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = [
    "Proyecto", 
    "Clave_Odoo",
    "Porcentaje",
    "acciones"
  ];

  constructor(@Inject(MAT_DIALOG_DATA) private data:any,
  private matDialogref: MatDialogRef<AsignacionesRealInsertComponent>,
  private formBuilder: FormBuilder,
  private _asignacionService: AsignacionesService,
  private _snackBar: MatSnackBar,
  private _colaboradoresService:ColaboradoresService,
  private _proyectosService:ProyectosService) { }

  dataSource!: MatTableDataSource<IProyectoAsignadoReal>;
  ProyectosAsignados:IProyectoAsignadoReal[]=[];
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

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      fecha_inicio: new FormControl(this.data.asignacionReal.fecha_inicio, Validators.required),
      fecha_final: new FormControl(this.data.asignacionReal.fecha_final, Validators.required),
      colaborador: new FormControl({value:this.data.asignacionReal.colaborador.id, disabled:true}, Validators.required),
      proyectos: new FormControl(""),
    });
  }

  private rellenarCampos(){
    let AsignacionReal:IAsignacionReal=this.data.asignacionReal;

    AsignacionReal.distribucion.map(x=>{
      let proyecto:IProyectoAsignadoReal={
        id:x.proyecto.id,
        clave:x.proyecto.clave,
        titulo:x.proyecto.titulo,
        porcentaje: x.porcentaje
      }
      this.ProyectosAsignados.push(proyecto);
      
    });

    this.dataSource=new MatTableDataSource<IProyectoAsignadoReal>(this.ProyectosAsignados);
    this.dataSource.paginator=this.paginator;
      
    };

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
          id_colaborador:this.formGroup.controls['colaborador'].value,
          fecha_inicio:this.formGroup.controls['fecha_inicio'].value,
          fecha_final:this.formGroup.controls['fecha_final'].value,
          proyectos:this.ProyectosAsignados
        }
          this._asignacionService
            .UpdateAsignacionReal(this.data.asignacionReal.id, POST)
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

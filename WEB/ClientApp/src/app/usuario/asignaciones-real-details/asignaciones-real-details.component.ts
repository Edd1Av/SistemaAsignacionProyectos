import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { IAsignacionReal } from 'src/app/interfaces/iasignacion';
import { IColaborador } from 'src/app/interfaces/Icolaboradores';
import { IProyectoAsignadoReal } from 'src/app/interfaces/iproyecto-asignado';
import { IProyecto } from 'src/app/interfaces/IProyectos';
import { AsignacionesService } from 'src/app/services/asignaciones.service';
import { ColaboradoresService } from 'src/app/services/colaboradores.service';
import { ProyectosService } from 'src/app/services/proyectos.service';
import { AsignacionesRealInsertComponent } from '../asignaciones-real-insert/asignaciones-real-insert.component';

@Component({
  selector: 'app-asignaciones-real-details',
  templateUrl: './asignaciones-real-details.component.html',
  styleUrls: ['./asignaciones-real-details.component.css']
})
export class AsignacionesRealDetailsComponent implements OnInit {

  formGroup!: FormGroup;
 
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = [
    "Proyecto", 
    "Clave_Odoo",
    "Porcentaje"
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
    this.buildForm();
    this.rellenarCampos();
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      fecha_inicio: new FormControl(this.data.asignacionReal.fecha_inicio, Validators.required),
      fecha_final: new FormControl(this.data.asignacionReal.fecha_final, Validators.required),
      colaborador: new FormControl(this.data.asignacionReal.colaborador.nombres+' '+this.data.asignacionReal.colaborador.apellidos, Validators.required)
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

}

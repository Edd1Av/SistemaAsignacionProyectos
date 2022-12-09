import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { DialogoConfirmacionComponent } from '../../dialogo-confirmacion/dialogo-confirmacion.component';
import { IProyecto } from '../../interfaces/IProyectos';
import { ProyectosService } from '../../services/proyectos.service';
import { ProyectosDetailsComponent } from './proyectos-details/proyectos-details.component';
import { ProyectosInsertComponent } from './proyectos-insert/proyectos-insert.component';
import { ProyectosUpdateComponent } from './proyectos-update/proyectos-update.component';

@Component({
  selector: 'app-proyectos',
  templateUrl: './proyectos.component.html',
  styleUrls: ['./proyectos.component.css']
})
export class ProyectosComponent implements OnInit {

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = [
    "titulo",
    "clave",
    "colaboradores",
    "acciones"
  ];


  
 
  constructor(private proyectosService: ProyectosService,
    private dialog: MatDialog,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar
    ) {
     
    }

    proyectos: IProyecto[]=[];
    dataSource!: MatTableDataSource<IProyecto>;
    formGroup: any;
    

  ngOnInit(): void {
    this.actualizarHistorico();
    this.buildForm();
    //this.initializeFormGroup();

  }


  actualizarHistorico() {
    this.proyectosService
      .getProyectos()
      .pipe(
        tap((result) => {
          this.proyectos = result;
          this.dataSource = new MatTableDataSource<IProyecto>(this.proyectos);
          this.dataSource.paginator = this.paginator;
          console.log(this.proyectos);
        })
      )
      .subscribe();
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      buscador: new FormControl(""),
    });
  }

  // initializeFormGroup() {
  //   this.formGroup.setValue({
  //     buscador: "",
  //   });
  // }

  filtrarTabla() {
    this.dataSource.filter = this.formGroup.get("buscador").value;
  }

  openDialogUpdate(proyecto:IProyecto): void {
    let dialog = this.dialog.open(ProyectosUpdateComponent, {
      width: "500px",
      data: {
        proyecto: proyecto
      },
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
    });
  }

  openDialogDetalle(proyecto:IProyecto): void {
    let dialog = this.dialog.open(ProyectosDetailsComponent, {
      width: "500px",
      data: {
        proyecto: proyecto
      },
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
    });
  }

  openDialogInsert(): void {
    let dialog = this.dialog.open(ProyectosInsertComponent, {
      width: "500px",
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
    });
  }

  mostrarDialogo(id:number): void {
    this.dialog
      .open(DialogoConfirmacionComponent, {
        data: `¿Está seguro de eliminar este proyecto?`,
      })
      .afterClosed()
      .subscribe((confirmado: Boolean) => {
        if (confirmado) {
          this.proyectosService.DeleteProyecto(id).subscribe(
            (rs) => {
              if (rs.success) {
                this.actualizarHistorico();
                this.openSnackBar(rs.response);
              } else {
                this.openSnackBar(rs.response);
              }
            },
            (error) => {
              this.openSnackBar(error.response);
            }
          );
        }
      });
  }

  openSnackBar(message:string) {
    this.snackBar.open(message, undefined, {
      duration: 3000,
    });
  }

}




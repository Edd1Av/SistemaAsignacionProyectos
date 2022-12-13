import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { IAsignacionPost } from 'src/app/interfaces/iasignacion-post';
import { IDelete } from 'src/app/interfaces/Icolaboradores';
import { IUsuario } from 'src/app/interfaces/IUsuario';
import { DialogoConfirmacionComponent } from '../../dialogo-confirmacion/dialogo-confirmacion.component';
import { IAsignacion } from '../../interfaces/iasignacion';
import { AsignacionesService } from '../../services/asignaciones.service';
import { AsignacionesDetailsComponent } from './asignaciones-details/asignaciones-details.component';
import { AsignacionesInsertComponent } from './asignaciones-insert/asignaciones-insert.component';
import { AsignacionesUpdateComponent } from './asignaciones-update/asignaciones-update.component';

@Component({
  selector: 'app-asignaciones',
  templateUrl: './asignaciones.component.html',
  styleUrls: ['./asignaciones.component.css']
})
export class AsignacionesComponent implements OnInit {

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = [
    "Colaborador",
    "Proyectos",
    "acciones"
  ];

  constructor(private AsignacionService: AsignacionesService, private dialog: MatDialog,
    private formBuilder: FormBuilder,
    private authService: AuthorizeService,
    private snackBar: MatSnackBar) 
    { }
    User:IUsuario;
    Asignaciones: IAsignacion[]=[];
    dataSource!: MatTableDataSource<IAsignacion>;
    formGroup: any;

  ngOnInit(): void {
    this.actualizarHistorico();
    this.buildForm();
    this.initializeFormGroup();
    if(this.authService.usuarioData!=null){
      this.User=this.authService.usuarioData;
    }
  }

  actualizarHistorico() {
    this.AsignacionService
      .getAsignaciones()
      .pipe(
        tap((result) => {
          console.log();

          this.Asignaciones = result;
          this.dataSource = new MatTableDataSource<IAsignacion>(this.Asignaciones);
          this.dataSource.paginator = this.paginator;
        })
      )
      .subscribe();
  }

  private buildForm() {
    this.formGroup = this.formBuilder.group({
      buscador: new FormControl(""),
    });
  }

  initializeFormGroup() {
    this.formGroup.setValue({
      buscador: "",
    });
  }

  filtrarTabla() {
    this.dataSource.filter = this.formGroup.get("buscador").value;
  }
  
  openDialogUpdate(asignacion:IAsignacion): void {
    let dialog = this.dialog.open(AsignacionesUpdateComponent, {
      width: "800px",
      data: {
        asignacion: asignacion
      },
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
      this.filtrarTabla();
    });
  }

    
  openDialogDetalle(asignacion:IAsignacion): void {
    console.log(asignacion);
    let dialog = this.dialog.open(AsignacionesDetailsComponent, {
      width: "800px",
      data: {
        asignacion: asignacion
      },
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
      this.filtrarTabla();
    });
  }

  openDialogInsert(): void {
    let dialog = this.dialog.open(AsignacionesInsertComponent, {
      width: "800px",
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
      this.filtrarTabla();
    });
  }

  mostrarDialogo(id:number): void {
    let POST:IDelete={
      // fecha_inicio:this.formGroup.controls['fecha_inicio'].value,
      // fecha_final:this.formGroup.controls['fecha_final'].value,
      user:this.User.correo,
      id:id,
    }
    this.dialog
      .open(DialogoConfirmacionComponent, {
        data: `¿Está seguro de eliminar esta asignacion?`,
      })
      .afterClosed()
      .subscribe((confirmado: Boolean) => {
        if (confirmado) {
          this.AsignacionService.DeleteAsignacion(POST).subscribe(
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

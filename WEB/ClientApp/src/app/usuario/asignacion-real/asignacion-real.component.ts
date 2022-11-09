import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { DialogoConfirmacionComponent } from 'src/app/dialogo-confirmacion/dialogo-confirmacion.component';
import { IAsignacionReal } from 'src/app/interfaces/iasignacion';
import { AsignacionesService } from 'src/app/services/asignaciones.service';
import { AsignacionesRealDetailsComponent } from '../asignaciones-real-details/asignaciones-real-details.component';
import { AsignacionesRealInsertComponent } from '../asignaciones-real-insert/asignaciones-real-insert.component';
import { AsignacionesRealUpdateComponent } from '../asignaciones-real-update/asignaciones-real-update.component';

@Component({
  selector: 'app-asignacion-real',
  templateUrl: './asignacion-real.component.html',
  styleUrls: ['./asignacion-real.component.css']
})
export class AsignacionRealComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = [
    "Colaborador",
    "Proyectos",
    "Fecha_Inicio",
    "Fecha_Final",
    "acciones"
  ];

  constructor(private AsignacionService: AsignacionesService, private dialog: MatDialog,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar) 
    { }

    Asignaciones: IAsignacionReal[]=[];
    dataSource!: MatTableDataSource<IAsignacionReal>;
    formGroup: any;

  ngOnInit(): void {
    this.actualizarHistorico();
    this.buildForm();
    this.initializeFormGroup();
  }

  actualizarHistorico() {
    console.log("entro");
    this.AsignacionService
      .getAsignacionesReal()
      .pipe(
        tap((result) => {
          console.log(result);

          this.Asignaciones = result;
          this.dataSource = new MatTableDataSource<IAsignacionReal>(this.Asignaciones);
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
  
  openDialogUpdate(asignacionReal:IAsignacionReal): void {
    let dialog = this.dialog.open(AsignacionesRealUpdateComponent, {
      width: "800px",
      data: {
        asignacionReal: asignacionReal
      },
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
      this.filtrarTabla();
    });
  }

    
  openDialogDetalle(asignacionReal:IAsignacionReal): void {
    console.log(asignacionReal);
    let dialog = this.dialog.open(AsignacionesRealDetailsComponent, {
      width: "800px",
      data: {
        asignacionReal: asignacionReal
      },
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
      this.filtrarTabla();
    });
  }

  openDialogInsert(): void {
    let dialog = this.dialog.open(AsignacionesRealInsertComponent, {
      width: "800px",
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
      this.filtrarTabla();
    });
  }

  mostrarDialogo(id:number): void {
    this.dialog
      .open(DialogoConfirmacionComponent, {
        data: `¿Está seguro de eliminar esta asignacion?`,
      })
      .afterClosed()
      .subscribe((confirmado: Boolean) => {
        if (confirmado) {
          this.AsignacionService.DeleteAsignacionReal(id).subscribe(
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

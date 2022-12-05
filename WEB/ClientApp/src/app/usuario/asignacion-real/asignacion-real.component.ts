import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatCalendar } from '@angular/material/datepicker';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { DialogoConfirmacionComponent } from 'src/app/dialogo-confirmacion/dialogo-confirmacion.component';
import { IAsignacionReal } from 'src/app/interfaces/iasignacion';
import { IUsuario } from 'src/app/interfaces/IUsuario';
import { AsignacionesService } from 'src/app/services/asignaciones.service';
import { AsignacionesRealDetailsComponent } from '../asignaciones-real-details/asignaciones-real-details.component';
import { AsignacionesRealInsertComponent } from '../asignaciones-real-insert/asignaciones-real-insert.component';
import { AsignacionesRealUpdateComponent } from '../asignaciones-real-update/asignaciones-real-update.component';

@Component({
  selector: 'app-asignacion-real',
  templateUrl: './asignacion-real.component.html',
  styleUrls: ['./asignacion-real.component.css'],
  encapsulation:ViewEncapsulation.None
})
export class AsignacionRealComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatCalendar) calendar: MatCalendar<Date>;
  displayedColumns: string[] = [
    "Colaborador",
    "Proyectos",
    "Fecha_Inicio",
    "Fecha_Final",
    "acciones"
  ];
  daysSelected: any[] = [];
  startDate:Date;
  event: any;
  Usuario:IUsuario;
  isSelected = (event: any) => {
    const date =
      event.getFullYear() +
      "-" +
      ("00" + (event.getMonth() + 1)).slice(-2) +
      "-" +
      ("00" + event.getDate()).slice(-2);
    return this.daysSelected.find(x => x == date) ? "selected" : "";
  };
  
  select(event: any, calendar: any) {
    const date =
      event.getFullYear() +
      "-" +
      ("00" + (event.getMonth() + 1)).slice(-2) +
      "-" +
      ("00" + event.getDate()).slice(-2);
    const index = this.daysSelected.findIndex(x => x == date);
    if (index < 0) this.daysSelected.push(date);
    else this.daysSelected.splice(index, 1);
    console.log(this.daysSelected);
    calendar.updateTodaysDate();
  }
  constructor(private AsignacionService: AsignacionesService, private dialog: MatDialog,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private _authService:AuthorizeService) 
    { }

    Asignaciones: IAsignacionReal[]=[];
    dataSource!: MatTableDataSource<IAsignacionReal>;
    formGroup: any;



  ngOnInit(): void {

    if(this._authService.usuarioData!=null){
      this.Usuario=this._authService.usuarioData;
    }
    this.actualizarHistorico();
    this.buildForm();
    this.initializeFormGroup();
  }

  getFechas() {
    this.daysSelected = [];
    let days: String[]=[];
    this.AsignacionService
      .GetFechasFaltantes({"id_colaborador":this.Usuario.idUsuario})
      .subscribe((result) => {
        console.log(result.response);
        if(result.success==true ){
          result.response.forEach(function (value:Date) {
            days.push(value.toString().substring(0,10));
        });
        }
        else{
          this.snackBar.open(result.response);
        }
      console.log(days);
      if(days.length>0){
        this.daysSelected=days;
        console.log(this.daysSelected);
        this.startDate = new Date(this.daysSelected[0]);
        this.startDate.setMinutes(this.startDate.getMinutes() + this.startDate.getTimezoneOffset())
        console.log(this.startDate);
        this.calendar._goToDateInView(this.startDate,"month")
        this.calendar.updateTodaysDate();
      }
      });
  }

  actualizarHistorico() {
    this.AsignacionService
      .getAsignacionesReal(this.Usuario.idUsuario)
      .pipe(
        tap((result) => {
          console.log(result);

          this.Asignaciones = result;
          
          this.dataSource = new MatTableDataSource<IAsignacionReal>(this.Asignaciones);
          this.dataSource.paginator = this.paginator;
        })
      )
      .subscribe();
      this.getFechas();
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

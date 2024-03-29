import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { IColaborador, IDelete } from '../../interfaces/Icolaboradores';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { ColaboradoresService } from '../../services/colaboradores.service';
import { DialogoConfirmacionComponent } from '../../dialogo-confirmacion/dialogo-confirmacion.component';
import { ColaboradorUpdateComponent } from './colaborador-update/colaborador-update.component';
import { ColaboradorInsertComponent } from './colaborador-insert/colaborador-insert.component';
import { ColaboradorDetailsComponent } from './colaborador-details/colaborador-details.component';
import { ChangePasswordComponent } from 'src/api-authorization/change-password/change-password.component';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { IUsuario } from 'src/app/interfaces/IUsuario';

@Component({
  selector: 'app-colaboradores',
  templateUrl: './colaboradores.component.html',
  styleUrls: ['./colaboradores.component.css']
})
export class ColaboradoresComponent implements OnInit {

    @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = [
    "nombre",
    "apellido",
    "email",
    "curp",
    "claveOdoo",
    "acciones"
  ];
  
 
  constructor(private colaboradorService: ColaboradoresService,
    private dialog: MatDialog,
    private formBuilder: FormBuilder,
    private authService: AuthorizeService,
    private snackBar: MatSnackBar
    ) {
     
    }
    User:IUsuario;
    colaboradores: IColaborador[]=[];
    dataSource!: MatTableDataSource<IColaborador>;
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
    this.colaboradorService
      .getColaboradores()
      .pipe(
        tap((result) => {
          this.colaboradores = result;
          console.log(this.colaboradores);
          this.dataSource = new MatTableDataSource<IColaborador>(this.colaboradores);
          this.dataSource.paginator = this.paginator;
          // this.dataSource.filterPredicate = (data, filter: string) => {
          //   return (data.Apellidos.trim().toUpperCase().includes(filter.trim().toUpperCase()));
          //  };
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

  openDialogUpdate(colaborador:IColaborador): void {
    let dialog = this.dialog.open(ColaboradorUpdateComponent, {
      width: "800px",
      data: {
        colaborador: colaborador
      },
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
      this.filtrarTabla();
    });
  }

  openDialogDetalle(colaborador:IColaborador): void {
    let dialog = this.dialog.open(ColaboradorDetailsComponent, {
      width: "800px",
      data: {
        colaborador: colaborador
      },
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      this.actualizarHistorico();
      this.filtrarTabla();
    });
  }


  openDialogInsert(): void {
    let dialog = this.dialog.open(ColaboradorInsertComponent, {
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
        data: `¿Está seguro de eliminar este colaborador?`,
      })
      .afterClosed()
      .subscribe((confirmado: Boolean) => {
        if (confirmado) {
          this.colaboradorService.DeleteColaborador(POST).subscribe(
            (rs) => {
              if (rs.success) {
                this.actualizarHistorico();
                this.openSnackBar(rs.response);
              } else {
                this.openSnackBar(rs.response);
              }
            },
            (error) => {
              this.actualizarHistorico();
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

// import { Component, OnInit } from '@angular/core';

// @Component({
//   selector: 'app-proyectos',
//   templateUrl: './proyectos.component.html',
//   styleUrls: ['./proyectos.component.css']
// })
// export class ProyectosComponent implements OnInit {

//   constructor() { }

//   ngOnInit(): void {
//   }

// }


import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { Icolaboradores } from '../interfaces/Icolaboradores';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { ColaboradoresService } from '../services/colaboradores.service';
import { DialogoConfirmacionComponent } from '../dialogo-confirmacion/dialogo-confirmacion.component';
// @Component({
//   selector: 'app-colaboradores',
//   templateUrl: './colaboradores.component.html',
//   styleUrls: ['./colaboradores.component.css']
// })
// export class ColaboradoresComponent implements OnInit {
  
//   @ViewChild(MatPaginator) paginator!: MatPaginator;
//   displayedColumns: string[] = [
//     "nombre",
//     "acciones"
//   ];
  

//   constructor(private colaboradorService: ColaboradoresService,
//     private dialog: MatDialog,
//     private formBuilder: FormBuilder,
//     private snackBar: MatSnackBar) {
     
//     }

//     colaboradores: Icolaboradores[]=[];
//     dataSource: MatTableDataSource<Icolaboradores> = new MatTableDataSource<Icolaboradores>;
//     formGroup: any;
    

//   ngOnInit(): void {
//     this.actualizarHistorico();
//     this.buildForm();
//     this.initializeFormGroup();

//   }


//   actualizarHistorico() {
//     this.colaboradorService
//       .getColaboradores()
//       .pipe(
//         tap((result) => {
//           this.colaboradores = result;
//           this.dataSource = new MatTableDataSource<Icolaboradores>(this.colaboradores);
//           this.dataSource.paginator = this.paginator;
//           this.dataSource.filterPredicate = (data, filter: string) => {
//             return (data.Apellidos.trim().toUpperCase().includes(filter.trim().toUpperCase()));
//            };
//         })
//       )
//       .subscribe();
//   }

//   private buildForm() {
//     this.formGroup = this.formBuilder.group({
//       buscador: new FormControl(""),
//     });
//   }

//   initializeFormGroup() {
//     this.formGroup.setValue({
//       buscador: "",
//     });
//   }

//   filtrarTabla() {
//     this.dataSource.filter = this.formGroup.get("buscador").value;
//   }

//   openDialog(type:string, color:any): void {
//     // let dialog = this.dialog.open(DialogColorComponent, {
//     //   width: "700px",
//     //   data: {
//     //     type: type,
//     //     color: color
//     //   },
//     //   disableClose: true,
//     // });

//     // dialog.afterClosed().subscribe((result) => {
//     //   this.actualizarHistorico();
//     //   this.filtrarTabla();
//     // });
//   }

//   mostrarDialogo(id:number): void {
//     this.dialog
//       .open(DialogoConfirmacionComponent, {
//         data: `¿Está seguro de eliminar este color?`,
//       })
//       .afterClosed()
//       .subscribe((confirmado: Boolean) => {
//         if (confirmado) {
//           this.colaboradorService.DeleteColaborador(id).subscribe(
//             (rs) => {
//               if (rs.success) {
//                 this.actualizarHistorico();
//                 this.openSnackBar(rs.response);
//               } else {
//                 this.openSnackBar(rs.response);
//               }
//             },
//             (error) => {
//               this.openSnackBar(error.response);
//             }
//           );
//         }
//       });
//   }

//   openSnackBar(message:string) {
//     this.snackBar.open(message, undefined, {
//       duration: 3000,
//     });
//   }

// }
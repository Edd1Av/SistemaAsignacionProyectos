import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import {Ihistorico, IReporte } from 'src/app/interfaces/ireporte';
import { ExcelService } from 'src/app/services/excel.service';
import { ReporteService } from 'src/app/services/reporte.service';
import { IUsuario } from '../interfaces/IUsuario';

@Component({
  selector: 'app-reporte',
  templateUrl: './reporte.component.html',
  styleUrls: ['./reporte.component.scss']
})
export class ReporteComponent implements OnInit {

  myFilter = (d: Date|null): boolean => {
    const day = (d || new Date()).getDay();
    // Prevent Saturday and Sunday from being selected.
    return day !== 0 && day !== 6;
  };

  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
  Reporte: IReporte;
  dataSource!: MatTableDataSource<Ihistorico>;
  constructor(private _snackBar: MatSnackBar,private authService: AuthorizeService,private ReporteService: ReporteService,private excelService: ExcelService) { }
  displayedColumns: string[] = [
    "colaborador",
  ];
  displayedColumnsProyectos: string[] = [
    "Id",
    "Titulo",
    "Dias",
    "Porcentaje",
  ];
  displayedColumnsDiasFaltantes: string[] = [
    "Inicio",
    "Final",
  ];
  panelOpenState = false;
  email:string;
  
  isAdmin:Boolean=false;
  isDevelop:Boolean=false;
  //idusuario:number=0;
  usuario:IUsuario;
  //usuarioLoggeado:Boolean=false;
  Intervalo = new FormGroup({
    start: new FormControl("", Validators.required),
    end: new FormControl("", Validators.required),
  });

  ngOnInit(): void {
    if(this.authService.usuarioData!=null){
      this.usuario=this.authService.usuarioData;
      this.email=this.usuario.correo;
      if(this.usuario.rol=="Administrador"){
        this.isAdmin=true;
            this.isDevelop=false;
      }
      else if(this.usuario.rol=="Desarrollador"){
        this.isAdmin=false;
        this.isDevelop=true;
      }
    }

   
  }


  generateExcel() {

    var fecha_inicio=this.Intervalo.controls['start'].value;
    var fecha_final=this.Intervalo.controls['end'].value;
    this.ReporteService
      .GetReporte({"fecha_inicio":fecha_inicio,
      "fecha_final":fecha_final,"id_colaborador":this.usuario.idUsuario})
      .subscribe((result) => {
        var headers = [[result.response.excel[0].a,result.response.excel[0].b,"","",""]];
        var temp=result.response.excel.shift();
        console.log(result.response.excel);
        this.excelService.exportAsExcelCustomHeaders(
          result.response.excel,
          headers,
          "Asignacion a proyectos"
        );
        // this.openSnackBar("Archivo generado");
      })
  }

  
  Consultar(){
    this.actualizarHistorico();
  }
  async actualizarHistorico() {
    var fecha_inicio=this.Intervalo.controls['start'].value;
    var fecha_final=this.Intervalo.controls['end'].value;
    if(this.Intervalo.valid){
      this.ReporteService
      .GetReporte({"fecha_inicio":fecha_inicio,
      "fecha_final":fecha_final,"id_colaborador":this.usuario.idUsuario})
      .pipe(
        tap((result) => {
          if(result.success==true){
            this.Reporte = result;
            this.Reporte.response.rest.forEach(element => {
              element.diasfaltantes.forEach(d => {
                d.inicio=new Date(d.inicio);
                d.inicio.setMinutes(d.inicio.getMinutes() + d.inicio.getTimezoneOffset());
                if(d.final!=null){
                  d.final=new Date(d.final);
                  d.final.setMinutes(d.final.getMinutes() + d.final.getTimezoneOffset());
                }
  
              });
            });
            this.dataSource = new MatTableDataSource<Ihistorico>(this.Reporte.response.rest);
            this.dataSource.paginator = this.paginator;
            console.log(this.Reporte.response);
          }else{
            this._snackBar.open(result.response.toString(),"",{
              duration: 3000
            });
          }

        })
      )
      .subscribe();
    } 
  }

}

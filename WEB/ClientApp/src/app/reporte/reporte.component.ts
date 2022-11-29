import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import {Ihistorico, IReporte } from 'src/app/interfaces/ireporte';
import { ExcelService } from 'src/app/services/excel.service';
import { ReporteService } from 'src/app/services/reporte.service';

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
  constructor(private authService: AuthorizeService,private ReporteService: ReporteService,private excelService: ExcelService) { }
  displayedColumns: string[] = [
    "colaborador",
  ];
  displayedColumnsProyectos: string[] = [
    "Id",
    "Titulo",
    "Dias",
    "Porcentaje",
  ];
  email:string;
  isAdmin:Boolean=false;
  isDevelop:Boolean=false;
  idusuario:number=0;
  usuarioLoggeado:Boolean=false;
  Intervalo = new FormGroup({
    start: new FormControl("", Validators.required),
    end: new FormControl("", Validators.required),
  });

  ngOnInit(): void {
    this.usuarioLoggeado = this.authService.isLogged();
    this.authService.changeLoginStatus.subscribe((value)=>{
      if(value){
        this.usuarioLoggeado=true;
        this.email=value.correo;
        if(value.rol=="Administrador"){
          this.isAdmin=true;
          this.isDevelop=false;
          this.idusuario=value.idUsuario;
        }
        if(value.rol=="Desarrollador"){
          this.isAdmin=false;
          this.isDevelop=true;
          this.idusuario=value.idUsuario;
        }
      }
      else{
        this.usuarioLoggeado = false;
        this.isAdmin=false;
        this.isDevelop=false;

      }
      
    })
    // this.actualizarHistorico();
  }


  generateExcel() {

    var fecha_inicio=this.Intervalo.controls['start'].value;
    var fecha_final=this.Intervalo.controls['end'].value;
    this.ReporteService
      .GetReporte({"fecha_inicio":fecha_inicio,
      "fecha_final":fecha_final,"id_colaborador":this.idusuario})
      .subscribe((result) => {
        var headers = [[result.response.excel[0].a,result.response.excel[0].b,"","",""]];
        var temp=result.response.excel.shift();
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
      "fecha_final":fecha_final,"id_colaborador":this.idusuario})
      .pipe(
        tap((result) => {
          this.Reporte = result;
          this.dataSource = new MatTableDataSource<Ihistorico>(this.Reporte.response.rest);
          this.dataSource.paginator = this.paginator;
          console.log(this.paginator);
          console.log(this.dataSource);
        })
      )
      .subscribe();
    }
    console.log(this.idusuario);
 
  }

}

import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import {Ihistorico, IReporte } from 'src/app/interfaces/ireporte';
import { ReporteService } from 'src/app/services/reporte.service';

@Component({
  selector: 'app-reporte',
  templateUrl: './reporte.component.html',
  styleUrls: ['./reporte.component.scss']
})
export class ReporteComponent implements OnInit {
  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
  Reporte: IReporte;
  dataSource!: MatTableDataSource<Ihistorico>;
  constructor(private ReporteService: ReporteService) { }
  displayedColumns: string[] = [
    "colaborador",
  ];
  displayedColumnsProyectos: string[] = [
    "Id",
    "Titulo",
    "Dias",
    "Porcentaje",
  ];
  Intervalo = new FormGroup({
    start: new FormControl(new Date()),
    end: new FormControl(new Date()),
  });

  ngOnInit(): void {
    // this.actualizarHistorico();
  }
  
  Consultar(){
    this.actualizarHistorico();
  }
  async actualizarHistorico() {
    var fecha_inicio=this.Intervalo.controls['start'].value;
    var fecha_final=this.Intervalo.controls['end'].value;
    if(fecha_inicio!=null&&fecha_final!=null){
      this.ReporteService
      .GetReporte({"fecha_inicio":fecha_inicio,
      "fecha_final":fecha_final})
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
 
  }

}

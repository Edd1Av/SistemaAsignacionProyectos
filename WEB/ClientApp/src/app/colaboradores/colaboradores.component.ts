import { Component, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { Icolaboradores } from '../interfaces/Icolaboradores';
import {
  MatTableDataSource
} from "@angular/material/table";
@Component({
  selector: 'app-colaboradores',
  templateUrl: './colaboradores.component.html',
  styleUrls: ['./colaboradores.component.css']
})
export class ColaboradoresComponent implements OnInit {
  // dataSource: MatTableDataSource<Icolaboradores>;
  constructor() { }

  ngOnInit(): void {
  }




}

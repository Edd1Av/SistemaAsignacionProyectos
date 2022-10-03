import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ColaboradoresComponent } from './colaboradores.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule,} from "@angular/material/table";
import { MatFormFieldModule } from '@angular/material/form-field';
import { ColaboradorInsertComponent } from './colaborador-insert/colaborador-insert.component';
import { MatDialogModule } from '@angular/material/dialog';


const routes: Routes = [
  { path: '', component: ColaboradoresComponent }
];

@NgModule({
  declarations: [
    ColaboradoresComponent,
    ColaboradorInsertComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatPaginatorModule,
    MatIconModule,
    MatFormFieldModule,
    MatTableModule,
    MatDialogModule,
    RouterModule.forChild(routes)
  ]
})
export class ColaboradoresModule { }

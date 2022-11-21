import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ReporteComponent } from './reporte.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatDatepicker, MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';


const routes: Routes = [
  { path: '', component: ReporteComponent }
];

@NgModule({
  declarations: [
    ReporteComponent,
    
  ],
  imports: [
    CommonModule,
    MatProgressBarModule,
    MatPaginatorModule,
    MatTableModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatIconModule,
    RouterModule.forChild(routes)
  ]
})
export class ReporteModule { }

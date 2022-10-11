import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AsignacionesComponent } from './asignaciones.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { AsignacionesInsertComponent } from './asignaciones-insert/asignaciones-insert.component';
import { AsignacionesUpdateComponent } from './asignaciones-update/asignaciones-update.component';
import { MatSelect, MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from "@angular/material/datepicker";

const routes: Routes = [
  { path: '', component: AsignacionesComponent }
];

@NgModule({
  declarations: [
    AsignacionesComponent,
    AsignacionesInsertComponent,
    AsignacionesUpdateComponent
  ],
  imports: [
    ReactiveFormsModule,
    FormsModule,
    MatPaginatorModule,
    MatIconModule,
    MatFormFieldModule,
    MatTableModule,
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    FlexLayoutModule,
    MatSnackBarModule,
    CommonModule,
    MatDatepickerModule,
    RouterModule.forChild(routes)
  ]
})
export class AsignacionesModule { }

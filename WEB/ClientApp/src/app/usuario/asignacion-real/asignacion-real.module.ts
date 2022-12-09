import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AsignacionRealComponent } from './asignacion-real.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { AsignacionesRealDetailsComponent } from './asignaciones-real-details/asignaciones-real-details.component';
import { AsignacionesRealInsertComponent } from './asignaciones-real-insert/asignaciones-real-insert.component';
import { AsignacionesRealUpdateComponent } from './asignaciones-real-update/asignaciones-real-update.component';
import { MatNativeDateModule } from '@angular/material/core';
import {MatMenuModule} from '@angular/material/menu';
import {MatExpansionModule} from '@angular/material/expansion';
const routes: Routes = [
  { path: '', component: AsignacionRealComponent }
];

@NgModule({
  declarations: [
    AsignacionRealComponent,
    AsignacionesRealInsertComponent,
    AsignacionesRealUpdateComponent,
    AsignacionesRealDetailsComponent
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
    MatNativeDateModule,
    MatMenuModule,
    MatExpansionModule,
    RouterModule.forChild(routes)
  ]
})
export class AsignacionRealModule { }

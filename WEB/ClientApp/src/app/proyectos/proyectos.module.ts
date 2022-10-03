import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ProyectosComponent} from './proyectos.component';
import { ProyectosInsertComponent } from './proyectos-insert/proyectos-insert.component';
import { ProyectosUpdateComponent } from './proyectos-update/proyectos-update.component';
import { ProyectosDetailsComponent } from './proyectos-details/proyectos-details.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule,} from "@angular/material/table";
import { MatFormFieldModule} from '@angular/material/form-field';
import { MatDialogModule } from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import { MatInputModule } from "@angular/material/input";
import { FlexLayoutModule } from "@angular/flex-layout";
import {MatSnackBarModule} from '@angular/material/snack-bar';


const routes: Routes = [
  { path: '', component: ProyectosComponent }
];

@NgModule({
  declarations: [
    ProyectosComponent,
    ProyectosInsertComponent,
    ProyectosUpdateComponent,
    ProyectosDetailsComponent
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
    MatButtonModule,
    MatInputModule,
    FlexLayoutModule,
    MatSnackBarModule,
    RouterModule.forChild(routes)
  ]
})
export class ProyectosModule { }

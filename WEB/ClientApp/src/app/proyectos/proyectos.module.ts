import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ProyectosComponent } from './proyectos.component';


const routes: Routes = [
  { path: '', component: ProyectosComponent }
];

@NgModule({
  declarations: [
    ProyectosComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class ProyectosModule { }

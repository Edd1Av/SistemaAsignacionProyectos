import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuardAdministrador} from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { LayoutComponent } from './layout/layout/layout.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatPaginatorModule } from '@angular/material/paginator';
import {MatTableModule,} from "@angular/material/table";
import { MatFormFieldModule } from '@angular/material/form-field';
import {MatIcon, MatIconModule} from '@angular/material/icon';
import { DialogoConfirmacionComponent } from './dialogo-confirmacion/dialogo-confirmacion.component';
import { MatDialogModule } from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import { MatInputModule } from "@angular/material/input";
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { LoginComponent } from 'src/api-authorization/login/login.component';
import { AuthorizeGuardDesarrollador } from 'src/api-authorization/authorizeDesarrollador.guard';
import { FlexLayoutModule } from '@angular/flex-layout';
import {MatProgressBarModule} from '@angular/material/progress-bar'
import {MatMenuModule} from '@angular/material/menu';
import { ChangePasswordComponent } from 'src/api-authorization/change-password/change-password.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CommonModule } from '@angular/common';



@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LayoutComponent,
    DialogoConfirmacionComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    MatPaginatorModule,
    MatTableModule,
    ApiAuthorizationModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatIconModule,
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    FlexLayoutModule,
    MatNativeDateModule,
    MatProgressBarModule,
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    MatSnackBarModule,
    MatDatepickerModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, canActivate:[AuthorizeGuardAdministrador] },
      { path: 'home', component: HomeComponent, canActivate:[AuthorizeGuardAdministrador] },
      { path: 'login', component: LoginComponent},
      { path: 'proyectos', loadChildren: () => import('./administador/proyectos/proyectos.module').then(m => m.ProyectosModule), canActivate:[AuthorizeGuardAdministrador] },
      { path: 'asignaciones', loadChildren: () => import('./administador/asignaciones/asignaciones.module').then(m => m.AsignacionesModule), canActivate:[AuthorizeGuardAdministrador]},
      { path: 'colaboradores', loadChildren: () => import('./administador/colaboradores/colaboradores.module').then(m => m.ColaboradoresModule), canActivate:[AuthorizeGuardAdministrador] },
      { path: 'asignacionReal', loadChildren: () => import('./usuario/asignacion-real/asignacion-real.module').then(m => m.AsignacionRealModule), canActivate:[AuthorizeGuardDesarrollador] },
      { path: 'reporte', loadChildren: () => import('./reporte/reporte.module').then(m => m.ReporteModule) },
      
    ]),
    BrowserAnimationsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    AuthorizeGuardDesarrollador,
    AuthorizeGuardAdministrador
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AddAdministradorComponent } from 'src/api-authorization/add-administrador/add-administrador.component';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { ChangePasswordComponent } from 'src/api-authorization/change-password/change-password.component';
import { DeleteAdministradorComponent } from 'src/api-authorization/delete-administrador/delete-administrador.component';
import { IUsuario } from 'src/app/interfaces/IUsuario';
import { LoaderService } from 'src/app/loader/loader.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css', './layout.component.scss']
})
export class LayoutComponent implements OnInit {

  constructor(
    private dialog: MatDialog,
    private authService: AuthorizeService,
    public loaderService:LoaderService) { }

  usuario:IUsuario;
  
  email:string;
  usuarioLoggeado:Boolean;
  isAdmin:Boolean=false;
  isDevelop:Boolean=false;
  isNotRoot:Boolean;
  ngOnInit(): void {
    this.usuarioLoggeado = this.authService.isLogged();
    if(this.authService.usuarioData!=null){
      this.usuario=this.authService.usuarioData;
      this.email = this.usuario.correo;
      if(this.usuario.rol=="Administrador"){
        this.isAdmin=true;
        this.isDevelop=false;
      }
      else if(this.usuario.rol=="Desarrollador"){
        this.isAdmin=false;
        this.isDevelop=true;
      }
      else{
        this.isAdmin=false;
        this.isDevelop=false;
      }
    }

    if(this.email == "admin@admin.com"){
      this.isNotRoot = false;
    }
    else{
      this.isNotRoot = true;
    }
  }


  salir(){
    this.authService.logout();
  }

  openDialog(): void {
    let dialog = this.dialog.open(ChangePasswordComponent, {
      width: "500px",
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      
    });
  }

  openDialogAdmin(): void {
    let dialog = this.dialog.open(AddAdministradorComponent, {
      width: "400px",
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      
    });
  }

  openDialogDeleteAdmin(): void {
    let dialog = this.dialog.open(DeleteAdministradorComponent, {
      width: "400px",
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      
    });
  }
}

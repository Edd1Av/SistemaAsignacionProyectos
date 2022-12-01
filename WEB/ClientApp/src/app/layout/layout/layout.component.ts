import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { ChangePasswordComponent } from 'src/api-authorization/change-password/change-password.component';
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

  usuario:any;
  
  email:string;
  usuarioLoggeado:Boolean;
  isAdmin:Boolean=false;
  isDevelop:Boolean=false;;
  ngOnInit(): void {
    this.usuarioLoggeado = this.authService.isLogged();
    this.authService.changeLoginStatus.subscribe((value)=>{
      if(value){
        this.usuarioLoggeado=true;
        this.email=value.correo;
        if(value.rol=="Administrador"){
          this.isAdmin=true;
          this.isDevelop=false;
          
        }
        if(value.rol=="Desarrollador"){
          this.isAdmin=false;
          this.isDevelop=true;
        }
      }
      else{
        this.usuarioLoggeado = false;
        this.isAdmin=false;
        this.isDevelop=false;

      }
      
    })
  }


  salir(){
    this.authService.logout();
  }

  openDialog(): void {
    let dialog = this.dialog.open(ChangePasswordComponent, {
      width: "400px",
      disableClose: true,
    });
    dialog.afterClosed().subscribe((result) => {
      
    });
  }
}

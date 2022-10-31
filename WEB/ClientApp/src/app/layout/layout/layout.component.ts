import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { IUsuario } from 'src/app/interfaces/IUsuario';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css', './layout.component.scss']
})
export class LayoutComponent implements OnInit {

  constructor(private authService: AuthorizeService) { }

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
}

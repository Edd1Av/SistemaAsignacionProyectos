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
  ngOnInit(): void {
    this.usuario=this.authService.isLoggedIn();
    console.log(this.usuario);
    if(typeof(this.usuario)!="boolean"){
      this.email = this.usuario.correo
    }
  }


  salir(){
    this.authService.logout();
  }
}
